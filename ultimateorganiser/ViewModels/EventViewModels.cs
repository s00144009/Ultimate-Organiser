using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ultimateorganiser.Models;

namespace ultimateorganiser.ViewModels
{
    public class EventViewModels
    {

        public int NumberofEvents { get; set; }
        public List<ClubEvent> Events { get; set; }
        public int EventCount { get; set; }
    }
}