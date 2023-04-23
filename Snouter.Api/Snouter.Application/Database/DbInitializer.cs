using Dapper;

namespace Blog.Application.Database;

public class DbInitializer
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public DbInitializer(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task InitializeAsync()
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

		await connection.ExecuteAsync("create extension if not exists \"uuid-ossp\"");

        await connection.ExecuteAsync(@"
create table if not exists users(
	id uuid primary key,
	name varchar not null,
	password varchar not null,
	isadmin bool not null
);
");

        await connection.ExecuteAsync(@"
create table if not exists categories(
	id uuid primary key,
	title varchar not null
);
");
        await connection.ExecuteAsync(@"
create table if not exists subcategories(
	id uuid primary key,
	title varchar not null,
	categoryid uuid references categories (id)
);
");
        await connection.ExecuteAsync(@"
create table if not exists specs(
	id uuid primary key,
	title varchar not null,
	categoryid uuid references categories (id)
);
");
        await connection.ExecuteAsync(@"
create table if not exists products(
	id uuid primary key,
	title varchar not null,
	issold bool not null,
	priceincents int not null,
	categoryid uuid references categories (id),
	subcategoryid uuid references subcategories (id),
	sellerid uuid references users (id)
);
");
        await connection.ExecuteAsync(@"
create table if not exists images(
	id uuid primary key,
	src varchar not null,
	productid uuid references products (id)
);
");
        await connection.ExecuteAsync(@"
create table if not exists productsspecs(
	id uuid primary key,
	productid uuid references products (id),
	specid uuid references specs (id),
	specinfo varchar not null
);
");

		await connection.ExecuteAsync(@"
insert into users (id, name, password, isadmin)
values (uuid_generate_v4(), 'a', 'b', false)
");

    }
}