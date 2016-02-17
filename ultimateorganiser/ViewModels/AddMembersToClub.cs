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
        public int ClubId { set; get; }
        public List<SelectListItem> Members { set; get; }
        public int[] SelectedMembers { set; get; }
    }
}