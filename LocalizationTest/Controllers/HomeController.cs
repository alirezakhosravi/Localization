using System.Diagnostics;
using Localization.Core;
using LocalizationTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace LocalizationTest.Controllers
{
    public class HomeController : Controller
    {
        //private readonly Context _context;
        private readonly IStringLocalizerFactory _factory;
        private readonly ILocalizerCroud _croud;
        protected string LocalizationResourceName { get; set; } = nameof(LocalizationResourceNames.SharedResource);
        protected string LocalizationResourceLocation { get; set; }

        public HomeController(
            //Context context, 
            IStringLocalizerFactory factory,
            ILocalizerCroud croud
            )
        {
            //_context = context;
            _factory = factory;
            _croud = croud;
        }

        public IActionResult Index()
        {
            //var blogs = _context.Blogs.ToList();
            IStringLocalizer str = _factory.Create(LocalizationResourceName, LocalizationResourceLocation);

            string aaa = str.GetString("Test");

            _croud.Insert("test01", "alireza1", "", LocalizationResourceName);
            aaa = str.GetString("test01");

            _croud.Insert("test02", "alireza2", "", LocalizationResourceName);
            aaa = str.GetString("test01");

            _croud.Insert("test03", "alireza3", "", LocalizationResourceName);
            aaa = str.GetString("test01");

            _croud.Insert("test04", "alireza4", "", LocalizationResourceName);
            aaa = str.GetString("test01");

            _croud.Update("test01", "khosravi", "", LocalizationResourceName);
            aaa = str.GetString("test01");

            _croud.Delete("test01", "", LocalizationResourceName);
            aaa = str.GetString("test01");
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
