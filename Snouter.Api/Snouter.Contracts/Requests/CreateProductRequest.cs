namespace Snouter.Contracts.Requests
{
    public class CreateProductRequest
    {
        public string Title { get; set; }
        public bool IsSold { get; set; } = false;
        public int PriceInCents { get; set; }
        public Guid CategoryId { get; set; }
        public Guid SubcategoryId { get; set; }

        public Guid SellerId { get; set; }
        public string Location { get; set; }
        public List<string> Images { get; set; } = new List<string>();

        public Dictionary<Guid, string> Specs { get; set; } = new Dictionary<Guid, string>();

    }
}
