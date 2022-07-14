﻿using Dapper;
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

        public async Task<IEnumerable<UserDetails>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            return await connection.QueryAsync<UserDetails>(UsersQueries.GetAll);
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

            var userId = await connection.ExecuteScalarAsync<uint>(UsersQueries.Create, user);
            // TODO: place this in the RoleService
            if(userId != 0)
            {
                var userRoleId = await connection.QueryAsync<uint>(UsersQueries.GetUserRoleId);
                await connection.ExecuteScalarAsync(UsersQueries.AddRoleToUser, new { userId, userRoleId });
            }

            return userId;
        }

        public async Task<bool> UpdateRoleAsync(int userId, Role role)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            var userIdParam = command.CreateParameter();
            userIdParam.ParameterName = "@UserId";
            userIdParam.Value = userId;
            var roleIdParam = command.CreateParameter();
            roleIdParam.ParameterName = "@RoleId";
            roleIdParam.Value = role.Id;
            command.CommandText = UsersQueries.SetRoleToUser;
            command.Parameters.Add(userIdParam);
            command.Parameters.Add(roleIdParam);
            var rows = await command.ExecuteNonQueryAsync();

            return rows == 1;
        }
    }
}
