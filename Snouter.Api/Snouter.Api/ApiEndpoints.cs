using System.Net.NetworkInformation;

namespace Snouter.Api
{
    public static class ApiEndpoints
    {
        private const string ApiBase = "api";
        public static class Product
        {
            private const string Base = $"{ApiBase}/products";
            public const string Create = Base;
            public const string Get = $"{Base}/{{id:guid}}";
            public const string GetAll = Base;
            public const string Update = $"{Base}/{{id:guid}}";
        }

        public static class Category {
            private const string Base = $"{ApiBase}/categores";
            public const string Create = Base;
            public const string Get = $"{Base}/{{id:guid}}";
            public const string GetAll = Base;
            public const string Update = $"{Base}/{{id:guid}}";
        }

        public static class Subcategory
        {
            private const string Base = $"{ApiBase}/subcategories";
            public const string Create = Base;
            public const string Get = $"{Base}/{{id:guid}}";
            public const string GetAll = Base;
            public const string Update = $"{Base}/{{id:guid}}";
        }

        public static class User
        {
            private const string Base = $"{ApiBase}/users";
            public const string Create = Base;
            public const string Get = $"{Base}/{{id:guid}}";
            public const string GetAll = Base;
            public const string Update = $"{Base}/{{id:guid}}";
        }

        public static class Spec
        {
            private const string Base = $"{ApiBase}/specs";
            public const string Create = Base;
            public const string Get = $"{Base}/{{id:guid}}";
            public const string GetAll = Base;
            public const string Update = $"{Base}/{{id:guid}}";
        }
    }
}
