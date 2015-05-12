using LearningSystemWithCodeFirst.Web.Factories;
using System.Collections.Generic;

namespace LearningSystemWithCodeFirst.Web.Models
{
    public class SearchModel
    {
        public CategoryModel Parent { get; set; }
        public ICollection<LessonModel> Children { get; set; }

        public SearchModel()
        { }

        public SearchModel(int categoryId)
        {
            this.Parent = CategoryModelFactory.GetModel(categoryId);
            this.Children = LessonModelFactory.GetAll(categoryId);
        }
    }
}