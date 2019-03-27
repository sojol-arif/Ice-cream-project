using ice.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ice.Controllers
{
    public class AdminController : Controller
    {
        ICECREAMPROJECTEntities db = new ICECREAMPROJECTEntities();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Feedback()
        {
            return View(db.tbl_feedback.ToList().OrderByDescending(x => x.f_id));
        }

        [HttpGet]
        public ActionResult AddFlawor()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddFlawor(tbl_flawors fl, HttpPostedFileBase imgfile)
        {
            string s = uploadimgfile(imgfile);
            if (s.Equals("-1"))
            {
                Response.Write("<script>alert('Image Uploading Failed.....')</script>");
            }
            else
            {
                tbl_flawors f = new tbl_flawors();
                f.fl_name = fl.fl_name;
                f.fl_image = s;
                db.tbl_flawors.Add(f);
                db.SaveChanges();
                return RedirectToAction("AddFlawor");
            }
            return View();
        }

        public string uploadimgfile(HttpPostedFileBase file)
        {
            Random r = new Random();
            string path = "-1";
            int random = r.Next();
            if (file != null && file.ContentLength > 0)
            {
                string extension = Path.GetExtension(file.FileName);
                if (extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".jpeg") || extension.ToLower().Equals(".png"))
                {
                    try
                    {
                        path = Path.Combine(Server.MapPath("~/Content/img"), random + Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        path = "~/Content/img/" + random + Path.GetFileName(file.FileName);
                    }
                    catch (Exception ex)
                    {
                        path = "-1";
                    }
                }
                else
                {
                    Response.Write("<script>alert('Only jpg .jpeg or png format are acceptable')</script>");
                }
            }
            else
            {
                Response.Write("<script>alert('Please select a file')</script>");
                path = "-1";
            }

            return path;
        }
    }
}