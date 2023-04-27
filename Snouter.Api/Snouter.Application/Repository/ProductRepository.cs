
using Blog.Application.Database;
using Dapper;
using Snouter.Application.Models;
using System.Collections.Immutable;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;

namespace Snouter.Application.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly ISpecRepository _specRepository;

        public ProductRepository(IDbConnectionFactory dbConnectionFactory, ISpecRepository specRepository)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _specRepository = specRepository;
        }

        public async Task<bool> CreateAsync(Product product, CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
            using var transaction = connection.BeginTransaction();

            var result = await connection.ExecuteAsync(new CommandDefinition(@"
                            insert into products (id, title, issold, priceincents, categoryid, subcategoryid, sellerid, location)
                            values (@Id, @Title, @IsSold,@PriceInCents, @CategoryId,@SubcategoryId, @SellerId, @Location)
                    ", product, cancellationToken: token));

            if (result <= 0)
            {
                return false;
            }

            foreach (var image in product.Images)
            {
                await connection.ExecuteAsync(new CommandDefinition(@"
                                insert into images (id, src, productid)
                                values (@Id, @Src, @ProductId)
            ", new { Id = Guid.NewGuid(), Src = image, ProductId = product.Id }, cancellationToken: token));
            }

            foreach (var spec in product.Specs)
            {
                await connection.ExecuteAsync(new CommandDefinition(@"
                                insert into productsspecs (id, productid, specid, specinfo)
                                values (@Id, @ProductId, @SpecId, @SpecInfo)
            ", new { Id = Guid.NewGuid(), ProductId = product.Id, SpecId = spec.Key, SpecInfo = spec.Value }, cancellationToken: token));
            }

            transaction.Commit();
            return true;
        }

        public async Task<IEnumerable<Product>> GetAllAsync( CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
            var result = await connection.QueryAsync(new CommandDefinition(@"
            SELECT
              p.id AS product_id,
              p.title AS product_title,
              p.issold AS product_issold,
              p.priceincents AS product_priceincents,
              p.location as location,
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
            ", cancellationToken: token));

            return result.Select(row => new Product
            {
                Id = row.product_id,
                Title = row.product_title,
                IsSold = row.product_issold,
                PriceInCents = row.product_priceincents,
                CategoryId = row.category_id,
                SubcategoryId = row.subcategory_id,
                SellerId = row.seller_id,
                Location= row.location,
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

        public async Task<Product?> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
            var product = await connection.QuerySingleOrDefaultAsync<Product>(new CommandDefinition(@"
                select id, title, issold, priceincents, categoryid, subcategoryid, sellerid, location
                from products where id = @Id
", new { Id = id }, cancellationToken: token));

            if (product is null) { return null; }

            var images = await connection.QueryAsync<string>(new CommandDefinition(@"
                select src from images where productid = @Id
", new { Id = id }, cancellationToken: token));

            product.Images = images.ToList();

            var specs = await connection.QueryAsync(new CommandDefinition(@"
                select specid,specinfo
                from specs join productsspecs on specs.id = productsspecs.specid
                where productsspecs.productid = @Id
", new {Id = product.Id}, cancellationToken: token));

            foreach (var spec in specs)
            {
                product.Specs.Add(spec.specid, spec.specinfo);
            }

            return product;
        }

        public async Task<bool> UpdateAsync(Product product, CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
            using var transaction = connection.BeginTransaction();

            await connection.ExecuteAsync(new CommandDefinition(@"
                delete from images where productid = @Id
", product, cancellationToken: token));

            foreach (var image in product.Images)
            {
                await connection.ExecuteAsync(new CommandDefinition(@"
                                insert into images (id, src, productid)
                                values (@Id, @Src, @ProductId)
            ", new { Id = Guid.NewGuid(), Src = image, ProductId = product.Id }));
            }

            var oldSpecIds = await connection.QueryAsync<Guid>(new CommandDefinition(@"
                select specid from productsspecs where productid = @Id
", product, cancellationToken: token));

            var newSpecs = new Dictionary<Guid, string>();
            var oldSpecs = new Dictionary<Guid, string>();

            foreach (var spec in product.Specs)
            {
                if (!oldSpecIds.Contains(spec.Key))
                {
                    newSpecs.Add(spec.Key, spec.Value);
                }
                else
                {
                    oldSpecs.Add(spec.Key, spec.Value);
                }
            }

            foreach (var spec in oldSpecs)
            {
                await connection.ExecuteAsync(new CommandDefinition(@"
                    update productsspecs set 
                    specinfo = @SpecInfo
                    where specid = @SpecId and productid = @ProductId
", new { ProductId = product.Id, SpecId = spec.Key, SpecInfo = spec.Value }, cancellationToken: token));

            }

            foreach (var spec in newSpecs)
            {
                await connection.ExecuteAsync(new CommandDefinition(@"
                    insert into productsspecs (id, productid, specid, specinfo)
                    values (@Id, @ProductId, @SpecId, @SpecInfo)
", new { Id = Guid.NewGuid(), ProductId = product.Id, SpecId = spec.Key, SpecInfo = spec.Value}, cancellationToken: token));
            }

            var result = await connection.ExecuteAsync(new CommandDefinition(@"
                update products set 
                title = @Title,
                issold = @IsSold,
                priceincents = @PriceInCents,
                location = @Location,
                categoryid = @CategoryId,
                subcategoryId = @SubcategoryId,
                sellerid = @SellerId
                where id = @Id
", product, cancellationToken: token));

            transaction.Commit();
            return result > 0;
        }

        public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
            using var transaction = connection.BeginTransaction();

            await connection.ExecuteAsync(new CommandDefinition(@"
            delete from images where productid = @Id
        ", new { Id = id }, cancellationToken: token));

            await connection.ExecuteAsync(new CommandDefinition(@"
            delete from productsspecs where productid = @Id
        ", new { Id = id }, cancellationToken: token));

            var result = await connection.ExecuteAsync(new CommandDefinition(@"
            delete from products where id = @Id
", new {Id = id}, cancellationToken: token));

            transaction.Commit();
            return result > 0;
        }

        public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
            return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(@"
                    select count(1) from products where id = @id
", new { id }, cancellationToken: token));
        }

        public async Task<bool> SpecsMachCategory(Product product, CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);

            foreach (var specId in product.Specs.Keys.ToList()) {
                var categoryIdFromSpec = await connection.QuerySingleOrDefaultAsync<Guid>(new CommandDefinition(@"
                    select categoryid from specs
                    where id = @Id
", new { Id = specId }, cancellationToken: token));

                if(categoryIdFromSpec != product.CategoryId)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
