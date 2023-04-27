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
        public async Task<bool> CreateAsync(Spec spec, CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
            using var transaction = connection.BeginTransaction();

            var result = await connection.ExecuteAsync(new CommandDefinition(@"
                insert into specs (id, title, categoryid)
                values (@Id, @Title, @CategoryId);
", new { Id = spec.Id, Title = spec.Title, CategoryId = spec.CategoryId }, cancellationToken: token));

            if (result < 0)
            {
                return false;
            }

            transaction.Commit();

            return result > 0;
        }

        public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
            using var transaction = connection.BeginTransaction();

            await connection.ExecuteAsync(new CommandDefinition(@"
                delete from productsspecs where specid = @Id
", new {Id = id}, cancellationToken: token));

            var result = await connection.ExecuteAsync(new CommandDefinition(@"
                delete from specs where id = @Id
", new { Id = id}, cancellationToken: token));

            transaction.Commit();
            return result > 0;
        }

        public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
            return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(@"
            select count(1) from specs where id = @Id
        ", new { Id = id }, cancellationToken: token));
        }

        public async Task<IEnumerable<Spec>> GetAllAsync(CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);

            var result = await connection.QueryAsync(new CommandDefinition(@"
                select id, title, categoryid from specs
", cancellationToken: token));

            return result.Select(x => new Spec
            {
                Id = x.id,
                Title = x.title,
                CategoryId = x.categoryid,
            });

        }

        public async Task<Spec?> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
            var spec = await connection.QuerySingleOrDefaultAsync<Spec>(new CommandDefinition(@"
                select id, title, categoryid from specs where id = @Id
", new { Id = id}, cancellationToken: token));

            return spec;

        }

        public async Task<bool> UpdateAsync(Spec spec, CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
            using var transaction = connection.BeginTransaction();

            var result = await connection.ExecuteAsync(new CommandDefinition(@"
            update specs set title = @Title, categoryid = @CategoryId
            where id = @Id
        ", spec, cancellationToken: token));

            transaction.Commit();
            return result > 0;
        }
    }
}
