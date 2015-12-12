using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ultimateorganiser.Models;

namespace ultimateorganiser.Controllers
{
    public class SliderController : Controller
    {
        // GET: Slider
        public ActionResult Index()
        {
            using (UltimateDb db = new UltimateDb())
            {
                return View(db.gallery.ToList());
            }
            //return View();
        }

        //Add image to Slider
        public ActionResult AddImage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddImage(HttpPostedFileBase ImagePath)
        {
            if (ImagePath != null)
            {
                //This block of code is to force the user to upload speciic resolution pics
                System.Drawing.Image img = System.Drawing.Image.FromStream(ImagePath.InputStream);
                if ((img.Width != 800) || (img.Height != 356))
                {
                    ModelState.AddModelError("", "Image resolution must be 800 x 356 pixels");
                    return View();
                }

                //Upload your pic
                string pic = System.IO.Path.GetFileName(ImagePath.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/Content/Images"), pic);
                ImagePath.SaveAs(path);

                using (UltimateDb db = new UltimateDb())
                {
                    Gallery gallery = new Gallery { ImagePath = "~/Content/Images" + pic };
                    db.gallery.Add(gallery);
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }

        //Delete Multiple Images
        public ActionResult DeleteImages()
        {
            using (UltimateDb db = new UltimateDb())
            {
                return View(db.gallery.ToList());
            }
        }

        [HttpPost]
        public ActionResult DeleteImages(IEnumerable<int> ImageIDs)
        {
            using (UltimateDb db = new UltimateDb())
            {
                foreach (var id in ImageIDs)
                {
                    var image = db.gallery.Single(s => s.Id == id);
                    string imgPath = Server.MapPath(image.ImagePath);
                    db.gallery.Remove(image);
                    if (System.IO.File.Exists(imgPath))
                        System.IO.File.Delete(imgPath);                    
                }
                db.SaveChanges();
            }
            return RedirectToAction("DeleteImages");
        }
    }

}