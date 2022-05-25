using jellytoring_api.Infrastructure.Users;
using jellytoring_api.Models.Users;
using jellytoring_api.Service.Email;
using System.Collections.Generic;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;

namespace jellytoring_api.Service.Users
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IEmailConfirmationService _emailConfirmationService;

        public UsersService(IUsersRepository usersRepository, IEmailConfirmationService emailConfirmationService)
        {
            _usersRepository = usersRepository;
            _emailConfirmationService = emailConfirmationService;
        }

        public Task<IEnumerable<User>> GetAllAsync() => _usersRepository.GetAllAsync();

        public Task<User> GetAsync(uint id) => _usersRepository.GetAsync(id);
        public Task<CreateSessionUser> GetAsync(string email) => _usersRepository.GetAsync(email);

        public async Task<User> CreateAsync(CreateUser user)
        {
            // check if user email already exists
            var existingUser = await _usersRepository.GetAsync(user.Email);
            if(existingUser is not null)
            {
                return null;
            }

            var hashedPassword = BC.HashPassword(user.Password);
            user.Password = hashedPassword;
            var userId = await _usersRepository.CreateAsync(user);

            if(userId != 0)
            {
                await _emailConfirmationService.SendEmailConfirmationAsync(user.Email, userId);
                return await GetAsync(userId);
            }

            return null;
        }
    }
}
