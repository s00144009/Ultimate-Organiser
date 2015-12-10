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
        EventViewModels evm = new EventViewModels();

        //Home Page
        public ActionResult Index()
        {
            #region MyClass definition
            //Club
            //cvm.Clubs = db.Clubs.ToList();
            //cvm.NumberofClubs = cvm.Clubs.Count();
            //cvm.MemberCount = 0;
            //cvm.Clubs.ForEach(clb => cvm.MemberCount += clb.ClubMembers.Count());
            //ViewBag.title = "Clubs List (" + cvm.NumberofClubs + ")";
            //return View(cvm.Clubs);
            #endregion
            //Events
            evm.Events = db.ClubEvents.ToList();
            ViewBag.title = "Home Page";
            ViewBag.PageTitle = "Home";

            return View(evm.Events);
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

        //My Profile
        public ActionResult MyProfile(int? id)
        {

            ViewBag.title = "My profile";
            ViewBag.PageTitle = "My Profile";

            var user = db.ClubMembers.Find(id);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.Message = user.UserName;
            return View(user);
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