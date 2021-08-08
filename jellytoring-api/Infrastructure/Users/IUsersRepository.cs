using jellytoring_api.Models.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace jellytoring_api.Infrastructure.Users
{
    public interface IUsersRepository
    {
        Task<uint> CreateAsync(CreateUser user);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetAsync(uint id);
    }
}