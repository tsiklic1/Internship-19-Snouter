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

        public CategoryRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
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

        public async Task<bool> DeleteAsync(Guid id)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            using var transaction = connection.BeginTransaction();

            var result = await connection.ExecuteAsync(new CommandDefinition(@"
                delete from categories where id = @id
", new { id }));


            transaction.Commit();
            return result > 0;

            //var tempCategory = _categories.SingleOrDefault(c => c.Id == id);
            //if (tempCategory is null) {
            //    return Task.FromResult(false);
            //}
            //_categories.Remove(tempCategory);
            //return Task.FromResult(true);
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
            //return Task.FromResult<IEnumerable<Category>>(_categories);
        }


        public async Task<Category?> GetByIdAsync(Guid id)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();

            var category = await connection.QuerySingleOrDefaultAsync<Category>(
                new CommandDefinition(@"select * from categories where id = @id", new {id}));

            if (category is null) { return null;}

            return category;
            //var tempCategory = _categories.FirstOrDefault(c => c.Id == id);
            //return Task.FromResult(tempCategory);
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

        //public Task<bool> UpdateAsync(Category category)
        //{
        //    //var tempCategory = _categories.SingleOrDefault(x => x.Id == category.Id);


        //    //if (tempCategory is null)
        //    //{
        //    //    return Task.FromResult(false);
        //    //}

        //    //tempCategory.Title = category.Title;

        //    //return Task.FromResult(true);
        //    return Task.FromResult(true);
        //}
    }
}
