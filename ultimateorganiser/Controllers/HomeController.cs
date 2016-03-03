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

//Google Maps API
using Geocoding;
using Geocoding.Google;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace ultimateorganiser.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        public UltimateDb db = new UltimateDb();
        ClubViewModels cvm = new ClubViewModels();
        MemberViewModels mvm = new MemberViewModels();

        //Google Maps
        private IGeocoder geocoder;
 

        //Home Page - displays a list of clubs
        public ActionResult Index()
        {
            string currentUserId = User.Identity.GetUserId();
            //ApplicationUser currentUser = db.First(x => x.Id == currentUserId);

            cvm.Clubs = db.Clubs.ToList();

            //Member Count
            cvm.MemberCount = 0;
            cvm.Clubs.ForEach(clb => cvm.MemberCount += clb.ClubMembers.Count());

            //Event Count
            cvm.EventCount = 0;
            cvm.Clubs.ForEach(clb => cvm.EventCount += clb.ClubEvents.Count());

            ViewBag.title = "Clubs List (" + cvm.NumberofClubs + ")";


            return View(cvm.Clubs);
        }

        //Details Page - Displays the details of a selected club
        [HttpGet]
        public ActionResult Details(int? id)
        {
            var selClub = db.Clubs.Find(id);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.Message = "Details Page of Club " + selClub.ClubName;

            //Delete Old Events
            foreach (var Event in selClub.ClubEvents.ToList())
            {
                //If Event date is older than one day remove this event
                if ((Event.EventDate - DateTime.Now).TotalDays < 1)
                {
                    db.ClubEvents.Remove(Event);
                    db.SaveChanges();
                }
            }

            // db.SaveChanges();
            return View(selClub);
        }

        [HttpGet]
        public ActionResult EventDetails(int? id)
        {
            //Get Selected Event details
            var selEvent = db.ClubEvents.Find(id);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }            

            //Event Information
            ViewBag.EventTitle = selEvent.EventTitle;
            ViewBag.EventDesc = selEvent.EventDesc;
            ViewBag.Location = selEvent.EventLocation;
            ViewBag.EventDate = selEvent.EventDate;

            //No location in event
            if (selEvent.EventLocation == null)
            {
                return View("EventDetails");
            }

            //Get longitude and latitude using Geocoder
            else
            {
                geocoder = new GoogleGeocoder();

                var location = geocoder.Geocode(selEvent.EventLocation).ToList();

                var f = location.First();

                ViewBag.Long = Convert.ToDouble(f.Coordinates.Longitude);
                ViewBag.Lat = Convert.ToDouble(f.Coordinates.Latitude);
                return View("EventDetails");
            }
        }

        //Create Club Partial View Popup using jQuery UI Dialog
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

        //Create a new Event for the current club - Date and time picker has to be added and set it to min date
        [HttpGet]
        public ActionResult CreateEvent(int id)
        {
            var selClub = db.Clubs.Find(id);

            if (selClub == null)
            {
                return HttpNotFound();
            }

            ViewBag.SelClubName = selClub.ClubName;
            ViewBag.SelClubId = selClub.ClubID;
            ViewBag.Date = DateTime.Today;


            ClubEvent newEvent = new ClubEvent() { ClubID = selClub.ClubID };
            return View(newEvent);
        }

        [HttpPost]
        public ActionResult CreateEvent(ClubEvent newEvent)
        {
            if (ModelState.IsValid)
            {
                //Get data entered by user on CreateEvent View
                newEvent.EventTitle =newEvent.EventTitle;
                newEvent.EventDesc = newEvent.EventDesc;
                newEvent.EventDate = Convert.ToDateTime(newEvent.EventDate);
                newEvent.eventType = newEvent.eventType;
                newEvent.eventPriority = newEvent.eventPriority;
                newEvent.EventLocation = newEvent.EventLocation;
                newEvent.ClubID = newEvent.ClubID;

                //Get the new id for this event
                var NextId = this.db.ClubEvents.Max(t => t.EventID);
                var newId = NextId + 1;
                newEvent.EventID = newId;

                //Save changes to database
                db.ClubEvents.Add(newEvent);
                db.SaveChanges();

                //Go back to Details page for the current club
                return RedirectToAction("Details", new
                {
                    id = newEvent.ClubID
                });
            }
            return RedirectToAction("Index");
        }

        //Add Member - Allows user to add new members to a selected club
        [HttpGet]
        public ActionResult AddMembers(int id)
        {
            var SelClub = db.Clubs.Find(id);

            if (SelClub == null)
            {
                return HttpNotFound();
            }

            var nm = new AddMembersToClub { ClubID = id, ClubName = SelClub.ClubName };

            //Get All the members who are not a part of the currentely selected club
            var MemList = (from r in db.ClubMembers.
                          Where(r => r.ClubID != SelClub.ClubID)
                           select r).ToList();
            //Put these selected members into a list
            var memberList = MemList.Select(m => new
            {
                MemberUserName = m.UserName,
                MemberIds = m.UserId
            }).ToList();


            nm.Members = new MultiSelectList(memberList, "MemberIds", "MemberUserName");

            ViewBag.SelClubID = SelClub.ClubID;

            return View(nm);
        }

        [HttpPost]
        public ActionResult AddMembers(AddMembersToClub model)
        {
            foreach (var MemberUserId in model.MemberIds)
            {
                ClubMember MemberSelected = db.ClubMembers.Find(MemberUserId);

                if (MemberSelected == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (MemberSelected != null)
                {
                    Club ClubSelected = db.Clubs.Find(model.ClubID);

                    ClubSelected.ClubMembers.Add(MemberSelected);
                }
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //Edit Club
        [HttpGet]
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

        //Edit Event
        [HttpGet]
        public ActionResult EditEvent(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClubEvent selEvent = db.ClubEvents.Find(id);
            if (selEvent == null)
            {
                return HttpNotFound();
            }

            ViewBag.SelClubID = selEvent.ClubID;

            ViewBag.Title = selEvent.EventTitle;

            return View(selEvent);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditEvent([Bind(Include = "EventID,EventTitle,EventDate,EventLocation,EventDesc,eventType")] ClubEvent editCEvent)
        {  

            ClubEvent SelEvent = db.ClubEvents.Find(editCEvent.EventID);

            int selectedID = SelEvent.ClubID;

            editCEvent.ClubID = selectedID;

            //Remove Old Event
            ClubEvent OldEvent = db.ClubEvents.Find(editCEvent.EventID);
            db.ClubEvents.Remove(OldEvent);
             

            //Save changes to database
            db.ClubEvents.Add(editCEvent);
            db.SaveChanges();

            //Go back to Details page for the current club
            return RedirectToAction("Details", new
            {
                id = editCEvent.ClubID
            });

            //if (ModelState.IsValid)
            //{
            //    db.Entry(editCEvent).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("Details", new
            //    {
            //        id = editCEvent.ClubID
            //    });
            //}
            //return View(editCEvent);
        }


        //Delete Method- Deletes a selected club - works with confirm dialog
        [HttpGet]
        public ActionResult Delete(int id)
        {
            Club club = db.Clubs.Find(id);
            db.Clubs.Remove(club);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //Delete Event Method- Deletes a selected event - works with confirm dialog
        //[HttpPost]
        public ActionResult DeleteEvent(int id)
        {

            ClubEvent SelEevent = db.ClubEvents.Find(id);
            db.ClubEvents.Remove(SelEevent);
            db.SaveChanges();

            return RedirectToAction("Details", new
            {
                id = SelEevent.ClubID
            });

        }

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> EventNotification(EventViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await Microsoft.AspNet.Identity.UserManager.FindByID(model.MemberId);
        //        if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.MemberId)))
        //        {
        //            // Don't reveal that the user does not exist or is not confirmed
        //            return View("HomeController");
        //        }

        //        string code = await UserManager.GeneratePasswordResetTokenAsync(user.MemberId);
        //        var callbackUrl = Url.Action("CreateEvent", "Home", new { userId = user.Id, code = code,  }, protocol: Request.Url.Scheme);
        //        await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
        //        return RedirectToAction("ForgotPasswordConfirmation", "Account");
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}


        public ActionResult About()
        {
            ViewBag.Title = "About Ultimate Organiser";

            ViewBag.Message = "Ultimate Organiser is an online club managment system which is designed to easily keep track of any upcoming events for its members.";

            return View();
        }
    }
}

//Old code / Commented out code




////Create - Allows the user to create a Club
//public ActionResult Create()
//{
//    return View();
//}

//// POST: Clubs/Create
//// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//[HttpPost]
//[ValidateAntiForgeryToken]
//public ActionResult Create([Bind(Include = "ClubID,ClubName,ClubDescription")] Club club)
//{
//    if (ModelState.IsValid)
//    {
//        db.Clubs.Add(club);
//        db.SaveChanges();
//        return RedirectToAction("Index");
//    }

//    return View(club);
//}

////Event Page - displays a list of events for the club the user selects via a dropdown list
//public ActionResult Event()
//{
//    var clubs = db.Clubs.Select(r => new SelectListItem
//    {
//        Value = r.ClubID.ToString(),
//        Text = r.ClubName
//    }).ToList();
//    ViewBag.Clubs = clubs;
//    return View();
//}


//public PartialViewResult EventPartialView(int id)
//{
//    var data = db.Clubs.Where(r => r.ClubID == id)
//                                                  .OrderByDescending(r => r.ClubEvents);
//    return PartialView(data);
//}