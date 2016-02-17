using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ultimateorganiser.Models;

namespace ultimateorganiser.ViewModels
{
    public class IndexViewModel
    {
        //The purpose of this class is to allow me to reference 2 models (Club and Event) in the Index View
        public Club ClubViewModel { get; set; }
        public ClubEvent EventViewModel { get; set; }
    }
}