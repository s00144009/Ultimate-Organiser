using ultimateorganiser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ultimateorganiser.ViewModels
{
    public class ClubViewModels
    {
        public int NumberofClubs { get; set; }
        public List<Club> Clubs { get; set; }
        public List<ClubMember> ClubMembers { get; set; }
        public int MemberCount { get; set; }
    }
}