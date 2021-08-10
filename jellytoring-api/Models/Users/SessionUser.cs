using System.Collections.Generic;

namespace jellytoring_api.Models.Users
{
    public class SessionUser
    {
        public string Email;
        public string FullName;
        public IList<Role> Roles;
    }
}