namespace Vote.Web.Models
{
    using System.ComponentModel.DataAnnotations;

    public class RecoverPasswordWebViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
