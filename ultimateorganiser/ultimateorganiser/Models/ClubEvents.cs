using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ultimateorganiser.Models
{
    public class ClubEvent
    {
        [Key]
        public int EventID { get; set; }

        [Required(ErrorMessage = "You must enter a Title for this event")]
        [Display(Name = "Event Title")]
        public string EventTitle { get; set; }

        [Required(ErrorMessage = "You must put a date on this event")]
        [DataType(DataType.DateTime)]
        public DateTime EventDate { get; set; }

        public enum EventType { Meeting, Game, Training, Social, Other }

        [Display(Name = "Event Description")]
        [DataType(DataType.Text)]
        public string EventDesc { get; set; }

    }//end ClubEvents
}//end Namespace