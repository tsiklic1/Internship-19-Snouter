//using Dapper;

//namespace Blog.Application.Database;

//public class DbInitializer
//{
//    private readonly IDbConnectionFactory _dbConnectionFactory;

//    public DbInitializer(IDbConnectionFactory dbConnectionFactory)
//    {
//        _dbConnectionFactory = dbConnectionFactory;
//    }

//    public async Task InitializeAsync()
//    {
//        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

//        //await connection.ExecuteAsync(@"
//        //    create table if not exists posts (
//        //    id UUID primary key,
//        //    slug TEXT not null, 
//        //    title TEXT not null,
//        //    createdat date not null);
//        //");

//        //await connection.ExecuteAsync(@"
//        //    create unique index concurrently if not exists posts_slug_idx
//        //    on posts
//        //    using btree(slug);
//        //");

//        //await connection.ExecuteAsync(@"
//        //    create table if not exists categories (
//        //    postId UUID references posts (Id),
//        //    name TEXT not null);
//        //");

//        await connection.ExecuteAsync(@"
//create table if not exists categories(
//	id SERIAL primary key,
//	category VARCHAR not null
//); 
//");

//        await connection.ExecuteAsync(@"
//create table if not exists specs(
//	id SERIAL primary key,
//	spec VARCHAR not null
//);
//");

        

//        await connection.ExecuteAsync(@"
//create table if not exists products (
//    id UUID primary key,
//    title TEXT not null,
//    issold BOOL not null,
//    priceincents INT not null,
//    category INT references categories,
//    subcategory TEXT,
//    images TEXT,
//    location TEXT,
            
//);
//");

//        await connection.ExecuteAsync(@"
//create table if not exists categoriesspecs(
//	id serial primary key,
//	categoryid int references categories,
//	specid int references specs
//);

//");

//        await connection.ExecuteAsync(@"
//create table if not exists productsspecs(
//	id serial primary key,
//	productid UUID references products,
//	specId int references categoriesspecs,
//	specinfo TEXT not null
//);
//");
//    }
//}