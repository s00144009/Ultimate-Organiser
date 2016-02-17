using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ultimateorganiser.Models;
using ultimateorganiser.ViewModels;
using System.Data;

namespace ultimateorganiser.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        public UltimateDb db = new UltimateDb();
        ClubViewModels cvm = new ClubViewModels();
        MemberViewModels mvm = new MemberViewModels();

        //Home Page - displays a list of clubs
        public ActionResult Index()
        {
            cvm.Clubs = db.Clubs.ToList();
            cvm.NumberofClubs = cvm.Clubs.Count();
            cvm.MemberCount = 0;
            cvm.Clubs.ForEach(clb => cvm.MemberCount += clb.ClubMembers.Count());
            ViewBag.title = "Clubs List (" + cvm.NumberofClubs + ")";

            return View(cvm.Clubs);
        }

        //Create Club Partial View Popup using jQuery UI Dialog - *NEEDS TO BE LOOKED OVER AND TESTED*
        [HttpGet]
        public ActionResult CreateClubPartialView()
        {
            return PartialView();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateClubPartialView([Bind(Include = "ClubID,ClubName,ClubDescription")] Club club)
        {
            if (ModelState.IsValid)
            {
                db.Clubs.Add(club);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(club);
        }

       
        //Add Events to current club
        [HttpGet]
        public ActionResult CreateEvent(int id)   //*Edit and delete functions for events*
        {
            var SelClub = db.Clubs.Find(id);

            //If no club is found handle it
            if (SelClub == null)
            {
                return HttpNotFound();
            }

            ViewBag.ClubName = SelClub.ClubName;
            ViewBag.ClubID = SelClub.ClubID;

            return View();
        }

        //This is the Controller for creating the event
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateEvent([Bind(Include = "EventTitle,EventDate,EventDesc,eventType,ClubID")] ClubEvent newEvent)
        {

            if (ModelState.IsValid)
            {
                db.ClubEvents.Add(newEvent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(newEvent);
        }


        //Add Members - Displays a page with a list of all members and a list of current members
        public ActionResult AddMembers(int id)
        {
            var Selclub = db.Clubs.Find(id);
         
            //if no club is found handle it   
            if (Selclub == null)
            {
                return HttpNotFound();
            }

            var cm = new AddMembersToClub { ClubId = id, ClubName = Selclub.ClubName };

            cm.Members = db.ClubMembers.Select(m => new SelectListItem
                        {
                            Value = m.ClubID.ToString(),
                            Text = m.UserName
                        }).ToList();         

            return View(cm);
        }

        [HttpPost]
        public ActionResult AddMembers(NewMemberList model)
        {
            foreach (var item in model.SelectedIDs)
            {
                var newMember = db.ClubMembers.FirstOrDefault(nm => nm.UserId == item);

                //If no members is passed back handle error
                if (newMember == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (newMember != null)
                {
                    //db.Entry(club).State = EntityState.Modified;
                    //db.SaveChanges();
                    //return RedirectToAction("Index");
                }
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }


        //Create - Allows the user to create a Club
        public ActionResult Create()
        {
            return View();
        }

        // POST: Clubs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ClubID,ClubName,ClubDescription")] Club club)
        {
            if (ModelState.IsValid)
            {
                db.Clubs.Add(club);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(club);
        }

        //Event Page - displays a list of events for the club the user selects via a dropdown list
        public ActionResult Event()
        {
            var clubs = db.Clubs.Select(r => new SelectListItem
            {
                Value = r.ClubID.ToString(),
                Text = r.ClubName
            }).ToList();
            ViewBag.Clubs = clubs;
            return View();
        }


        public PartialViewResult EventPartialView(int id)
        {
            var data = db.Clubs.Where(r => r.ClubID == id)
                                                          .OrderByDescending(r => r.ClubEvents);
            return PartialView(data);
        }




        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Club club = db.Clubs.Find(id);
            if (club == null)
            {
                return HttpNotFound();
            }

            ViewBag.ClubName = club.ClubName;

            cvm.Clubs = db.Clubs.ToList();

            return View(club);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ClubID,ClubName,ClubDescription,ClubMembers")] Club club)
        {
            if (ModelState.IsValid)
            {
                db.Entry(club).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(club);
        }



        //Details Page - Displays the details of a selected club
        public ActionResult Details(int? id)
        {
            var selClub = db.Clubs.Find(id);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.Message = "Details Page of Club " + selClub.ClubName;

                    // db.SaveChanges();
                    return View(selClub);
        }

        //Delete Method- Deletes a selected club - works with confirm dialog
        public ActionResult Delete(int id)
        {
            Club club = db.Clubs.Find(id);
            db.Clubs.Remove(club);
            db.SaveChanges();
            return RedirectToAction("Index");            

        }

        public ActionResult Members()
        {

            mvm.Members = db.ClubMembers.ToList();
            ViewBag.title = "Members List";
            return View(db.ClubMembers.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Title = "About Ultimate Organiser";

            ViewBag.Message = "Ultimate Organiser is an online club managment system which is designed to easily keep track of any upcoming events for its members.";

            return View();
        }
    }
}

//Old code / Commented out code


//public ActionResult Index(ClubEvent)
//{
//    return PartialView();
//}

////Add Event
//[HttpPost]
//public ActionResult Index(ClubEvent Event)
//{
//    if (ModelState.IsValid)
//    {
//        using (var db = new UltimateDb())
//        {
//            var newEvent = new ClubEvent();
//            newEvent.EventTitle = Event.EventTitle;
//            newEvent.EventDesc = Event.EventDesc;
//            newEvent.eventType = Event.eventType;


//            db.ClubEvents.Add(newEvent);
//            db.SaveChanges();
//        }
//    }

//    return PartialView();
//}




//Add Event
//public ActionResult CreateEvent()
//{
//    var vm = new EventViewModel();
//    {
//        vm.Members = db.ClubMembers.Select(s => new SelectListItem
//        {
//            Value = s.UserImage.ToString(),
//            Text = s.UserEmail
//        }).ToList();
//    }
//    return View( new EventViewModel());
//}

//Add Event
//[HttpPost]
//public ActionResult CreateEvent(EventViewModel model)
//{
//    if(ModelState.IsValid)
//    {
//        using (var db = new UltimateDb())
//        {
//            var newEvent = new ClubEvent();
//            newEvent.EventTitle = model.EventTitle;
//            newEvent.EventDesc = model.EventDescription;

//            db.ClubEvents.Add(newEvent);
//            db.SaveChanges();
//        }
//    }

//    return View();
//}


//Event Page
//[HttpGet]
//public ActionResult Event()
//{
//    var clubs = new SelectList(
//      db.Clubs.Select(r => r.ClubName).Distinct().ToList());

//    ViewBag.Clubs = clubs;
//    return View();
//}

//Event Page
//[HttpGet]
//public ActionResult Event()
//{
//    var clubs = new SelectList(
//      db.Clubs.Select(r => r.ClubName).Distinct().ToList());

//    ViewBag.Clubs = clubs;
//    return View();
//}



////Edit Club (error)
//public ActionResult Edit(int? id)
//{
//    if (id == null)
//    {
//        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//    }
//    Club club = db.Clubs.Find(id);
//    if (club == null)
//    {
//        return HttpNotFound();
//    }
//    return View(club);
//}

////Edit Club
//[HttpPost]
//[ValidateAntiForgeryToken]
//public ActionResult Edit([Bind()] Club club)
//{
//    if (ModelState.IsValid)
//    {
//        db.Entry(club).State = EntityState.Modified;
//        db.SaveChanges();
//        return RedirectToAction("Index");
//    }
//    return View(club);
//}



//Edit Club Partial View - Allows the user to edit a selected clubs information
//*Allow the club admin to remove and add members of the club*
//[HttpGet]
//public ActionResult EditClubPartialView(int? id)
//{
//    if (id == null)
//    {
//        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//    }
//    Club club = db.Clubs.Find(id);
//    if (club == null)
//    {
//        return HttpNotFound();
//    }
//    return PartialView(club);
//}

//[HttpPost]
//[ValidateAntiForgeryToken]
//public ActionResult EditClubPartialView([Bind()] Club club)
//{
//    if (ModelState.IsValid)
//    {
//        db.Entry(club).State = EntityState.Modified;
//        db.SaveChanges();
//        return RedirectToAction("Index");
//    }
//    return View(club);
//}

//Add Members - Displays a page with a list of all members and a list of current members
//[HttpGet]
//public ActionResult AddMembers(int? id)
//{
//    //****Remove Members that are already in the Team from the list****

//    //ClubMember oCustomer = new ClubMember()

//    if (id == null)
//    {
//        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//    }

//    Club club = db.Clubs.Find(id);

//    if (club == null)
//    {
//        return HttpNotFound();
//    }

//    List<ClubMember> CurrentMembers = club.ClubMembers;

//    List<ClubMember> MembersList = new List<ClubMember>();
//    MembersList = db.ClubMembers.ToList();

//    ViewBag.CurrentMembersList = CurrentMembers;
//    return View(new NewMemberList() { NewMembers = MembersList });
//}

//[HttpPost]
//[ValidateAntiForgeryToken]
//public ActionResult AddMembers([Bind(Include = "SelectedIDs")] Club club)
//{
//    if (ModelState.IsValid)
//    {
//        db.Entry(club).State = EntityState.Modified;
//        db.SaveChanges();
//        return RedirectToAction("Index");
//    }
//    return View(club);
//}


//[HttpPost]
//[ValidateAntiForgeryToken]
//public ActionResult CreateEvent([Bind(Include = "EventTitle,EventDate,EventDesc,EventType,ClubID")] ClubEvent newEvent)
//{
//    //ModelState.Remove("EventID");

//    if (ModelState.IsValid)
//    {
//        db.ClubEvents.Add(newEvent);
//        db.SaveChanges();
//        return RedirectToAction("Details");
//}
//    return View();
//}