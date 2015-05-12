using LearningSystemWithCodeFirst.Entities;
using LearningSystemWithCodeFirst.Web.Factories;
using LearningSystemWithCodeFirst.Web.Models;
using LearningSystemWithCodeFirst.Web.Utils;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace LearningSystemWithCodeFirst.Web.Controllers
{
    public class CategoryController : Controller
    {
        public ActionResult Listing()
        {
            List<CategoryModel> all = CategoryModelFactory.GetAll();
            return View(all);
        }

        public ActionResult View(int? id)
        {
            if (id != null)
            {
                CategoryModel category;
                try
                {
                    category = CategoryModelFactory.GetModel(id);
                }
                catch (ArgumentNullException)
                {
                    TempData["ErrorMessage"] = "Error - Categoty not exist!";
                    return RedirectToAction("Listing", "Category");
                }

                ViewData["InitiatorType"] = typeof(Category).Name;
                return View(category);
            }

            return View();
        }

        [Authorize(Roles = Constants.ADMIN)]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = Constants.ADMIN)]
        public ActionResult Create(CategoryModel model, HttpPostedFileBase file)
        {
            try
            {
                model.ImagePath = ImageFactory.Upload(file);
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

            // try to create new category
            bool isValid = CategoryModel.Create(model);
            if (!isValid)
            {
                // delete already saved file, if creation failed
                if (model.ImagePath != null)
                {
                    ImageFactory.Delete(model.ImagePath);
                }

                // throw message
                TempData["ErrorMessage"] = "Category can not be created!";
                return View(model);
            }

            // throw message
            TempData["Message"] = "Category created!";
            return RedirectToAction("Listing", "Category");
        }

        [Authorize(Roles = Constants.ADMIN)]
        public ActionResult Edit(int id)
        {
            CategoryModel category;
            try
            {
                category = CategoryModelFactory.GetModel(id);
            }
            catch (ArgumentNullException)
            {
                TempData["ErrorMessage"] = "Error - This category does not exist!";
                return PartialView("Error");
            }

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Constants.ADMIN)]
        public ActionResult Edit(CategoryModel model, HttpPostedFileBase file)
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

            // update category
            model.Update();

            if (!string.IsNullOrEmpty(oldLogo))
            {
                // delete old logo
                ImageFactory.Delete(oldLogo);
            }

            ViewData["Message"] = "Category update successful!";
            return RedirectToAction("View", "Category", new { id = model.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Constants.ADMIN)]
        public ActionResult Delete(int id)
        {
            CategoryModel model = new CategoryModel() { Id = id };
            if (model.Delete())
            {
                TempData["Message"] = "Category deleted!";
                return RedirectToAction("Listing", "Category");
            }

            TempData["ErrorMessage"] = "Error - This category does not exist!";
            return PartialView("Error");
        }
    }
}