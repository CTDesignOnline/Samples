namespace AbleMods.Dto
{
    using System;
    using Umbraco.Core.Persistence;
    using Umbraco.Core.Persistence.DatabaseAnnotations;

    /// <summary>
    /// The legacy ac product review dto.
    /// </summary>
    /// <remarks>
    /// The name of the database column must match the Column attribute - including casing
    /// </remarks>
    [TableName("legacyacProductReviews")]
    [PrimaryKey("Id", autoIncrement = true)]
    [ExplicitColumns]
    public class AcProductReviewDto
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <remarks>
        /// Not needed but lets include it anyway
        /// </remarks>
        [Column("id")]
        [PrimaryKeyColumn]
        public int ProductReviewId { get; set; }

        /// <summary>
        /// Gets or sets the AbleCommerce product id.
        /// </summary>
        [Column("ProductId")]
        public int ProductId { get; set; }

        /// <summary>
        /// Gets or sets the AbleCommerce user id.
        /// </summary>
        [Column("Email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the review date
        /// </summary>
        [Column("ReviewDate")]
        public DateTime ReviewDate { get; set; }

        /// <summary>
        /// Gets or sets the rating
        /// </summary>
        [Column("Rating")]
        public int Rating{ get; set; }

        /// <summary>
        /// Gets or sets the review title
        /// </summary>
        [Column("ReviewTitle")]
        public string ReviewTitle { get; set; }

        /// <summary>
        /// Gets or sets the review body
        /// </summary>
        [Column("ReviewBody")]
        [SpecialDbType(SpecialDbTypes.NTEXT)]
        public string ReviewBody { get; set; }

    }
}