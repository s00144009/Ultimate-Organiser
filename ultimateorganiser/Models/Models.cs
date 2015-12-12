using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Common;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Data.Entity;

namespace ultimateorganiser.Models
{
    public class UltimateDb : DbContext
    {
        public UltimateDb() : base("UltimateDb"){ }
        public DbSet<Gallery> gallery { get; set; }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<ClubEvent> ClubEvents { get; set; }
        public DbSet<ClubMember> ClubMembers { get; set; }

        //link https://www.youtube.com/watch?v=oHKKZ6erEFY
    }

    public class Club
    {
        // Class to manage a single Club
        [Key]
        public int ClubID { get; set; }

        //Name
        //[Required(ErrorMessage = "You must enter a Club Name")]
        //[DataType(DataType.Text)]
        //[StringLength(50, ErrorMessage = "Club Name cannot be longer than 50 characters")]
        //[Display(Name = "Club Name")]
        public string ClubName { get; set; }

        //Discription
        [Display(Name = "Description")]
        [StringLength(200, ErrorMessage = "Discription is too long")] //character remaining count
        public string ClubDescription { get; set; }

        //Club Type
        public enum ClubType { Archery, Athletics, Basketball, Boxing, Camogie, Cricket, Cycling, Darts, Gaelic, Golf, Handball, Hocky, Hurling, Karate, MMA, Pool, Rugby, Snooker, Snowsports, Soccer, Surfing, Swimming, Tennis, Vollyball, Windsurfing, Youth, Other }

        //List of Members that are members of this Club
        public virtual List<ClubMember> ClubMembers { get; set; }

        //List of Events for this Club
        public virtual List<ClubEvent> ClubEvents { get; set; } //List of Events for this Club

        public String ClubImage { get; set; }

    }//end Club

    //Club Event
    public partial class ClubEvent
    {
        [Key]
        public int EventID { get; set; }

        [Required(ErrorMessage = "You must enter a Title for this event")]
        [Display(Name = "Event Title")]
        public string EventTitle { get; set; }

        [Required(ErrorMessage = "You must put a date on this event")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Date")]
        public DateTime EventDate { get; set; }

        public enum EventType { Meeting, Game, Training, Social, Other }

        [Display(Name = "Description")]
        [DataType(DataType.Text)]
        public string EventDesc { get; set; }

        //Link table

    }//end ClubEvents

    public partial class ClubEvent
    {
        //List of Events
        public List<ClubEvent> EventPartialModel { get; set; }
    }

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
        [StringLength(25, ErrorMessage = "You have used too many characters")] //character remaining count
        [DataType(DataType.Text)]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        //Email
        [Required(ErrorMessage = "You must enter an Email Address")]
        [RegularExpression(@"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$", ErrorMessage = "Please enter a valid email address")]
        [StringLength(254, ErrorMessage = "You have used too many characters")] //character remaining count
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
        [Required(ErrorMessage = "You must enter a Password")]
        [Display(Name = "Password")]
        [StringLength(20, MinimumLength = 6)]
        public string UserPassword { get; set; }

        // Confirm Password
        [Required(ErrorMessage = "You must confirm your Password")]
        [Compare("UserPassword", ErrorMessage = "Passwords do not match")]        
        [Display(Name = "Compare Password")]
        public string ConfirmPassword { get; set; }
    }//end User Class

    //Gallery
    public class Gallery
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
    }
}//end Namespace