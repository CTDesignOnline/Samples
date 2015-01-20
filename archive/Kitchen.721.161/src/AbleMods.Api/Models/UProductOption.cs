using System.Collections.Generic;

namespace AbleMods.Api.Models
{
    public class UProductOption
    {
        public int ProductId { get; set; }
        public int OptionId { get; set; }
        public string OptionName { get; set; }
        public List<UOptionChoice> Choices { get; set; } 
    }
}