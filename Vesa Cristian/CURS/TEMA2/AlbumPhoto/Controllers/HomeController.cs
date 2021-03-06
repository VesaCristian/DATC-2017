﻿using AlbumPhoto.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace AlbumPhoto.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            var service = new AlbumFotoService();
            return View(service.GetPoze());
        }

        [HttpPost]
        public ActionResult IncarcaPoza(HttpPostedFileBase file)
        {
            var service = new AlbumFotoService();
            if (file != null && file.ContentLength > 0)
            {
                service.IncarcaPoza("guest", file.FileName, file.InputStream);
            }

            return View("Index", service.GetPoze());
        }

        [HttpPost]
        public ActionResult GetComment()
        {
            var service = new AlbumFotoService();
            var poza = Request["Picture"].ToString();
            return View("Comentariu", service.GetComments(poza));
        }

        [HttpPost]
        public ActionResult GetLink()
        {
            var service = new AlbumFotoService();
            var poza = Request["Picture"].ToString();
            return View("Link", service.GetLink(poza));
        }

        [HttpPost]
        public ActionResult AddComment()
        {
            var service = new AlbumFotoService();
            var by = Request["By"].ToString();
            var poza = Request["Picture"].ToString();
            var length = Request["Comment"].ToString().Length;
            var comment = Request["Comment"].ToString();

            if (length > 0 && comment != null)
            {
                comment = poza + "#%#" + comment;
                MemoryStream stream = new MemoryStream();
                StreamWriter writer = new StreamWriter(stream);
                writer.Write(by + ": " + comment);
                writer.Flush();
                stream.Position = 0;
                service.UploadComment("guest", comment, by, stream);
            }
            return View("Index", service.GetPoze());
        }
    }
}
