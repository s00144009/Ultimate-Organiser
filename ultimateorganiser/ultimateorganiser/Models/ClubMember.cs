using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace ultimateorganiser.Models
{
    public class ClubMember
    {
        [Key]
        public int UserId { get; set; }

        //First Name
        [Required(ErrorMessage = "You must enter a First Name")]
        [StringLength(50, ErrorMessage = "You have used too many characters")] //character remaining count
        [DataType(DataType.Text)]
        [Display(Name = "First Name")]
        public string UserFName { get; set; }

        //Last Name
        [Required(ErrorMessage = "You must enter a Last Name")]
        [StringLength(50, ErrorMessage = "You have used too many characters")] //character remaining count
        [DataType(DataType.Text)]
        [Display(Name = "Last Name")]
        public string UserLName { get; set; }

        //User Name
        [Required(ErrorMessage = "You must enter a User Name")]
        [DataType(DataType.Text)]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        //Email
        [Required(ErrorMessage = "You must enter an Email Address")]
        [RegularExpression(@"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$", ErrorMessage = "Please enter a valid email address")]
        [StringLength(50, ErrorMessage = "You have used too many characters")] //character remaining count
        [Display(Name = "Email")]
        public string UserEmail { get; set; }

        //Description
        [Display(Name = "User Description")]
        [StringLength(200, ErrorMessage = "You have used too many characters")] //character remaining count
        public string UserDescription { get; set; }

        //Date Of Brith
        [Required(ErrorMessage = "You must select your Date Of Birth")]
        [Display(Name = "Date of Birth")]
        public DateTime UserDoB { get; set; }

        //Image
        public string UserImage { get; set; }

        //Password
        //[Required]
        [StringLength(20, MinimumLength = 6)]
        public string UserPassword { get; set; }
    }//end User Class
}//end Namespace

//Validation
//https://joeylicc.wordpress.com/2013/06/20/asp-net-mvc-model-validation-using-data-annotations/