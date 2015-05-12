using LearningSystemWithCodeFirst.Entities;
using LearningSystemWithCodeFirst.Web.Factories;
using LearningSystemWithCodeFirst.Web.Models;
using LearningSystemWithCodeFirst.Web.Utils;
using System;
using System.Web;
using System.Web.Mvc;

namespace LearningSystemWithCodeFirst.Web.Controllers
{
    public class LessonController : Controller
    {
        public ActionResult View(int id)
        {
            LessonModel lesson;
            try
            {
                lesson = LessonModelFactory.GetModel(id);
            }
            catch (ArgumentNullException)
            {
                TempData["ErrorMessage"] = "Error - This lesson does not exist!";
                return PartialView("Error");
            }

            ViewData["InitiatorType"] = typeof(Lesson).Name;
            return View(lesson);
        }

        [Authorize(Roles = Constants.ADMIN)]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Constants.ADMIN)]
        public ActionResult Create(LessonModel model, HttpPostedFileBase file)
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
            bool isValid = LessonModel.Create(model);
            if (!isValid)
            {
                // delete already saved file, if creation failed
                if (model.ImagePath != null)
                {
                    ImageFactory.Delete(model.ImagePath);
                }

                // throw message
                TempData["ErrorMessage"] = "Lesson can not be created!";
                return View(model);
            }

            // throw message
            TempData["Message"] = "Lesson created!";
            return RedirectToAction("View", "Category", new { id = model.CategoryId });
        }

        [Authorize(Roles = Constants.ADMIN)]
        public ActionResult Edit(int id)
        {
            LessonModel lesson;
            try
            {
                lesson = LessonModelFactory.GetModel(id);
            }
            catch (ArgumentNullException)
            {
                TempData["ErrorMessage"] = "Error - This lesson does not exist!";
                return PartialView("Error");
            }

            return View(lesson);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Constants.ADMIN)]
        public ActionResult Edit(LessonModel model, HttpPostedFileBase file)
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

            // update lesson
            if (model.Update())
            {
                if (!string.IsNullOrEmpty(oldLogo))
                {
                    // delete old logo
                    ImageFactory.Delete(oldLogo);
                }

                ViewData["Message"] = "Lesson update successful!";
                return RedirectToAction("View", "Lesson", new { id = model.Id });
            }

            TempData["ErrorMessage"] = "Error - update is not successful! The lesson name is already used.";
            return RedirectToAction("Edit", new { id = model.Id });
        }

        [Authorize(Roles = Constants.ADMIN)]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            LessonModel model = LessonModelFactory.GetModel(id);
            if (model.Delete())
            {
                TempData["Message"] = "Lesson deleted!";
                return RedirectToAction("View", "Category", new { id = model.CategoryId });
            }

            TempData["ErrorMessage"] = "Error - This lesson does not exist!";
            return PartialView("Error");
        }
    }
}