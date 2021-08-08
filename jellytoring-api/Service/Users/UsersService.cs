using jellytoring_api.Infrastructure.Users;
using jellytoring_api.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jellytoring_api.Service.Users
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;

        public UsersService(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public Task<IEnumerable<User>> GetAllAsync() => _usersRepository.GetAllAsync();

        public Task<User> GetAsync(uint id) => _usersRepository.GetAsync(id);

        public async Task<User> CreateAsync(CreateUser user)
        {
            // TODO: encrypt password
            var userId = await _usersRepository.CreateAsync(user);

            return userId == 0 ? null : await GetAsync(userId);
        }
    }
}
