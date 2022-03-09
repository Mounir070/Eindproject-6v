using System.ComponentModel.DataAnnotations;

namespace Eindproject_6v.Models
{
    public class Customer
    {
        [Display(Name = "First name")]
        [Required (ErrorMessage = "Please fill in your first name.")]
        public string Firstname { get; set; }
        [Display(Name = "Familyname")]
        [Required (ErrorMessage = "Please give us your familyname.")]
        public string Lastname { get; set; }
        [Display(Name = "Emailadress")]
        [Required(ErrorMessage = "Please give us your emailadress so we can contact you if necessary.")]
        public string email { get; set; }
        [Display(Name = "Your phonenumber")]
        public string phonenumber { get; set; }
        [Display(Name = "Your address")]

        public string address { get; set; }
        [Required(ErrorMessage = "Please give us a global explanation of your question.")]
        [Display(Name ="Please give us a description of your problem")]
        public string description { get; set; }
    }
}
