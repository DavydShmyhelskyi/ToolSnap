using Domain.Models.Roles;
using Domain.Models.ToolAssignments;

namespace Domain.Models.Users
{
    public class User
    {
        public UserId Id { get; }
        public string FullName { get; private set; }
        public string Email { get; private set; }
        public bool ConfirmedEmail { get; private set; }
        public RoleId RoleId { get; private set; }
        public bool IsActive { get; private set; }
        public string PasswordHash { get; private set; }
        public DateTime CreatedAt { get; }

        // navigation properties
        public Role? Role { get; private set; }
        public IEnumerable<ToolAssignment> ToolAssignments { get; private set; } = new List<ToolAssignment>();

        private User(UserId id, string fullName, string email, bool confirmedEmail, RoleId roleId, string passwordHash, bool isActive)
        {
            Id = id;
            FullName = fullName;
            Email = email;
            ConfirmedEmail = confirmedEmail;
            RoleId = roleId;
            PasswordHash = passwordHash;
            IsActive = isActive;
            CreatedAt = DateTime.UtcNow;
        }

        public static User New(string fullName, string email, RoleId roleId, string password, bool isActive)
        {
            return new User(
                UserId.New(),
                fullName.Trim(),
                email.Trim(),
                false,
                roleId,
                BCrypt.Net.BCrypt.HashPassword(password),
                isActive);
        }
        public void Update(string fullName, string email, bool confirmedEmail, RoleId roleId)
        {
            FullName = fullName.Trim();
            Email = email.Trim();
            ConfirmedEmail = false;
            RoleId = roleId;
        }
        public void ConfirmEmail() => ConfirmedEmail = true;
        public void ChangePassword(string newPassword)
            => PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);

        public bool VerifyPassword(string password)
            => BCrypt.Net.BCrypt.Verify(password, PasswordHash);

        public void Activate() => IsActive = true;
        public void Deactivate() => IsActive = false;
    }
}