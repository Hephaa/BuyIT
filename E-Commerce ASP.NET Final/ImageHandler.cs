using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace E_Commerce_ASP.NET_Final
{
    public class ImageHandler
    {
        private static string path = HostingEnvironment.MapPath("~/Content/Images/Products");


        public static void SaveImage(Bitmap bmp, int id)
        {
            //Check and create image directory
            if(!Directory.Exists(path))
                Directory.CreateDirectory(path);

            bmp.Save(path + "\\" + id + ".jpg");
        }
        
    }
}