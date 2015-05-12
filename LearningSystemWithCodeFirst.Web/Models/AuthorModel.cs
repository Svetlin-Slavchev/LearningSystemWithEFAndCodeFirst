using LearningSystemWithCodeFirst.Web.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LearningSystemWithCodeFirst.Web.Models
{
    public class AuthorModel
    {
        private IdentityDb db = new IdentityDb();

        [Required]
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        public string ImagePath { get; set; }

        public static AuthorModel GetModel(string userName)
        {
            using (IdentityDb db = new IdentityDb())
            {
                ApplicationUser user = db.Users
                .Where(x => x.UserName == userName)
                .FirstOrDefault();

                if (user == null)
                {
                    throw new ArgumentNullException();
                }

                AuthorModel model = new AuthorModel()
                {
                    Name = user.UserName,
                    Mobile = user.Mobile,
                    Email = user.Email,
                    Description = user.Description,
                    ImagePath = user.ImagePath
                };

                return model;
            }
        }

        public bool Update()
        {
            ApplicationUser user = db.Users
                .Where(x => x.UserName == this.Name)
                .FirstOrDefault();
            if (user != null)
            {
                user.Mobile = this.Mobile;
                user.Email = this.Email;
                user.Description = this.Description;
                user.ImagePath = this.ImagePath;

                db.SaveChanges();
                return true;
            }

            return false;
        }
    }
}