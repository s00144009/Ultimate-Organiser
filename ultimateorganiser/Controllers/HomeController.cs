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

        //Home Page
        public ActionResult Index()
        {
            cvm.Clubs = db.Clubs.ToList();
            cvm.NumberofClubs = cvm.Clubs.Count();
            cvm.MemberCount = 0;
            cvm.Clubs.ForEach(clb => cvm.MemberCount += clb.ClubMembers.Count());
            ViewBag.title = "Clubs List (" + cvm.NumberofClubs + ")";
            return View(cvm.Clubs);
        }

        //Details Page
        public ActionResult Details(int? id)
        {
            var selClub = db.Clubs.Find(id);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.Message = "Details Page of Club " + selClub.ClubName;
            db.SaveChanges();
            return View(selClub);
        }

        //Add Event
        public ActionResult CreateEvent()
        {
            var vm = new EventViewModel();
            {
                vm.Members = db.ClubMembers.Select(s => new SelectListItem
                {
                    Value = s.UserImage.ToString(),
                    Text = s.UserEmail
                }).ToList();
            }
            return View( new EventViewModel());
        }

        //Add Event
        [HttpPost]
        public ActionResult CreateEvent(EventViewModel model)
        {
            if(ModelState.IsValid)
            {
                using (var db = new UltimateDb())
                {
                    var newEvent = new ClubEvent();
                    newEvent.EventTitle = model.EventTitle;
                    newEvent.EventDesc = model.EventDescription;

                    db.ClubEvents.Add(newEvent);
                    db.SaveChanges();
                }
            }

            model.Members = db.ClubMembers.Select(s => new SelectListItem
            {
                Value = s.UserId.ToString(),
                Text = s.UserEmail
            }).ToList();

            return View();
        }

        //Edit Club (error)
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
            return View(club);
        }

        //Edit Club
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind()] Club club)
        {
            if (ModelState.IsValid)
            {
                db.Entry(club).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(club);
        }

        //Delete Club(error)
        [HttpGet]
        public ActionResult DeleteConfirmed(int? clubid)
        {
            if (clubid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Club club = db.Clubs.Find(clubid);
            if (club == null)
            {
                return HttpNotFound();
            }
            return View(club);
        }

        //Delete Club
        [HttpPost, ActionName("DeleteConfirmed")]
        public ActionResult DeleteConfirmed(int clubid)
        {
            Club club = db.Clubs.Find(clubid);
            var membersToDelete = club.ClubMembers.ToList();
            foreach (var item in membersToDelete)
            {
                db.ClubMembers.Remove(item);
            }

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

        public ActionResult Contact()
        {
            ViewBag.Message = "This is the Calander page";

            return View();
        }
    }
}