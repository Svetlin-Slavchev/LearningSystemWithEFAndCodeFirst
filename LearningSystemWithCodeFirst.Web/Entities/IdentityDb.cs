using LearningSystemWithCodeFirst.Web.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LearningSystemWithCodeFirst.Web.Entities
{
    public class IdentityDb : IdentityDbContext<ApplicationUser>
    {
        public IdentityDb()
            : base("DefaultConnection")
        { }

        public static IdentityDb Create()
        {
            return new IdentityDb();
        }
    }
}