using System;

namespace AbleMods.Api.Models
{
    public class UProductReview
    {
        public int ProductId { get; set; }
        public string Email { get; set; }
        public DateTime ReviewDate { get; set; }
        public int Rating { get; set; }
        public string ReviewTitle { get; set; }
        public string ReviewBody { get; set; }
 
    }
}