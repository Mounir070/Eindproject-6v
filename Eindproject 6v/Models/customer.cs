using System.ComponentModel.DataAnnotations;

namespace Eindproject_6v.Models
{
    public class Customer
    {
        [Display(Name ="First name")]
        [Required]
        public string Firstname { get; set; }
        [Display(Name ="Family name")]
        [Required]
        public string Lastname { get; set; }
        [Display(Name ="Emailadress")]
        [Required]
        public string email { get; set; }
        [Display(Name ="Your phonenumber")]
        public string phonenumber { get; set; }
        [Display(Name = "Your address")]

        public string address { get; set; }
        [Display(Name ="Please give us a description of your problem")]
        public string description { get; set; }
    }
}
