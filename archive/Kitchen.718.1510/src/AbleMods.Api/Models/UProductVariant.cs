namespace AbleMods.Api.Models
{
    public class UProductVariant
    {
        public int ProductId { get; set; } 
        public int ProductVariantId { get; set; }
        public decimal Price { get; set; }
        public int Option1 { get; set; }
        public int Option2 { get; set; }
        public int Option3 { get; set; }
        public string AcUrl { get; set; }
        public string AcSku { get; set; }
        public decimal Weight { get; set; }
        public bool Available { get; set; }
    }
}