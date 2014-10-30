namespace BrambleBerry.Kitchen.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Merchello.Web.Models.ContentEditing;

    /// <summary>
    /// The add item model.
    /// </summary>
    public class AddItemFormModel
    {
        /// <summary>
        /// Gets or sets the content id.
        /// </summary>
        [Required]
        public int ContentId { get; set; }

        /// <summary>
        /// Gets or sets the product key.
        /// </summary>
        public Guid ProductKey { get; set; }

        /// <summary>
        /// Gets or sets the Merchello product.
        /// </summary>
        /// <remarks>
        /// Used to populate the add item form
        /// </remarks>
        public ProductDisplay Product { get; set; }

        /// <summary>
        /// Gets or sets the option choices.
        /// </summary>
        /// <remarks>
        /// Used to determine the product variant (if applicable) to add to the basket
        /// </remarks>
        public Guid[] OptionChoices { get; set; }

        /// <summary>
        /// Gets or sets the quantity to add to the basket
        /// </summary>
        [Required]
        public int Quantity { get; set; }
    }
}