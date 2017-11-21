using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebsiteIntegration.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using WebsiteIntegration.Integration;

namespace WebsiteIntegration.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHostingEnvironment hostingEnvironment;

        public HomeController(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View(new HomeViewModel());
        }

        [HttpPost]
        public IActionResult Index(HomeViewModel model)
        {
            try
            {
                IFileInfo fileInfo = hostingEnvironment.ContentRootFileProvider.GetFileInfo(@"TestFile\bulksign_test_sample.pdf");

                string url = new BulksignIntegration().SendDocumentForSigning(model.Name, model.Email, fileInfo.PhysicalPath);

                model.Url = url;

                return View(model);
            }
            catch (Exception)
            {
                //SendBundle failed
                return RedirectToAction("Index");

            }
        }
    }
}
