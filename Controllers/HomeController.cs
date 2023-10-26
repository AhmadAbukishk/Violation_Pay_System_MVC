using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Traffic_Violation.Models;

namespace Traffic_Violation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnviroment;

        public HomeController(ModelContext context, IWebHostEnvironment webHostEnviroment, ILogger<HomeController> logger)
        {
            this._context = context;
            this._webHostEnviroment = webHostEnviroment;
            _logger = logger;

        }

        public IActionResult Index()
        {
            var header = _context.ProjectHeaderSections.ToList();
            var about = _context.ProjectAboutSections.FirstOrDefault(x => x.Aboutid == 1);
            var fact = _context.ProjectFactSections.FirstOrDefault(x => x.Factid == 1);
            var service = _context.ProjectServiceSections.ToList();
            var contact = _context.ProjectContactSections.FirstOrDefault(x => x.Contactid == 1);
            var testimonial = _context.ProjectTestimonials.Where(x => x.Stateid == 2).OrderByDescending(x => x.Dateadded).ToList();
            var user = _context.ProjectUsers.ToList();

            var testimonialJserJoin = from t in testimonial
                         join u in user on t.Userid equals u.Userid
                         select new TestimonialJserJoin { testimonial = t, user = u };

            var tuple = Tuple.Create<IEnumerable<ProjectHeaderSection>, ProjectAboutSection, ProjectFactSection, IEnumerable<ProjectServiceSection>, ProjectContactSection, IEnumerable<TestimonialJserJoin>>(header, about, fact, service, contact, testimonialJserJoin);

            ViewBag.Clients = _context.ProjectUsers.Count();
            ViewBag.Testimonials = _context.ProjectTestimonials.Count();
            ViewBag.Violations = _context.ProjectViolations.Count();

            return View(tuple);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string name, string userEmail, string subject, string message)
        {
            var mailKitController = new MailKitControllercs(_context);
            IActionResult emailResult = mailKitController.SendEmail(name, userEmail, subject, message);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult AboutUs()
        {

            var about = _context.ProjectAboutSections.FirstOrDefault(x => x.Aboutid == 1);
            var fact = _context.ProjectFactSections.FirstOrDefault(x => x.Factid == 1);

            ViewBag.Clients = _context.ProjectUsers.Count();
            ViewBag.Testimonials = _context.ProjectTestimonials.Count();
            ViewBag.Violations = _context.ProjectViolations.Count();

            var tuple = Tuple.Create<ProjectAboutSection, ProjectFactSection>(about, fact);

            return View(tuple);
        }

        public IActionResult Services()
        {
          
            var service = _context.ProjectServiceSections.ToList();
                 

            return View(service);
        }

        public IActionResult Contact()
        {

            var contact = _context.ProjectContactSections.FirstOrDefault(x => x.Contactid == 1);
            return View(contact);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(string name, string userEmail, string subject, string message)
        {
            var mailKitController = new MailKitControllercs(_context);
            IActionResult emailResult = mailKitController.SendEmail(name, userEmail, subject, message);

            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}