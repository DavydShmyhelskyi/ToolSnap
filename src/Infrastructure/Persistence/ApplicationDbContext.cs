using Application.Common.Interfaces;
using Domain.Models.DetectedTools;
using Domain.Models.Locations;
using Domain.Models.PhotoSessions;
using Domain.Models.Roles;
using Domain.Models.ToolAssignments;
using Domain.Models.Tools;
using Domain.Models.Users;
using Domain.Models.ToolPhotos;
using Domain.Models.ToolInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Reflection;

namespace Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
    : DbContext(options), IApplicationDbContext
{
    public DbSet<Role> Roles { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<LocationType> LocationTypes { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Tool> Tools { get; set; }
    public DbSet<ToolStatus> ToolStatuses { get; set; }
    public DbSet<ToolAssignment> ToolAssignments { get; set; }
    public DbSet<ToolPhoto> ToolPhotos { get; set; }
    public DbSet<PhotoType> PhotoTypes { get; set; }
    public DbSet<ActionType> ActionTypes { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Model> Models { get; set; }
    public DbSet<ToolType> ToolTypes { get; set; }
    public DbSet<PhotoSession> PhotoSessions { get; set; }
    public DbSet<PhotoForDetection> PhotosForDetection { get; set; }
    public DbSet<DetectedTool> DetectedTools { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    public async Task<IDbTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
    {
        var transaction = await Database.BeginTransactionAsync(cancellationToken);

        return transaction.GetDbTransaction();
    }
}