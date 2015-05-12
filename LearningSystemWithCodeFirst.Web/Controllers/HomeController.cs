using LearningSystemWithCodeFirst.Web.Entities;
using LearningSystemWithCodeFirst.Web.Factories;
using LearningSystemWithCodeFirst.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace LearningSystemWithCodeFirst.Web.Controllers
{
    public class HomeController : Controller
    {
        public UserManager<ApplicationUser> UserManager
        {
            get { return new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new IdentityDb())); }
        }

        public ActionResult Index()
        {
            List<CategoryModel> categories = CategoryModelFactory.GetAll();
            return View(categories);
        }

        public ActionResult About()
        {
            AuthorModel author;
            try
            {
                author = AuthorModel.GetModel(Utils.Constants.Author);
            }
            catch (ArgumentNullException)
            {
                TempData["ErrorMessage"] = "Error - This is not author!";
                return RedirectToAction("Index", "Home");
            }

            return View(author);
        }

        public ActionResult EditAuthorInfo()
        {
            AuthorModel author;
            try
            {
                author = AuthorModel.GetModel(Utils.Constants.Author);
            }
            catch (ArgumentNullException)
            {
                TempData["ErrorMessage"] = "Error - This is not author!";
                return RedirectToAction("Index", "Home");
            }
            return View(author);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAuthorInfo(AuthorModel model, HttpPostedFileBase file)
        {
            string newLogo;
            try
            {
                // uploaded logo
                newLogo = ImageFactory.Upload(file);
            }
            catch (FormatException)
            {
                TempData["ErrorMessage"] = "Error - File format is not supported!";
                return View(model);
            }
            catch (FieldAccessException e)
            {
                string exceptionData = e.Data["directory"].ToString();
                TempData["ErrorMessage"] = "Error - User has no access rights to the " + exceptionData + " folder.";
                return View(model);
            }
            catch (ArgumentException e)
            {
                string exceptionData = e.Data["maxSize"].ToString();
                TempData["ErrorMessage"] = "Error - Uploaded files must not exceed " + exceptionData + " MB.";
                return View(model);
            }

            string oldLogo = null;

            // if new logo is not null, set it to the model and delete older
            if (newLogo != null)
            {
                // save old logo file in another variable for using later in this method
                oldLogo = model.ImagePath;

                // assignee new logo
                model.ImagePath = newLogo;
            }

            // update client
            if (model.Update())
            {
                if (!string.IsNullOrEmpty(oldLogo))
                {
                    // delete old logo
                    ImageFactory.Delete(oldLogo);
                }

                // show message in View page
                TempData["Message"] = "Author update successful!";
                return RedirectToAction("About");
            }

            TempData["ErrorMessage"] = "Error - Wrong author!";
            return View(model);
        }
    }
}