using Dapper;
using jellytoring_api.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jellytoring_api.Infrastructure.Users
{
    public class UsersRepository : IUsersRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public UsersRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public Task<IEnumerable<User>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetAsync(uint id)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            return (await connection.QueryAsync<User>(UsersQueries.Get, new { id })).FirstOrDefault();
        }

        public async Task<CreateSessionUser> GetAsync(string email)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            return (await connection.QueryAsync<CreateSessionUser>(UsersQueries.GetByEmail, new { email })).FirstOrDefault();
        }

        public async Task<SessionUser> GetUserRolesAsync(string email)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            var result = await connection.QueryAsync(UsersQueries.GetUserRoles, new { email });

            return Slapper.AutoMapper.MapDynamic<SessionUser>(result).FirstOrDefault();
        }

        public async Task<uint> CreateAsync(CreateUser user)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            return await connection.ExecuteScalarAsync<uint>(UsersQueries.Create, user);
        }
    }
}
