using System.ComponentModel.DataAnnotations;

namespace LearningSystemWithCodeFirst.Entities
{
    public class Lesson
    {
        public int Id { get; set; }

        [Required]
        [StringLength(250)]
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}
