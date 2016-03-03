using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ultimateorganiser.Models;
using System.Web.Mvc;

namespace ultimateorganiser.ViewModels
{
    public class AddMembersToClub
    {
        public string ClubName { set; get; }
        public int ClubID { set; get; }
        public int[] MemberIds { get; set; }
        public string[] MemberUserName { get; set; }
        public MultiSelectList Members { get; set; }
    }
}