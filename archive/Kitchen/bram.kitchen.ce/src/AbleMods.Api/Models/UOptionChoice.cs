namespace AbleMods.Api.Models
{
    public class UOptionChoice
    {
        public int OptionChoiceId { get; set; }
        public int OptionId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public decimal PriceModifier { get; set; }
        public decimal CostModifier { get; set; }
        public decimal WeightModifier { get; set; }
        public string SkuModifier { get; set; }
    }
}