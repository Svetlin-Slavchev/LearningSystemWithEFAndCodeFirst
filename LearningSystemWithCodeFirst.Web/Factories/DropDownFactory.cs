using LearningSystemWithCodeFirst.Web.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LearningSystemWithCodeFirst.Web.Factories
{
    public static class DropDownFactory
    {
        private static LearningSystemDb db = new LearningSystemDb();

        public static List<SelectListItem> GetCategories()
        {
            return GetCategories(0);
        }

        public static List<SelectListItem> GetCategories(int selectedId)
        {
            List<SelectListItem> listItemCollection = new List<SelectListItem>();

            var categories = db.Categories.ToList();

            foreach (var category in categories)
            {
                if (selectedId != 0 && selectedId == category.Id)
                {
                    SelectListItem item = new SelectListItem() { Text = category.Name, Value = category.Id.ToString(), Selected = true };
                    listItemCollection.Add(item);
                }
                else
                {
                    SelectListItem item = new SelectListItem() { Text = category.Name, Value = category.Id.ToString() };
                    listItemCollection.Add(item);
                }
            }

            return listItemCollection;
        }
    }
}