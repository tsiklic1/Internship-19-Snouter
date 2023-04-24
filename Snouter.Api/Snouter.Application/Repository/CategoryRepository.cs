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
        //private List<Category> _categories = new List<Category>();

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

        public async Task<bool> CreateAsync(Category category)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            using var transaction = connection.BeginTransaction();

            var result = await connection.ExecuteAsync(new CommandDefinition(@"
                insert into categories (id, title)
                values (@Id, @Title)
", category));

            if (result <= 0)
            {
                return false;
            }

            transaction.Commit();
            return result > 0;

            //if (_categories.Contains(category))
            //{
            //    return Task.FromResult(false);
            //}
            //_categories.Add(category);
            //return Task.FromResult(true);
        }

        public async Task<bool> DeleteByIdAsync(Guid id)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            using var transaction = connection.BeginTransaction();

            var productIds = await connection.QueryAsync<Guid>(new CommandDefinition(@"
                select id from products where categoryid = @Id
", new { Id = id }));

            foreach (var productId in productIds) {
                await _productRepository.DeleteByIdAsync(productId);
            }

            var subcategoryIds = await connection.QueryAsync<Guid>(new CommandDefinition(@"
                select id from subcategories where categoryid = @Id
", new {Id = id}));

            foreach (var subcategoryId in subcategoryIds)
            {
                await _subcategoryRepository.DeleteByIdAsync(subcategoryId);
            }

            var result = await connection.ExecuteAsync(new CommandDefinition(@"
                delete from categories where id = @Id
", new { Id = id }));


            transaction.Commit();
            return result > 0;
        }

        public async Task<bool> ExistsByIdAsync(Guid id)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(@"
                    select count(1) from categories where id = @id

", new { id }));
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            var result = await connection.QueryAsync(new CommandDefinition(@"select id as id, title as title from categories"));

            return result.Select(x => new Category { Id = x.id, Title = x.title });
        }


        public async Task<Category?> GetByIdAsync(Guid id)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();

            var category = await connection.QuerySingleOrDefaultAsync<Category>(
                new CommandDefinition(@"select * from categories where id = @id", new {id}));

            if (category is null) { return null;}

            return category;
        }

        public async Task<bool> UpdateAsync(Category category)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            using var transaction = connection.BeginTransaction();

            var result = await connection.ExecuteAsync(new CommandDefinition(@"
            update categories set title = @Title
            where id = @Id
        ", category));

            transaction.Commit();
            return result > 0;
        }
    }
}
