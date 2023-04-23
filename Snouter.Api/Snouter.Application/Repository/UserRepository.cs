﻿using Blog.Application.Database;
using Dapper;
using Snouter.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snouter.Application.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public UserRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<bool> CreateAsync(User user)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            using var transaction = connection.BeginTransaction();

            var result = await connection.ExecuteAsync(new CommandDefinition(@"
            insert into users (id, name, password, isadmin)
            values (@Id, @Name, @Password, @IsAdmin)
        ", user));

            if (result < 0)
            {
                return false;
            }
            transaction.Commit();
            return true;
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            var result = await connection.QueryAsync(new CommandDefinition(@"
            select users.id as id, users.name as name, 
            users.password as password, users.isadmin as isadmin
            from users
        "));

            return result.Select(x => new User
            {
                Id = x.id,
                Name = x.name,
                Password= x.password,
                IsAdmin= x.isadmin
            });

        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();

            var user = await connection.QuerySingleOrDefaultAsync<User>(
            new CommandDefinition(@"
                select * from users where id = @id
            ", new { id }));

            if (user is null)
            {
                return null;
            }
            return user;
        }
    }
}