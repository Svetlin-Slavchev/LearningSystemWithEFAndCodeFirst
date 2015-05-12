using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LearningSystemWithCodeFirst.Entities
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [StringLength(250)]
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }

        public virtual ICollection<Lesson> Lessons { get; set; }

        public Category()
        {
            this.Lessons = new HashSet<Lesson>();
        }
    }
}
