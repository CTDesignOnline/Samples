using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Web.Models;

namespace AbleMods.Models
{

    public class ImportPageModel 
    {
        // Display Attribute will appear in the Html.LabelFor
        public int CategoryId { get; set; }

        [Display(Name = "Select the Able category to import:")]
        public IEnumerable<SelectListItem> Categories { get; set; }


        public int ParentNodeId { get; set; }

        [Display(Name = "Select the root page of Umbraco site:")]
        public IEnumerable<SelectListItem> ContentNodes { get; set; }

        public bool DeepImport { get; set; }

        public bool Test3Only { get; set; }

    }

}
