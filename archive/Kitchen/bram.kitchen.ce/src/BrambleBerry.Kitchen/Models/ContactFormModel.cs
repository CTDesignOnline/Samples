namespace BrambleBerry.Kitchen.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;

    /// <summary>
    /// The contact form model.
    /// </summary>
    public class ContactFormModel
    {
        /// <summary>
        /// The display mode.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:EnumerationItemsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public enum DisplayMode
        {
            RenderForm,
            ConfirmationMessage,
            Error
        }

        /// <summary>
        /// Gets or sets the content id.
        /// </summary>
        [Required]
        public int ContentId { get; set; }

        /// <summary>
        /// Gets or sets the to name.
        /// </summary>
        [Required(ErrorMessage = "Your name is required.")]
        public string FromName { get; set; }

        /// <summary>
        /// Gets or sets the to.
        /// </summary>
        [Required(ErrorMessage = "The to email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string FromEmail { get; set; }

        /// <summary>
        /// Gets or sets the phone number
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        [Required(ErrorMessage = "The message is required")]
        public string Message { get; set; }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        public IHtmlString ErrorMessage { get; internal set; }

        /// <summary>
        /// Gets or sets the confirmation message.
        /// </summary>
        public IHtmlString ConfirmationMessage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is confirmation.
        /// </summary>
        public DisplayMode ViewMode { get; set; }
    }
}