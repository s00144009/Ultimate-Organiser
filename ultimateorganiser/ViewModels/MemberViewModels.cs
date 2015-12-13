using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ultimateorganiser.Models;

namespace ultimateorganiser.ViewModels
{
    public class MemberViewModels
    {
        public int NumberOfMembers { get; set; }
        public List<ClubMember> Members { get; set; }
    }
}