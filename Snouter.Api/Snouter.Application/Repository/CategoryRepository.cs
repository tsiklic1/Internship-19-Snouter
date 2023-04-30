using Blog.Application.Database;
using Dapper;
using Snouter.Application.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snouter.Application.Repository
{
    public class CategoryRepository : ICategoryRepository
    {

        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly IProductRepository _productRepository;
        private readonly ISubcategoryRepository _subcategoryRepository;

        public CategoryRepository(IDbConnectionFactory dbConnectionFactory,
            IProductRepository productRepository, ISubcategoryRepository subcategoryRepository)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _productRepository = productRepository;
            _subcategoryRepository = subcategoryRepository;
        }

        public async Task<bool> CreateAsync(Category category, CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
            using var transaction = connection.BeginTransaction();

            var result = await connection.ExecuteAsync(new CommandDefinition(@"
                insert into categories (id, title)
                values (@Id, @Title)
", category, cancellationToken: token));

            if (result <= 0)
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

            var productIds = await connection.QueryAsync<Guid>(new CommandDefinition(@"
                select id from products where categoryid = @Id
", new { Id = id }, cancellationToken: token));

            foreach (var productId in productIds) {
                await _productRepository.DeleteByIdAsync(productId);
            }

            var subcategoryIds = await connection.QueryAsync<Guid>(new CommandDefinition(@"
                select id from subcategories where categoryid = @Id
", new {Id = id}, cancellationToken: token));

            foreach (var subcategoryId in subcategoryIds)
            {
                await _subcategoryRepository.DeleteByIdAsync(subcategoryId);
            }

            var result = await connection.ExecuteAsync(new CommandDefinition(@"
                delete from categories where id = @Id
", new { Id = id }, cancellationToken: token));


            transaction.Commit();
            return result > 0;
        }

        public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
            return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(@"
                    select count(1) from categories where id = @id

", new { id }, cancellationToken: token));
        }

        public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
            var result = await connection.QueryAsync(new CommandDefinition(@"select id as id, title as title from categories", cancellationToken: token));

            return result.Select(x => new Category { Id = x.id, Title = x.title });
        }


        public async Task<Category?> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);

            var category = await connection.QuerySingleOrDefaultAsync<Category>(
                new CommandDefinition(@"select * from categories where id = @id", new {id}, cancellationToken: token));

            if (category is null) { return null;}

            return category;
        }

        public async Task<bool> UpdateAsync(Category category, CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
            using var transaction = connection.BeginTransaction();

            var result = await connection.ExecuteAsync(new CommandDefinition(@"
            update categories set title = @Title
            where id = @Id
        ", category, cancellationToken: token));

            transaction.Commit();
            return result > 0;
        }
    }
}
