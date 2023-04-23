using Blog.Application.Database;
using Dapper;
using Snouter.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace Snouter.Application.Repository
{
    public class SpecRepository : ISpecRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        //private readonly ICategoryRepository _categoryRepository;

        public SpecRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
            //_categoryRepository = categoryRepository;
        }
        public async Task<bool> CreateAsync(Spec spec)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            using var transaction = connection.BeginTransaction();

            var result = await connection.ExecuteAsync(new CommandDefinition(@"
                insert into specs (id, title, categoryid)
                values (@Id, @Title, @CategoryId);
", new { Id = spec.Id, Title = spec.Title, CategoryId = spec.CategoryId }));

            if (result < 0)
            {
                return false;
            }

            transaction.Commit();

            return result > 0;
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExistsByIdAsync(Guid id)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(@"
            select count(1) from specs where id = @Id
        ", new { Id = id }));
        }

        public async Task<IEnumerable<Spec>> GetAllAsync()
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();

            var result = await connection.QueryAsync(new CommandDefinition(@"
                select id, title, categoryid from specs
"));

            return result.Select(x => new Spec
            {
                Id = x.id,
                Title = x.title,
                CategoryId = x.categoryid,
            });

        }

        public async Task<Spec?> GetByIdAsync(Guid id)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            var spec = await connection.QuerySingleOrDefaultAsync<Spec>(new CommandDefinition(@"
                select id, title, categoryid from specs where id = @Id
", new { Id = id}));

            return spec;

        }

        public async Task<bool> UpdateAsync(Spec spec)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            using var transaction = connection.BeginTransaction();

            var result = await connection.ExecuteAsync(new CommandDefinition(@"
            update specs set title = @Title, categoryid = @CategoryId
            where id = @Id
        ", spec));

            transaction.Commit();
            return result > 0;
        }
    }
}
