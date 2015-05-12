using LearningSystemWithCodeFirst.Entities;
using LearningSystemWithCodeFirst.Web.Entities;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace LearningSystemWithCodeFirst.Web.Models
{
    public class LessonModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(250)]
        public string Name { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public int CategoryId { get; set; }

        public LessonModel()
        { }

        public static bool Create(LessonModel model)
        {
            using (LearningSystemDb db = new LearningSystemDb())
            {
                // check if exist
                if (!model.IsExist(model.Name))
                {
                    Lesson newLesson = new Lesson()
                    {
                        Name = model.Name,
                        Description = model.Description,
                        CategoryId = model.CategoryId,
                        ImagePath = model.ImagePath
                    };

                    db.Lessons.Add(newLesson);
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
                    Lesson lesson = db.Lessons.Find(this.Id);

                    lesson.Name = this.Name;
                    lesson.Description = this.Description;
                    lesson.CategoryId = this.CategoryId;
                    lesson.ImagePath = this.ImagePath;

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
                Lesson lesson = db.Lessons.Find(this.Id);
                if (lesson != null)
                {
                    db.Lessons.Remove(lesson);
                    db.SaveChanges();

                    return true;
                }

                return false;
            }
        }

        public bool IsExist(string lessonName)
        {
            return this.IsExist(lessonName, 0);
        }

        public bool IsExist(string lessonName, int id)
        {
            using (LearningSystemDb db = new LearningSystemDb())
            {
                if (db.Lessons.Any(x => x.Name == lessonName && x.Id != id))
                {
                    return true;
                }

                return false;
            }
        }
    }
}