using System.Collections.Generic;

namespace AbleMods.Api.Models
{
    public class UProduct
    {

        public int ProductId { get; set; }
        public string Name { get; set; }
        public string NavigateUrl { get; set; }
        public string Sku { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public decimal COGS { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public bool HasOptions { get; set; }
        public List<string> Images { get; set; }
        public List<UOption> Options { get; set; } 
        public decimal Weight { get; set; }
        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }


        public UProduct()
        {
        }
    }
}
