using Blog.Application.Database;
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

        public async Task<bool> DeleteByIdAsync(Guid id)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            using var transaction = connection.BeginTransaction();

            var productIds = await connection.QueryAsync<Guid>(new CommandDefinition(@"
                select id from products where sellerid = @Id;
", new { Id = id }));

            foreach (var productId in productIds) {
                await connection.ExecuteAsync(new CommandDefinition(@"
                delete from productsspecs where productid = @Id
", new { Id = productId}));
            }

            foreach (var productId in productIds) {
                await connection.ExecuteAsync(new CommandDefinition(@"
                delete from images where productid = @Id
", new { Id = productId }));
            }

            await connection.QueryAsync(new CommandDefinition(@"
                delete from products where sellerid = @Id
", new {Id = id }));

            var result = await connection.ExecuteAsync(new CommandDefinition(@"
                delete from users where id = @Id
", new { Id = id }));

            transaction.Commit();
            return result > 0;
        }

        public async Task<bool> ExistsByIdAsync(Guid id)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(@"
                    select count(1) from users where id = @id
", new { id }));
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

        public async Task<bool> UpdateAsync(User user)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            using var transaction = connection.BeginTransaction();

            var result = await connection.ExecuteAsync(new CommandDefinition(@"
            update users set name = @Name, password = @Password, isadmin = @IsAdmin
            where id = @Id
        ", user));

            transaction.Commit();
            return result > 0;
        }
    }
}
