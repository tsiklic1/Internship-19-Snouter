
namespace Snouter.Application.Models
{
    public class Product
    {
        public Guid Id { get; init; }

        public string Title { get; set; }
        public bool IsSold { get; set; } = false;
        public int PriceInCents { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public List<string> Images { get; set; } = new List<string>();
        public string Location { get; set; }
        public Dictionary<string, string> Properties { get; set;} = new Dictionary<string, string>();
    }
}
