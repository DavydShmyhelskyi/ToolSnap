using Domain.Models.Roles;
using Domain.Models.ToolAssignments;

namespace Domain.Models.Users
{
    public class User
    {
        public UserId Id { get; }
        public string FullName { get; private set; }
        public string Email { get; private set; }
        public RoleId RoleId { get; private set; }
        public bool IsActive { get; private set; }
        public string PasswordHash { get; private set; }
        public DateTime CreatedAt { get; }

        // navigation properties
        public Role? Role { get; private set; }
        public IReadOnlyCollection<ToolAssignment> ToolAssignments => _toolAssignments;
        private readonly List<ToolAssignment> _toolAssignments = new();
        private User(UserId id, string fullName, string email, RoleId roleId, string passwordHash, bool isActive)
        {
            Id = id;
            FullName = fullName;
            Email = email;
            RoleId = roleId;
            PasswordHash = passwordHash;
            IsActive = isActive;
            CreatedAt = DateTime.UtcNow;
        }

        public static User New(string fullName, string email, RoleId roleId, string password)
        {
            return new User(
                UserId.New(),
                fullName.Trim(),
                email.Trim(),
                roleId,
                BCrypt.Net.BCrypt.HashPassword(password),
                true);
        }
        public void Update(string fullName, string email)
        {
            FullName = fullName.Trim();
            Email = email.Trim();
        }
        public void ChangePassword(string newPassword)
            => PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);

        public bool VerifyPassword(string password)
            => BCrypt.Net.BCrypt.Verify(password, PasswordHash);

        public void Activate() => IsActive = true;
        public void Deactivate() => IsActive = false;
    }
}