using Application.Common.Interfaces.Queries;
using Domain.Models.Locations;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Queries
{
    public class LocationsQueries(ApplicationDbContext context) : ILocationsQueries
    {
        public async Task<IReadOnlyList<Location>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await context.Locations.ToListAsync(cancellationToken);
        }

        public async Task<Option<Location>> GetByIdAsync(LocationId locationId, CancellationToken cancellationToken)
        {
            var location = await context.Locations.FirstOrDefaultAsync(l => l.Id == locationId, cancellationToken);
            return location != null ? Option<Location>.Some(location) : Option<Location>.None;
        }

        public async Task<Option<Location>> GetByTitleAsync(string name, CancellationToken cancellationToken)
        {
            var location = await context.Locations.FirstOrDefaultAsync(l => l.Name == name, cancellationToken);
            return location != null ? Option<Location>.Some(location) : Option<Location>.None;
        }

        public async Task<IReadOnlyList<Location>> GetByTypeAsync(LocationTypeId locationTypeId, CancellationToken cancellationToken)
        {
            return await context.Locations.Where(l => l.LocationTypeId == locationTypeId).ToListAsync(cancellationToken);
        }
        public async Task<Option<Location>> GetNearestAsync(
            double latitude,
            double longitude,
            double radiusMeters,
            CancellationToken cancellationToken)
        {
            // ~111 320 м в одному градусі широти
            const double metersPerDegreeLat = 111_320.0;

            var latRad = latitude * Math.PI / 180.0;

            // Для довготи множимо ще на cos(lat)
            var metersPerDegreeLon = metersPerDegreeLat * Math.Cos(latRad);

            var latDelta = radiusMeters / metersPerDegreeLat;
            var lonDelta = radiusMeters / metersPerDegreeLon;

            var minLat = latitude - latDelta;
            var maxLat = latitude + latDelta;
            var minLon = longitude - lonDelta;
            var maxLon = longitude + lonDelta;

            // 1) Фільтруємо по bounding box в БД
            var candidates = await context.Locations
                .Where(l =>
                    l.Latitude >= minLat && l.Latitude <= maxLat &&
                    l.Longitude >= minLon && l.Longitude <= maxLon)
                .ToListAsync(cancellationToken);

            if (candidates.Count == 0)
                return Option<Location>.None;

            // 2) В пам’яті рахуємо точну відстань і знаходимо найближчу
            const double earthRadius = 6_371_000; // м

            double DegToRad(double deg) => deg * Math.PI / 180.0;

            double DistanceMeters(double lat1, double lon1, double lat2, double lon2)
            {
                var dLat = DegToRad(lat2 - lat1);
                var dLon = DegToRad(lon2 - lon1);

                var rLat1 = DegToRad(lat1);
                var rLat2 = DegToRad(lat2);

                var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                        Math.Cos(rLat1) * Math.Cos(rLat2) *
                        Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

                var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

                return earthRadius * c;
            }

            Location? nearest = null;
            double nearestDistance = double.MaxValue;

            foreach (var loc in candidates)
            {
                var distance = DistanceMeters(latitude, longitude, loc.Latitude, loc.Longitude);

                if (distance <= radiusMeters && distance < nearestDistance)
                {
                    nearest = loc;
                    nearestDistance = distance;
                }
            }

            return nearest is null
                ? Option<Location>.None
                : Option<Location>.Some(nearest);
        }
    }
}
