using Domain.Models.Users;

namespace Domain.Models.Roles
{
    public class Role
    {
        public RoleId Id { get; }
        public string Title { get; private set; }

        // navigation properties
        public IEnumerable<User> Users { get; private set; } = new List<User>();

        private Role(RoleId id, string title)
        {
            Id = id;
            Title = title;
        }

        public static Role New(string title)
        {
            return new Role(RoleId.New(), title.Trim().ToLower());
        }
        public void Update(string title)
        {
            Title = title.Trim().ToLower();
        }
    }
}