﻿using Blog.Application.Database;
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
        private readonly IProductRepository _productRepository;

        public SubcategoryRepository(IDbConnectionFactory dbConnectionFactory, IProductRepository productRepository)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _productRepository = productRepository;
        }
        public async Task<bool> CreateAsync(Subcategory subcategory)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            using var transaction = connection.BeginTransaction();
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

        public async Task<bool> DeleteByIdAsync(Guid id)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            using var transaction = connection.BeginTransaction();

            var productIds = await connection.QueryAsync<Guid>(new CommandDefinition(@"
                select id from products where subcategoryid = @Id
", new { Id = id }));
            
            foreach (var productId in productIds) {
                await _productRepository.DeleteByIdAsync(productId);
            }

            var result = await connection.ExecuteAsync(new CommandDefinition(@"
                delete from subcategories where id = @id
", new { id }));


            transaction.Commit();
            return result > 0;
        }

        public async Task<bool> ExistsByIdAsync(Guid id)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(@"
            select count(1) from subcategories where id = @Id
        ", new { Id = id }));
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
                CategoryId = x.categoryid,
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
                select subcategories.id as id, subcategories.title as title,
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

        public async Task<bool> UpdateAsync(Subcategory subcategory)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            using var transaction = connection.BeginTransaction();

            var result = await connection.ExecuteAsync(new CommandDefinition(@"
            update subcategories set title = @Title, categoryid = @CategoryId
            where id = @Id
        ", subcategory));

            transaction.Commit();
            return result > 0;
        }
    }
}
