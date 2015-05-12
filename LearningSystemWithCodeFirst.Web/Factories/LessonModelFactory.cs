using LearningSystemWithCodeFirst.Entities;
using LearningSystemWithCodeFirst.Web.Entities;
using LearningSystemWithCodeFirst.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LearningSystemWithCodeFirst.Web.Factories
{
    public static class LessonModelFactory
    {
        public static LessonModel GetModel(int id)
        {
            Lesson entity = LessonModelFactory.GetEntity(id);
            if (entity == null)
            {
                throw new ArgumentNullException();
            }

            LessonModel model = new LessonModel();
            model.Id = entity.Id;
            model.Name = entity.Name;
            model.Description = entity.Description;
            model.CategoryId = entity.Category.Id;
            model.ImagePath = entity.ImagePath;

            return model;
        }

        public static List<LessonModel> GetAll(int categoryId)
        {
            using (LearningSystemDb db = new LearningSystemDb())
            {
                var all = db.Lessons
                    .Include("Categories")
                    .Where(x => x.Category.Id == categoryId)
                    .Select(x => new LessonModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        CategoryId = x.Category.Id,
                        ImagePath = x.ImagePath
                    })
                    .ToList();

                return all;
            }
        }

        public static List<LessonModel> GetAll(List<Lesson> items)
        {
            var models = items.Select(x => new LessonModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    CategoryId = x.Category.Id,
                    ImagePath = x.ImagePath
                })
                .ToList();

            return models;
        }

        private static Lesson GetEntity(int id)
        {
            using (LearningSystemDb db = new LearningSystemDb())
            {
                Lesson lesson = db.Lessons
                .Include("Category")
                .FirstOrDefault(x => x.Id == id);

                return lesson;
            }
        }
    }
}