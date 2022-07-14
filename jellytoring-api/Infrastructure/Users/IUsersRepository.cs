using jellytoring_api.Models.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace jellytoring_api.Infrastructure.Users
{
    public interface IUsersRepository
    {
        Task<uint> CreateAsync(CreateUser user);
        Task<IEnumerable<UserDetails>> GetAllAsync();
        Task<User> GetAsync(uint id);
        Task<CreateSessionUser> GetAsync(string email);
        Task<SessionUser> GetUserRolesAsync(string email);
        Task<bool> UpdateRoleAsync(int userId, Role role);
    }
}