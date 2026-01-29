using Domain.Models.Roles;

namespace Domain.Models.Users
{
    public class User
    {
        public UserId Id { get; }
        public string FullName { get; private set; }
        public string Email { get; private set; }
        public RoleId RoleId { get; private set; }
        public bool IsActive { get; private set; }
        public DateTimeOffset CreatedAt { get; }

        private User(UserId id, string fullName, string email, RoleId roleId, bool isActive, DateTimeOffset createdAt)
        {
            Id = id;
            FullName = fullName;
            Email = email;
            RoleId = roleId;
            IsActive = isActive;
            CreatedAt = createdAt;
        }

        public static User New(string fullName, string email, RoleId roleId)
        {
            if (string.IsNullOrWhiteSpace(fullName)) throw new ArgumentException("FullName is required.", nameof(fullName));
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email is required.", nameof(email));
            if (roleId is null) throw new ArgumentNullException(nameof(roleId));

            return new User(UserId.New(), fullName.Trim(), email.Trim(), roleId, true, DateTimeOffset.UtcNow);
        }

        public void Activate() => IsActive = true;
        public void Deactivate() => IsActive = false;
    }
}