using LearningSystemWithCodeFirst.Entities;
using LearningSystemWithCodeFirst.Web.Entities;
using LearningSystemWithCodeFirst.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LearningSystemWithCodeFirst.Web.Factories
{
    public static class CategoryModelFactory
    {
        public static CategoryModel GetModel(int? id)
        {
            Category category = CategoryModelFactory.GetEntity(id);
            if (category == null)
            {
                throw new ArgumentNullException();
            }

            CategoryModel model = new CategoryModel();
            model.Id = category.Id;
            model.Name = category.Name;
            model.Description = category.Description;
            model.ImagePath = category.ImagePath;
            model.Lessons = LessonModelFactory.GetAll(category.Lessons.ToList());

            return model;
        }

        public static List<CategoryModel> GetAll()
        {
            using (LearningSystemDb db = new LearningSystemDb())
            {
                var all = db.Categories
                    .Select(x => new CategoryModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        ImagePath = x.ImagePath
                    })
                    .ToList();

                return all;
            }
        }

        private static Category GetEntity(int? id)
        {
            using (LearningSystemDb db = new LearningSystemDb())
            {
                Category category = db.Categories
                .Include("Lessons")
                .FirstOrDefault(x => x.Id == id);

                return category;
            }
        }
    }
}