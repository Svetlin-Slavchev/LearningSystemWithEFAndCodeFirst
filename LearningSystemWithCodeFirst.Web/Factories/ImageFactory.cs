using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Web;


namespace LearningSystemWithCodeFirst.Web.Factories
{
        public class ImageFactory
    {
        protected static string uploadFolder = ConfigurationManager.AppSettings["UploadImageFolder"];
        protected static string uploadFileSizeBytes = ConfigurationManager.AppSettings["UploadImageSize"];

        // list from all valid image formats
        protected static List<string> fileContentTypes = new List<string>()
        {
            "image/bmp", "image/cis-cod", "image/gif", "image/ief", "image/jpeg", "image/pipeg", "image/svg+xml", 
            "image/tiff", "image/x-cmu-raster", "image/x-cmx", "image/x-icon", "image/x-portable-anymap", "image/x-portable-bitmap", "image/png",
            "image/x-portable-graymap", "image/x-portable-pixmap", "image/x-rgb", "image/x-xbitmap", "image/x-xpixmap", "image/x-xwindowdump"
        };

        /// <summary>
        ///     Factory method for uploading images.
        /// </summary>
        /// <param name="file">File submitted from the request.</param>
        public static string Upload(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                if (!fileContentTypes.Contains(file.ContentType))
                {
                    throw new FormatException();
                }

                // create a unique name for the uploaded file 
                var uniqueFileName = string.Format("{0}", Guid.NewGuid());
                string dir = HttpContext.Current.Server.MapPath(ImageFactory.uploadFolder);
                string extension = Path.GetExtension(file.FileName);

                // check if directory exists. if not create it
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                // Check if logged windows user has access rights to the given directory
                if (!ImageFactory.HasWritePermission(dir))
                {
                    FieldAccessException exception = new FieldAccessException();
                    exception.Data.Add("directory", dir);
                    throw exception;
                }

                // get image size from web config
                long uploadFileSizeMegaBytes = long.Parse(ImageFactory.uploadFileSizeBytes) / (1024 * 1024);

                // check file size
                if (file.ContentLength > long.Parse(uploadFileSizeBytes))
                {
                    ArgumentException exception = new ArgumentException();
                    exception.Data.Add("maxSize", uploadFileSizeMegaBytes);
                    throw exception;
                }

                string path = HttpContext.Current.Server.MapPath(uploadFolder + "/" + uniqueFileName + extension);
                file.SaveAs(path);

                // return only file name
                return uniqueFileName + extension;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        ///     Method for checking access rights to a given folder.
        /// </summary>
        /// <param name="directoryPath">Path to the selected directory.</param>
        protected static bool HasWritePermission(string directoryPath)
        {
            System.Security.AccessControl.AuthorizationRuleCollection collection =
                Directory.GetAccessControl(directoryPath).GetAccessRules(true, true, typeof(NTAccount));

            foreach (FileSystemAccessRule rule in collection)
            {
                if ((rule.FileSystemRights & FileSystemRights.WriteData) > 0)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Delete file from project tree 
        /// </summary>
        /// <param name="inputFile">Path to the selected file.</param>
        public static void Delete(string inputFile)
        {
            if (inputFile != null)
            {
                string dir = HttpContext.Current.Server.MapPath(ImageFactory.uploadFolder);
                string allPath = dir + "\\" + inputFile;

                FileInfo file = new FileInfo(allPath);
                if (file.Exists)
                {
                    File.Delete(allPath);
                }
            }
        }

        /// <summary>
        /// Get file path 
        /// </summary>
        /// <param name="inputFile">file name, stored in database</param>
        public static string GetFilePath(string file)
        {
            string pathToFile = ImageFactory.uploadFolder + "\\" + file;
            return pathToFile;
        }
    }
}