using jellytoring_api.Infrastructure.Users;
using jellytoring_api.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;

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
            var hashedPassword = BC.HashPassword(user.Password);
            user.Password = hashedPassword;
            var userId = await _usersRepository.CreateAsync(user);

            return userId == 0 ? null : await GetAsync(userId);
        }
    }
}
