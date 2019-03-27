using ice.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace ice.Controllers
{
    public class RegisterUserController : Controller
    {
        ICECREAMPROJECTEntities db = new ICECREAMPROJECTEntities();
        // GET: RegisterUser
        [HttpGet]
        public ActionResult Register()
        {
            IEnumerable<TBL_MEMBERSHIP> li = db.TBL_MEMBERSHIP.ToList();
            ViewBag.list = new SelectList(li, "MEM_ID", "MEM_TYPE", "select");
            return View();
        }

        [HttpPost]
        public ActionResult Register(tbl_user u, HttpPostedFileBase imgfile)
        {
            try
            {
                IEnumerable<TBL_MEMBERSHIP> li = db.TBL_MEMBERSHIP.ToList();
                ViewBag.list = new SelectList(li, "MEM_ID", "MEM_TYPE", "select");
                string s = uploadimgfile(imgfile);
                if (s.Equals("-1"))
                {
                    Response.Write("<script>alert('Image Uploading Failed.....')</script>");
                }
                else
                {
                    tbl_user ur = new tbl_user();
                    ur.u_name = u.u_name;
                    ur.u_email = u.u_email;
                    ur.u_image = s;
                    ur.u_contact = u.u_contact;
                    ur.u_subs = u.u_subs;
                    ur.u_password = u.u_password;
                    ur.u_cpassword = u.u_cpassword;
                    db.tbl_user.Add(ur);
                    db.SaveChanges();
                    return RedirectToAction("AfterSignup");
                }
                

                
            }
            catch(Exception)
            {
                
            }

            return View();
        }//Action method end .....

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session.Abandon();
            Session.RemoveAll();
            return RedirectToAction("Login");
        }


        [HttpPost]
        public ActionResult Login(tbl_user u)
        {
            tbl_user ui = db.tbl_user.Where(x => x.u_email == u.u_email && x.u_password == u.u_password).SingleOrDefault();
            if (ui != null)
            {
                Session["uid"] = u.u_id;
                return RedirectToAction("UserPanel");
            }
            else
            {
                ViewBag.error = "Invalid Eamil or Password ...";
            }
            return View();
        }


        [HttpGet]
        public ActionResult UserPanel()
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Login");
            }

            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Contact(tbl_feedback fed)
        {
            
            try
            {
                db.tbl_feedback.Add(fed);
                db.SaveChanges();
                TempData["msg"] = "Feedback submitted successfully...";
                TempData.Keep();
                return RedirectToAction("Contact");
            }
            catch (Exception)
            {
                return RedirectToAction("Errorpage");
            }
            return View();
        }

        public ActionResult AfterSignup()
        {
            return View();
        }

        public ActionResult flavors(string searchFlavors)
        {
            //search flavors
            ViewBag.CurrentFilter = searchFlavors;

            var flavors = from f in db.tbl_flawors
                           select f;
            if (!String.IsNullOrEmpty(searchFlavors))
            {
                flavors = db.tbl_flawors.Where(s => s.fl_name.Contains(searchFlavors));
            }
            return View(flavors.ToList());
        }

        public ActionResult Errorpage()
        {
            return View();
        }

        public string uploadimgfile(HttpPostedFileBase file)
        {
            Random r = new Random();
            string path = "-1";
            int random = r.Next();
            if(file != null && file.ContentLength > 0)
            {
                string extension = Path.GetExtension(file.FileName);
                if(extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".jpeg") || extension.ToLower().Equals(".png"))
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