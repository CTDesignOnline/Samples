using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace AbleMods.Api.Models
{
    public class UOption
    {
        public int OptionId { get; set; }
        public string Name { get; set; }
        public string HeaderText { get; set; }
        public IList<UOptionChoice> Choices { get; set; }
    }
}