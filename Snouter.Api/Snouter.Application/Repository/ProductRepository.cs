
using Blog.Application.Database;
using Dapper;
using Snouter.Application.Models;
using System.Collections.Immutable;

namespace Snouter.Application.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public ProductRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<bool> CreateAsync(Product product)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            using var transaction = connection.BeginTransaction();

            var result = await connection.ExecuteAsync(new CommandDefinition(@"
                            insert into products (id, title, issold, priceincents, categoryid, subcategoryid, sellerid)
                            values (@Id, @Title, @IsSold,@PriceInCents, @CategoryId,@SubcategoryId, @SellerId)
                    ", product));

            if (result <= 0)
            {
                return false;
            }

            foreach (var image in product.Images)
            {
                await connection.ExecuteAsync(new CommandDefinition(@"
                                insert into images (id, src, productid)
                                values (@Id, @Src, @ProductId)
            ", new { Id = Guid.NewGuid(), Src = image, ProductId = product.Id }));
            }

            foreach (var spec in product.Specs)
            {
                await connection.ExecuteAsync(new CommandDefinition(@"
                                insert into productsspecs (id, productid, specid, specinfo)
                                values (@Id, @ProductId, @SpecId, @SpecInfo)
            ", new { Id = Guid.NewGuid(), ProductId = product.Id, SpecId = spec.Key, SpecInfo = spec.Value }));
            }

            transaction.Commit();
            return true;
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            var result = await connection.QueryAsync(new CommandDefinition(@"
            select id, title,
            issold,priceincents,
            categoryid, subcategoryid,
            sellerid from products 
            "));

            var productsList = result.Select(x => new Product
            {
                Id = x.id,
                Title = x.title,
                IsSold = x.issold,
                PriceInCents = x.priceincents,
                CategoryId = x.categoryid,
                SubcategoryId = x.subcategoryid,
                SellerId = x.sellerid,
                Images = new List<string> { },
                Specs = new Dictionary<Guid, string> { }
            }); 

            foreach (var product in productsList)
            {
                var images = await connection.QueryAsync(new CommandDefinition(@"
                select images.src as src from images
                where images.productid = @Id
", new { Id = product.Id }));

                foreach (var image in images)
                {
                    product.Images.Add(image.src);
                }
            }

            foreach (var product in productsList)
            {
                var specs = await connection.QueryAsync(new CommandDefinition(@"
                select specid,specinfo
                from specs join productsspecs on specs.id = productsspecs.specid
                where productsspecs.productid = @Id
", new { Id = product.Id }));

                var specsList = specs.Select(x => new { SpecId = x.specid, SpecInfo = x.specinfo });

                foreach (var spec in specsList)
                {
                     product.Specs.Add(spec.SpecId, spec.SpecInfo);
                }
            }

            return productsList;
        }

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            var product = await connection.QuerySingleOrDefaultAsync<Product>(new CommandDefinition(@"
                select id, title, issold, priceincents, categoryid, subcategoryid, sellerid
                from products where id = @Id
", new { Id = id }));

            if (product is null) { return null; }
            return product;
        }




        //        //Task<bool> IProductRepository.DeleteAsync(Guid id)
        //        //{
        //        //    return Task.FromResult(false);
        //        //    //var tempProduct = _products.SingleOrDefault(x => x.Id == id);
        //        //    //if (tempProduct is null)
        //        //    //{
        //        //    //    return Task.FromResult(false);
        //        //    //}

        //        //    //_products.Remove(tempProduct);
        //        //    //return Task.FromResult(true);
        //        //}

        //        async Task<IEnumerable<Product>> IProductRepository.GetAllAsync()
        //        {
        //            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        //            var result = await connection.QueryAsync(new CommandDefinition(@"
        //               select products.id as id, products.title as title,
        //                products.issold as issold, product.priceincents as priceincents,
        //                products.categoryid as categoryid, products.subcategoryid as subcategoryid,

        //from products 
        //join productsspecs on products.id = productsspecs.productid
        //join specs on specs.id = productsspecs.specid; 
        //"));

        //        }

        //        async Task<Product?> IProductRepository.GetByIdAsync(Guid id)
        //        {
        //            //var tempProduct = _products.FirstOrDefault(x => x.Id == id);
        //            //return Task.FromResult(tempProduct);
        //            return true;
        //        }

        //Task<bool> IProductRepository.UpdateAsync(Product product)
        //{
        //    var tempProduct = _products.SingleOrDefault(x => x.Id == product.Id);


        //    if (tempProduct is null)
        //    {
        //        return Task.FromResult(false);
        //    }

        //    tempProduct.Title= product.Title;
        //    tempProduct.IsSold= product.IsSold;
        //    tempProduct.PriceInCents= product.PriceInCents;
        //    tempProduct.Category = product.Category;
        //    tempProduct.SubCategory = product.SubCategory;
        //    tempProduct.Images= product.Images;
        //    tempProduct.Properties= product.Properties;

        //    return Task.FromResult(true);

        //}
    }
}
