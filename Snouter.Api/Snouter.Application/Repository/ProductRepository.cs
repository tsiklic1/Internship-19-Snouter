
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
            SELECT
              p.id AS product_id,
              p.title AS product_title,
              p.issold AS product_issold,
              p.priceincents AS product_priceincents,
              c.id AS category_id,
              s.id AS subcategory_id,
              u.id AS seller_id,
              (
                SELECT string_agg(i.src, ',') 
                FROM images i 
                WHERE i.productid = p.id
              ) AS image_urls,
              (
                SELECT string_agg(ps.specid || ':' || ps.specinfo, ',') 
                FROM productsspecs ps 
                WHERE ps.productid = p.id
              ) AS specs_info
            FROM products p
            JOIN categories c ON p.categoryid = c.id
            JOIN subcategories s ON p.subcategoryid = s.id
            JOIN users u ON p.sellerid = u.id;
            "));

            return result.Select(row => new Product
            {
                Id = row.product_id,
                Title = row.product_title,
                IsSold = row.product_issold,
                PriceInCents = row.product_priceincents,
                CategoryId = row.category_id,
                SubcategoryId = row.subcategory_id,
                SellerId = row.seller_id,
                Images = _ConvertImages(row.image_urls),
                Specs = _ConvertSpecs(row.specs_info)
            }) ;
        }
        List<String> _ConvertImages(string imagesString)
        {
            var result = imagesString != null ? imagesString.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList() : new List<string>();
            return result;
        }
        Dictionary<Guid, string> _ConvertSpecs(string specsString)
        {
            var specs = specsString != null ? specsString.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(specInfo => specInfo.Split(':'))
            .ToDictionary(specParts => Guid.Parse(specParts[0]), specParts => specParts[1]) : new Dictionary<Guid, string>();
            
            return specs;
        }

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            var product = await connection.QuerySingleOrDefaultAsync<Product>(new CommandDefinition(@"
                select id, title, issold, priceincents, categoryid, subcategoryid, sellerid
                from products where id = @Id
", new { Id = id }));

            if (product is null) { return null; }

            var images = await connection.QueryAsync<string>(new CommandDefinition(@"
                select src from images where productid = @Id
", new { Id = id }));

            product.Images = images.ToList();

            var specs = await connection.QueryAsync(new CommandDefinition(@"
                select specid,specinfo
                from specs join productsspecs on specs.id = productsspecs.specid
                where productsspecs.productid = @Id
", new {Id = product.Id}));

            foreach (var spec in specs)
            {
                product.Specs.Add(spec.specid, spec.specinfo);
            }

            return product;
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            throw new NotImplementedException();
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
