using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ultimateorganiser.ViewModels
{
    public class EventViewModel
    {
        public string EventTitle { get; set; }
        public string EventDescription { set; get; }    
        public List<SelectListItem> Members { set; get; }
        public int MemberId { set; get; }
    }
}