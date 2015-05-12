using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace LearningSystemWithCodeFirst.Web.Utils
{
    public class Constants
    {
        public const string ADMIN = "Admin";

        private static string noImage = ConfigurationSettings.AppSettings["noImage"];

        public static string NoImage
        {
            get { return noImage; }
            set { noImage = value; }
        }

        private static string author = ConfigurationSettings.AppSettings["author"];

        public static string Author
        {
            get { return author; }
            set { author = value; }
        }
        
    }
}