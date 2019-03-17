namespace Shop.Web.Data.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Identity;

    public class User : IdentityUser
    {
        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string FirstName { get; set; }

        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string LastName { get; set; }

        public int Stratum { get; set; }

        public string Gender { get; set; }

        [Required]
        [Display(Name = "Birthdate")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime OrderDate { get; set; }

        public int CityId { get; set; }

        public City City { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get { return $"{this.FirstName} {this.LastName}"; } }
    }
}