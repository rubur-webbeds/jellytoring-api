using jellytoring_api.Models.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace jellytoring_api.Service.Users
{
    public interface IUsersService
    {
        Task<User> CreateAsync(CreateUser user);
        Task<User> GetAsync(uint id);
        Task<IEnumerable<User>> GetAllAsync();
    }
}