using Blog.Application.Database;
using Dapper;
using Snouter.Application.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snouter.Application.Repository
{
    public class SubcategoryRepository : ISubcategoryRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        //private readonly ICategoryRepository _categoryRepository;

        public SubcategoryRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
            //_categoryRepository = categoryRepository;
        }
        public async Task<bool> CreateAsync(Subcategory subcategory)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            using var transaction = connection.BeginTransaction();
            //var categoryExists = await _categoryRepository.ExistsByIdAsync(subcategory.Id);

            //if (!categoryExists)
            //{
            //    return false;
            //}


            var result = await connection.ExecuteAsync(new CommandDefinition(@"
                    insert into subcategories (id, title, categoryid)
                    values (@Id, @Title, @CategoryId)
", subcategory));

            if (result <= 0)
            {
                return false;
            }

            transaction.Commit();
            return result > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            using var transaction = connection.BeginTransaction();

            var result = await connection.ExecuteAsync(new CommandDefinition(@"
                delete from subcategories where id = @id
", new { id }));


            transaction.Commit();
            return result > 0;
        }

        public Task<bool> ExistsByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Subcategory>> GetAllAsync()
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            var result = await connection.QueryAsync(new CommandDefinition(@"
                select subcategories.id as id, subcategories.title as subtitle,
                subcategories.categoryid as categoryid, categories.title as categorytitle
                from subcategories inner join categories
                on subcategories.categoryid = categories.id
"));
            return result.Select(x => new Subcategory
            {
                Id = x.id,
                Title = x.subtitle,
                Category = new Category
                {
                    Id = x.categoryid,
                    Title = x.categorytitle
                }
            }
                );

        }

        public async Task<Subcategory?> GetByIdAsync(Guid id)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();

            var subcategory = await connection.QuerySingleOrDefaultAsync <Subcategory>(
                new CommandDefinition(@"
                select subcategories.id as id, subcategories.title as subtitle,
                subcategories.categoryid as categoryid from subcategories
                where subcategories.id = @id
", new { id }));

            if (subcategory is null) { return null; }

            var category = await connection.QuerySingleOrDefaultAsync<Category>(@"
                select * from categories where id = @id
", new {id});

            subcategory.Category = category;

            return subcategory;

        }
    }
}
