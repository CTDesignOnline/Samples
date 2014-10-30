namespace AbleMods.Dto
{
    using System;
    using Umbraco.Core.Persistence;
    using Umbraco.Core.Persistence.DatabaseAnnotations;

    /// <summary>
    /// The legacy ac product dto.
    /// </summary>
    /// <remarks>
    /// The name of the database column must match the Column attribute - including casing
    /// </remarks>
    [TableName("legacyAcProduct")]
    [PrimaryKey("id")]
    [ExplicitColumns]
    public class AcProductDto
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <remarks>
        /// Not needed but lets include it anyway
        /// </remarks>
        [Column("id")]
        [PrimaryKeyColumn]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the AbleCommerce product id.
        /// </summary>
        [Column("acProductId")]
        public int AcProductId { get; set; }

        /// <summary>
        /// Gets or sets the AbleCommerce product variant id.
        /// </summary>
        [Column("acProductVariantId")]
        [NullSetting(NullSetting = NullSettings.Null)]
        public int? AcProductVariantId { get; set; }

        /// <summary>
        /// Gets or sets the AbleCommerce option 1.
        /// </summary>
        /// <remarks>
        /// Joe - I'm not sure this is the best field choice as I think I remember that 
        /// most of the lookups come off a CSV list of option ids ... whatever you think is best
        /// </remarks>
        [Column("acOptionList")]
        [NullSetting(NullSetting = NullSettings.Null)]
        public string AcOptionList { get; set; }

        /// <summary>
        /// Gets or sets the Merchello product key.
        /// </summary>
        [Column("productKey")]
        public Guid ProductKey { get; set; }

        /// <summary>
        /// Gets or sets the Merchello product variant key.
        /// </summary>
        [Column("productVariantKey")]
        public Guid ProductVariantKey { get; set; }

        /// <summary>
        /// Gets or sets the ac url.
        /// </summary>
        [Column("acUrl")]
        [NullSetting(NullSetting = NullSettings.Null)]
        public string AcUrl { get; set; }

        /// <summary>
        /// Gets or sets the ac url.
        /// </summary>
        [Column("acSku")]
        [NullSetting(NullSetting = NullSettings.Null)]
        public string AcSku { get; set; }
    }
}