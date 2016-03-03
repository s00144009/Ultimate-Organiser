using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Common;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace ultimateorganiser.Models
{
    public class UltimateDb : DbContext
    {
        public UltimateDb() : base("UltimateDb") { }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<ClubEvent> ClubEvents { get; set; }
        public DbSet<ClubMember> ClubMembers { get; set; }
        public object Users { get; internal set; }
    }

    //Enums
   // public enum ClubType { Archery, Athletics, Basketball, Boxing, Camogie, Cricket, Cycling, Darts, Gaelic, Golf, Handball, Hocky, Hurling, Karate, MMA, Pool, Rugby, Snooker, Snowsports, Soccer, Surfing, Swimming, Tennis, Vollyball, Windsurfing, Youth, Other }

    public enum EventType { Meeting, Game, Training, Social, Other }

    public enum EventPriority { Low, Medium, High, None }

    public class Club
    {
        // Class to manage a single Club
        [Key]
        public int ClubID { get; set; }

        //Name
        [Required(ErrorMessage = "You must enter a Club Name")]
        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessage = "Club Name cannot be longer than 50 characters")]
        [Display(Name = "Club Name")]
        public string ClubName { get; set; }


        //Discription
        [Display(Name = "Description")]
        [StringLength(200, ErrorMessage = "Discription is too long")] //character remaining count
        public string ClubDescription { get; set; }

       
        //List of Members that are members of this Club
        public virtual List<ClubMember> ClubMembers { get; set; }

        //List of Events for this Club
        public virtual List<ClubEvent> ClubEvents { get; set; } //List of Events for this Club

    }//end Club
        

    public class ClubEvent
    {
        [Key]
        public int EventID { get; set; }

        //Event Title
        [Required(ErrorMessage = "You must enter a Title for this event")]
        [Display(Name = "Title")]
        public string EventTitle { get; set; }

        //Event Date
        [Required(ErrorMessage = "You must put a date on this event")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Date")]
        public DateTime EventDate { get; set; }

        //Event Description
        [Display(Name = "Description")]
        [DataType(DataType.Text)]
        public string EventDesc { get; set; }

        //Event Location
        [Required(ErrorMessage = "You must enter a location for this event")]
        [DataType(DataType.Text)]
        [Display(Name = "Location")]
        public string EventLocation { get; set; }

        //Event Type
        [Required]
        [Display(Name = "Event Type")]
        public EventType eventType { get; set; }

        //Event Priority
        [Required]
        [Display(Name = "Priority")]
        public EventPriority eventPriority { get; set; }



        //Event Members
        //public virtual List<ClubMember> EventMembers { get; set; }

        //Foreign Key for Club
        public int ClubID { get; set; }

        [ForeignKey("ClubID")]
        public virtual Club Club { get; set; }
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

        //Foreign Key for Club
        public int ClubID { get; set; }

        //[ForeignKey("ClubID")]
        //public virtual Club Club { get; set; }
    }//end User Class


    public class NewMemberList //Class used when adding new members to the members list of a club
    {
        public List<ClubMember> NewMembers { get; set; }
        public List<int> SelectedIDs { get; set; }
    }

  
}//end Namespace