using LearningSystemWithCodeFirst.Entities;
using System.Data.Entity;

namespace LearningSystemWithCodeFirst.Web.Entities
{
    public class LearningSystemDb : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Lesson> Lessons { get; set; }

        public LearningSystemDb()
            : base("DefaultConnection")
        { }
    }
}