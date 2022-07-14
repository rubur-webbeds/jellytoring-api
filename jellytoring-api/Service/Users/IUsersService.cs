using jellytoring_api.Models.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace jellytoring_api.Service.Users
{
    public interface IUsersService
    {
        Task<User> CreateAsync(CreateUser user);
        Task<User> GetAsync(uint id);
        Task<CreateSessionUser> GetAsync(string email);
        Task<IEnumerable<UserDetails>> GetAllAsync();
        Task<bool> UpdateRoleAsync(int userId, Role role);
    }
}