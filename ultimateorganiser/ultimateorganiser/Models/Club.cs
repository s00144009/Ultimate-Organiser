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

        //video paused 22:40
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
}//end Namespace