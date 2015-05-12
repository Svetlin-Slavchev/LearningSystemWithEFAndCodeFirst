using LearningSystemWithCodeFirst.Entities;
using LearningSystemWithCodeFirst.Web.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace LearningSystemWithCodeFirst.Web.Models
{
    public class CategoryModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(250)]
        public string Name { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public List<LessonModel> Lessons { get; set; }

        public static bool Create(CategoryModel model)
        {
            using (LearningSystemDb db = new LearningSystemDb())
            {
                // check if exist
                if (!model.IsExist(model.Name))
                {
                    Category category = new Category()
                    {
                        Name = model.Name,
                        Description = model.Description,
                        ImagePath = model.ImagePath
                    };

                    db.Categories.Add(category);
                    db.SaveChanges();

                    return true;
                }

                return false;
            }
        }

        public bool Update()
        {
            using (LearningSystemDb db = new LearningSystemDb())
            {
                if (!this.IsExist(this.Name, this.Id))
                {
                    Category category = db.Categories.Find(this.Id);

                    category.Name = this.Name;
                    category.Description = this.Description;
                    category.ImagePath = this.ImagePath;

                    db.SaveChanges();

                    return true;
                }

                return false;
            }
        }

        public bool Delete()
        {
            using (LearningSystemDb db = new LearningSystemDb())
            {
                Category category = db.Categories.Find(this.Id);
                if (category != null)
                {
                    db.Categories.Remove(category);
                    db.SaveChanges();

                    return true;
                }

                return false;
            }
        }

        public bool IsExist(string categoryName)
        {
            return this.IsExist(categoryName, 0);
        }

        public bool IsExist(string categoryName, int id)
        {
            using (LearningSystemDb db = new LearningSystemDb())
            {
                if (db.Categories.Any(x => x.Name == categoryName && x.Id != id))
                {
                    return true;
                }

                return false;
            }
        }
    }
}