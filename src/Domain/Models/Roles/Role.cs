namespace Domain.Models.Roles
{
    public class Role
    {
        public RoleId Id { get; }
        public string Name { get; private set; }
        public DateTimeOffset CreatedAt { get; }

        private Role(RoleId id, string name, DateTimeOffset createdAt)
        {
            Id = id;
            Name = name;
            CreatedAt = createdAt;
        }

        public static Role New(string name)
        {
            return new Role(RoleId.New(), name.Trim().ToLower(), DateTimeOffset.UtcNow);
        }
        public void Update(string name)
        {
            Name = name.Trim().ToLower();
        }
    }
}