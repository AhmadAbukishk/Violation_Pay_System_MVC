using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Traffic_Violation.Models;
using System.Net;
using System.Net.Mail;


namespace Traffic_Violation.Controllers
{
    public class AdminDashboardController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnviroment;

        public AdminDashboardController(ModelContext context, IWebHostEnvironment webHostEnviroment)
        {
            this._context = context;
            this._webHostEnviroment = webHostEnviroment;
        }

        public IActionResult Index()
        {

            int? Userid;
            try
            {
                Userid = (int)HttpContext.Session.GetInt32("UserID");

            }
            catch
            {
                return RedirectToAction("Login", "LogReg");
            }

            var user = _context.ProjectUsers.First(x => x.Userid == Userid);
            var violations = _context.ProjectViolations.ToList();
            var testimonls = _context.ProjectTestimonials;
            var users = _context.ProjectUsers;


            var testimonialsUsers = from t in testimonls
                                      join u in users on t.Userid equals u.Userid
                                      select new TestimonialJserJoin { testimonial = t, user = u };

            var pendingTestimonials = testimonialsUsers.Where(x => x.testimonial.Stateid == 1);

            var violationLocations = violations.GroupBy(x => x.Location).Select(x => new LocationCount
            {
                location = x.Key,
                count = x.Count(),
                percentage = (int)(((double)x.Count() / violations.Count()) * 100)
            })
            .OrderByDescending(x => x.count)
            .ToList();

            

            ViewBag.Name = user.Fname;
            ViewBag.BirthYear = user.Birthdate.Value.Year;
            ViewBag.Age = DateTime.Now.Year - user.Birthdate.Value.Year;
            ViewBag.UserCount = _context.ProjectUsers.Count();
            ViewBag.ViolationCount = _context.ProjectViolations.Count();
            ViewBag.FineSum = _context.ProjectViolations.Sum(x => x.Violationtype.Fine);
            ViewBag.PayedPer = MathF.Round((_context.ProjectViolations.Count(x => x.Stateid == 1) / (float) _context.ProjectViolations.Count()) * 100);

            var tuple = Tuple.Create<ProjectUser, IEnumerable<LocationCount>, IEnumerable<TestimonialJserJoin>>(user, violationLocations, pendingTestimonials);
            return View(tuple);
        }

        public IActionResult Profile()
        {
            int UserID = (int)HttpContext.Session.GetInt32("UserID");

            var user = _context.ProjectUsers.First(x => x.Userid == UserID);


            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(decimal Userid, [Bind("Userid,Fname,Lname,Birthdate,Phonenumber,Email,ImgFile, Imgpath, Address")] ProjectUser projectUser)
        {

            if (Userid != projectUser.Userid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    bool emailCheck = _context.ProjectUsers.Any(x => x.Email == projectUser.Email && x.Userid != projectUser.Userid);
                    
                    if (emailCheck)
                    {
                        return View(projectUser);
                    }

                    if (projectUser.ImgFile != null)
                    {

                        string wwwRootPath = _webHostEnviroment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + "_" + projectUser.ImgFile.FileName;
                        string path = Path.Combine(wwwRootPath + "/Imgs/UserImgs", fileName);

                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {

                            await projectUser.ImgFile.CopyToAsync(fileStream);

                        }

                        projectUser.Imgpath = fileName;


                    }

                    _context.Update(projectUser);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "AdminDashboard");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectUserExists(projectUser.Userid))
                    {
                        return View(projectUser);
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(projectUser);
        }

        public IActionResult ChangePassword()
        {
            int? Userid;
            try
            {
                Userid = (int)HttpContext.Session.GetInt32("UserID");

            }
            catch
            {
                return RedirectToAction("Login", "LogReg");
            }

            var userLogin = _context.ProjectUserLogins.First(x => x.Userid == Userid);


            return View(userLogin);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(decimal id, [Bind("Loginid, Username, Password, Userid, Roleid")] ProjectUserLogin projectUserLogin, string OldPassword, string NewPassword)
        {
            int? Userid;
            try
            {
                Userid = (int)HttpContext.Session.GetInt32("UserID");

            }
            catch
            {
                return RedirectToAction("Login", "LogReg");
            }

            if (ModelState.IsValid)
            {
                if (ProjectUserExists(id))
                {
                    if (projectUserLogin.Password == OldPassword)
                    {
                        projectUserLogin.Password = NewPassword;

                        _context.Update(projectUserLogin);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Profile", "AdminDashboard");
                    }
                }
            }



            return View(projectUserLogin);
        }

        public IActionResult ChangeUsername()
        {
            int? Userid;
            try
            {
                Userid = (int)HttpContext.Session.GetInt32("UserID");

            }
            catch
            {
                return RedirectToAction("Login", "LogReg");
            }



            var userLogin = _context.ProjectUserLogins.First(x => x.Userid == Userid);


            return View(userLogin);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUsername(decimal id, [Bind("Loginid, Username, Password, Userid, Roleid")] ProjectUserLogin projectUserLogin)
        {
            int? Userid;
            try
            {
                Userid = (int)HttpContext.Session.GetInt32("UserID");

            }
            catch
            {
                return RedirectToAction("Login", "LogReg");
            }

            if (ModelState.IsValid)
            {
                if (ProjectUserExists(id))
                {
                    if (!_context.ProjectUserLogins.Any(x => x.Username == projectUserLogin.Username))
                    {
                        _context.Update(projectUserLogin);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Profile", "AdminDashboard");
                    }
                }
            }


            return View(projectUserLogin);

        }

        public IActionResult ViolationManagement(DateTime? startDate, DateTime? endDate)
        {


            if (startDate == null && endDate == null)
            {
                var violations = _context.ProjectViolations.ToList();

                var violationTypes = _context.ProjectViolationTypes.ToList();
                var violaionStates = _context.ProjectViolationStates.ToList();
                var cars = _context.ProjectCars.ToList();



                var result = from v in violations
                             join vt in violationTypes on v.Violationtypeid equals vt.Violationtypeid
                             join vs in violaionStates on v.Stateid equals vs.Stateid
                             join c in cars on v.Carid equals c.Carid
                             select new ViolationJoin { car = c, violation = v, violationType = vt, violationState = vs };

                return View(result);
            }
            else if (startDate == null && endDate != null)
            {
                var violations = _context.ProjectViolations.Where(x => x.Violationdate <= endDate).ToList();

                var violationTypes = _context.ProjectViolationTypes.ToList();
                var violaionStates = _context.ProjectViolationStates.ToList();
                var cars = _context.ProjectCars.ToList();



                var result = from v in violations
                             join vt in violationTypes on v.Violationtypeid equals vt.Violationtypeid
                             join vs in violaionStates on v.Stateid equals vs.Stateid
                             join c in cars on v.Carid equals c.Carid
                             select new ViolationJoin { car = c, violation = v, violationType = vt, violationState = vs };

                return View(result);

            }
            else if (startDate != null && endDate == null)
            {
                var violations = _context.ProjectViolations.Where(x => x.Violationdate >= startDate).ToList();

                var violationTypes = _context.ProjectViolationTypes.ToList();
                var violaionStates = _context.ProjectViolationStates.ToList();
                var cars = _context.ProjectCars.ToList();



                var result = from v in violations
                             join vt in violationTypes on v.Violationtypeid equals vt.Violationtypeid
                             join vs in violaionStates on v.Stateid equals vs.Stateid
                             join c in cars on v.Carid equals c.Carid
                             select new ViolationJoin { car = c, violation = v, violationType = vt, violationState = vs };

                return View(result);
            }
            else
            {
                var violations = _context.ProjectViolations.Where(x => x.Violationdate <= endDate && x.Violationdate >= startDate).ToList();

                var violationTypes = _context.ProjectViolationTypes.ToList();
                var violaionStates = _context.ProjectViolationStates.ToList();
                var cars = _context.ProjectCars.ToList();



                var result = from v in violations
                             join vt in violationTypes on v.Violationtypeid equals vt.Violationtypeid
                             join vs in violaionStates on v.Stateid equals vs.Stateid
                             join c in cars on v.Carid equals c.Carid
                             select new ViolationJoin { car = c, violation = v, violationType = vt, violationState = vs };

                return View(result);

            }
            
        }


        public IActionResult CreateViolation()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateViolation([Bind("Violationid, Violationdate, Location")] ProjectViolation projectViolation, string ViolationType, int CarNumber)
        {
            
            if (ModelState.IsValid)
            {

                var chosenCar = _context.ProjectCars.FirstOrDefault(x => (int) x.Platenumber == CarNumber);
                var chosenType = _context.ProjectViolationTypes.FirstOrDefault(x => x.Name == ViolationType);


                if (chosenCar != null && chosenType != null)
                {
                    projectViolation.Violationtypeid = chosenType.Violationtypeid;
                    projectViolation.Stateid = 2;
                    projectViolation.Carid = chosenCar.Carid;

                    _context.Add(projectViolation);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("ViolationManagement", "AdminDashboard");
                }

                
            }

      

            return View();
        }

        public IActionResult EditViolation(decimal id)
        {
            if(ProjectViolationExists(id))
            {
                var violation = _context.ProjectViolations.SingleOrDefault(x => x.Violationid == id);
                var car = _context.ProjectCars.SingleOrDefault(x => x.Carid == violation.Carid);
                var violationType = _context.ProjectViolationTypes.SingleOrDefault(x => x.Violationtypeid == violation.Violationtypeid);
                var violationState = _context.ProjectViolationStates.SingleOrDefault(x => x.Stateid == violation.Stateid);

                ViewBag.Plate = car.Platenumber;
                ViewBag.Stateid = violationState.Stateid;
    

                if (violation != null || car != null || violationType != null)
                {
                    return View(violation);
                }
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditViolation(decimal id, [Bind("Violationid, Violationdate, Location")] ProjectViolation projectViolation, string ViolationType, int CarNumber, decimal Stateid)
          {

            if (ModelState.IsValid && id == projectViolation.Violationid)
            {
                var chosenCar = _context.ProjectCars.FirstOrDefault(x => (int)x.Platenumber == CarNumber);
                var chosenType = _context.ProjectViolationTypes.FirstOrDefault(x => x.Name == ViolationType);

                if (chosenCar != null || chosenType != null)
                {
                    projectViolation.Violationtypeid = chosenType.Violationtypeid;
                    projectViolation.Carid = chosenCar.Carid;
                    projectViolation.Stateid = Stateid;

                    _context.Update(projectViolation);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("ViolationManagement", "AdminDashboard");
                }

            }
            
            return View();


        }

        public IActionResult DeleteViolation(decimal id)
        {
            var violation =  _context.ProjectViolations.FirstOrDefault(x => x.Violationid == id);

            if(violation != null)
            {
                return View(violation);
            }

            return RedirectToAction("ViolationManagement", "AdminDashboard");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal Violationid)
        {

            var violation = _context.ProjectViolations.FirstOrDefault(x => x.Violationid == Violationid);

            if (violation != null)
            {
                _context.ProjectViolations.Remove(violation);

                await _context.SaveChangesAsync();
                return RedirectToAction("ViolationManagement", "AdminDashboard");
            }
          
            return RedirectToAction("DeleteViolation", "AdminDashboard");
            

        }

        public IActionResult ManagePageHeader()
        {
            var Headers = _context.ProjectHeaderSections.ToList();          
            return View(Headers);

        }

        
        public IActionResult EditHeader(decimal id)
        {
            if (ProjectHeaderExist(id))
            {
                var Header = _context.ProjectHeaderSections.FirstOrDefault(x => x.Headerid == id);
                return View(Header);
            }

            return RedirectToAction("ManagePageHeader", "AdminDashboard");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditHeader(decimal id, [Bind("Headerid, Imgfile, Imgpath, Title, Subtitle")] ProjectHeaderSection projectHeader)
        {
            if (ModelState.IsValid)
            {
                if (ProjectHeaderExist(id))
                {
                    if (projectHeader.Imgfile != null)
                    {

                        string wwwRootPath = _webHostEnviroment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + "_" + projectHeader.Imgfile.FileName;
                        string path = Path.Combine(wwwRootPath + "/Imgs/HeaderImgs", fileName);

                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {

                            await projectHeader.Imgfile.CopyToAsync(fileStream);

                        }

                        projectHeader.Imgpath = fileName;


                    }

                    _context.Update(projectHeader);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("ManagePageHeader", "AdminDashboard");
                }

            }

            return View(projectHeader);

        }

        public IActionResult EditContact()
        {
            if (ProjectHeaderExist(1))
            {
                var Contact = _context.ProjectContactSections.FirstOrDefault(x => x.Contactid == 1);
                return View(Contact);
            }

            return RedirectToAction("Index", "AdminDashboard");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditContact([Bind("Contactid, Title, Content, Email, Phone, Address")] ProjectContactSection projectContact)
        {
            if (ModelState.IsValid)
            {
                if (ProjectContactExist(1))
                {
                    _context.Update(projectContact);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("EditContact", "AdminDashboard");
                }

            }

            return View(projectContact);

        }
        
        
        public IActionResult EditAboutus()
        {
            if (ProjectAboutExist(1))
            {
                var About = _context.ProjectAboutSections.FirstOrDefault(x => x.Aboutid == 1);
                return View(About);
            }

            return RedirectToAction("Index", "AdminDashboard");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAboutus([Bind("Aboutid, Imgfile, Imgpath, Header, Subheader")] ProjectAboutSection projectAbout)
        {
            if (ModelState.IsValid)
            {
                if (ProjectAboutExist(1))
                {
                    if (projectAbout.Imgfile != null)
                    {

                        string wwwRootPath = _webHostEnviroment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + "_" + projectAbout.Imgfile.FileName;
                        string path = Path.Combine(wwwRootPath + "/Imgs/AboutImgs", fileName);

                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {

                            await projectAbout.Imgfile.CopyToAsync(fileStream);

                        }

                        projectAbout.Imgpath = fileName;


                    }
                    _context.Update(projectAbout);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("EditAboutus", "AdminDashboard");
                }

            }

            return View(projectAbout);

        }

       
        public IActionResult ManagePageServices()
        {
            var Services = _context.ProjectServiceSections.ToList();
            return View(Services);

        }

        public IActionResult EditService(decimal id)
        {
            if (ProjectServiceExist(id))
            {
                var Service = _context.ProjectServiceSections.FirstOrDefault(x => x.Serviceid == id);
                return View(Service);
            }

            return RedirectToAction("ManagePageServices", "AdminDashboard");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditService(decimal id, [Bind("Serviceid, Imgfile, Imgpath, Title, Description")] ProjectServiceSection projectService)
        {
            if (ModelState.IsValid)
            {
                if (ProjectServiceExist(id))
                {
                    if (projectService.Imgfile != null)
                    {

                        string wwwRootPath = _webHostEnviroment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + "_" + projectService.Imgfile.FileName;
                        string path = Path.Combine(wwwRootPath + "/Imgs/ServiceImgs", fileName);

                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {

                            await projectService.Imgfile.CopyToAsync(fileStream);

                        }

                        projectService.Imgpath = fileName;


                    }

                    _context.Update(projectService);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("ManagePageServices", "AdminDashboard");
                }

            }

            return View(projectService);

        }

        public IActionResult EditFact()
        {
            if (ProjectFactExist(1))
            {
                var Fact = _context.ProjectFactSections.FirstOrDefault(x => x.Factid == 1);
                return View(Fact);
            }

            return RedirectToAction("Index", "AdminDashboard");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditFact([Bind("Factid, Title, Content")] ProjectFactSection projectFact)
        {
            if (ModelState.IsValid)
            {
                if (ProjectFactExist(1))
                {
                    _context.Update(projectFact);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("EditFact", "AdminDashboard");
                }

            }

            return View(projectFact);

        }

        public IActionResult TestimonialManagement()
        {
            int? Userid;
            try
            {
                Userid = (int)HttpContext.Session.GetInt32("UserID");

            }
            catch
            {
                return RedirectToAction("Login", "LogReg");
            }

            var testimonls = _context.ProjectTestimonials;
            var users = _context.ProjectUsers;


            var testimonialsUsers = from t in testimonls
                                    join u in users on t.Userid equals u.Userid
                                    select new TestimonialJserJoin { testimonial = t, user = u };

            var pendingTestimonials = testimonialsUsers.Where(x => x.testimonial.Stateid == 1);

            return View(pendingTestimonials);
        }



        public IActionResult AcceptTestimonial(decimal id)
        {
            int? Userid;
            try
            {
                Userid = (int)HttpContext.Session.GetInt32("UserID");

            }
            catch
            {
                return RedirectToAction("Login", "LogReg");
            }


            if (ProjectTestimonialExists(id))
            {
                var testimonial = _context.ProjectTestimonials.FirstOrDefault(x => x.Testimonialid == id);
                return View(testimonial);
            }


            return RedirectToAction("TestimonialManagement", "AdminDashboard");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AcceptTestimonial(decimal id, [Bind("Testimonialid, Content, Dateadded, Stateid, Userid")] ProjectTestimonial projectTestimonial)
        {
            int? Userid;
            try
            {
                Userid = (int)HttpContext.Session.GetInt32("UserID");

            }
            catch
            {
                return RedirectToAction("Login", "LogReg");
            }

            if (ModelState.IsValid)
            {
                if (ProjectTestimonialExists(id))
                {
                    var testimonial = _context.ProjectTestimonials.FirstOrDefault(x => x.Testimonialid == id);
                    testimonial.Stateid = 2;
                    
                    _context.Update(testimonial);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("TestimonialManagement", "AdminDashboard");
                }

            }

            return View();

        }

        public IActionResult RejectTestimonial(decimal id)
        {
            int? Userid;
            try
            {
                Userid = (int)HttpContext.Session.GetInt32("UserID");

            }
            catch
            {
                return RedirectToAction("Login", "LogReg");
            }


            if (ProjectTestimonialExists(id))
            {
                var testimonial = _context.ProjectTestimonials.FirstOrDefault(x => x.Testimonialid == id);
                return View(testimonial);
            }


            return RedirectToAction("TestimonialManagement", "AdminDashboard");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectTestimonial(decimal id, [Bind("Testimonialid, Content, Dateadded, Stateid, Userid")] ProjectTestimonial projectTestimonial)
        { 
            int? Userid;
            try
            {
                Userid = (int)HttpContext.Session.GetInt32("UserID");

            }
            catch
            {
                return RedirectToAction("Login", "LogReg");
            }

            if (ModelState.IsValid)
            {
                if (ProjectTestimonialExists(id))
                {
                    var testimonial = _context.ProjectTestimonials.FirstOrDefault(x => x.Testimonialid == id);
                    testimonial.Stateid = 3;

                    _context.Update(testimonial);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("TestimonialManagement", "AdminDashboard");
                }

            }

            return View();

        }

        public IActionResult Report(int month)
        {
            int? Userid;
            try
            {
                Userid = (int)HttpContext.Session.GetInt32("UserID");

            }
            catch
            {
                return RedirectToAction("Login", "LogReg");
            }

            if (month == 0)
            {
                month = DateTime.Now.Month;
            }


            var violations = ViolationJoinReturn().Where(x => x.violation.Violationdate.Month == month && x.violation.Violationdate.Year == DateTime.Now.Year).ToList();

            var violationLocations = violations.GroupBy(x => x.violation.Location).Select(x => new LocationCount
            {
                location = x.Key,
                count = x.Count(),
                percentage = MathF.Round(((float) x.Count() / violations.Count()) * 100)
            }) 
            .ToList();


            ViewBag.Month = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(month);
            ViewBag.violationCount = violations.Count();
            ViewBag.violationSum = violations.Sum(x => x.violationType.Fine);
            ViewBag.finesPayed = ((double)violations.Where(x => x.violation.Stateid == 1).Count() / violations.Count()) * 100;
            
            var tuple = Tuple.Create<IEnumerable<LocationCount>, int, IEnumerable<ViolationJoin>>(violationLocations, month, violations);
            return View(tuple);
        }

        public IActionResult ReportYear(int year)
        {
            int? Userid;
            try
            {
                Userid = (int)HttpContext.Session.GetInt32("UserID");

            }
            catch
            {
                return RedirectToAction("Login", "LogReg");
            }

            if (year == 0)
            {
                year = DateTime.Now.Year;
            }

          

            var violations = ViolationJoinReturn().Where(x => x.violation.Violationdate.Year == year).ToList();

            var violationLocations = violations.GroupBy(x => x.violation.Location).Select(x => new LocationCount
            {
                location = x.Key,
                count = x.Count(),
                percentage = (int)(((double)x.Count() / violations.Count()) * 100)
            })
            .OrderByDescending(x => x.count)
            .ToList();


            ViewBag.Year = year;
            ViewBag.violationCount = violations.Count();
            ViewBag.violationSum = violations.Sum(x => x.violationType.Fine);
            ViewBag.finesPayed = MathF.Round(((float)violations.Where(x => x.violation.Stateid == 1).Count() / violations.Count()) * 100);

            var tuple = Tuple.Create<IEnumerable<LocationCount>, int, IEnumerable<ViolationJoin>>(violationLocations, year, violations);
            return View(tuple);
        }

        public IActionResult UsersView()
        {
            int? Userid;
            try
            {
                Userid = (int)HttpContext.Session.GetInt32("UserID");

            }
            catch
            {
                return RedirectToAction("Login", "LogReg");
            }

            var users = _context.ProjectUsers.ToList();
            var usersLogin = _context.ProjectUserLogins.ToList();


            var userFullInfo = from u in users
                               join ul in usersLogin on u.Userid equals ul.Userid
                               select new UserJoin { user = u, userLogin = ul};
            
            return View(userFullInfo);

        }

        public IActionResult CarsView(decimal id)
        {
            int? Userid;
            try
            {
                Userid = (int)HttpContext.Session.GetInt32("UserID");

            }
            catch
            {
                return RedirectToAction("Login", "LogReg");
            }

            if (ProjectUserExists(id))
            {
                ViewBag.username = _context.ProjectUsers.FirstOrDefault(x => x.Userid == id).Fname;
                var UserCars = _context.ProjectCars.Where(x => x.Userid == id).ToList();
                return View(UserCars);
            }

            return RedirectToAction("UsersView", "AdminDashboard");
            

        }

        public IActionResult userViolations(decimal id)
        {
            int? Userid;
            try
            {
                Userid = (int)HttpContext.Session.GetInt32("UserID");

            }
            catch
            {
                return RedirectToAction("Login", "LogReg");
            }

            if (ProjectCarExists(id))
            {
                var UserViolations= ViolationJoinReturn().Where(x => x.violation.Carid == id);
                return View(UserViolations);
            }

            return RedirectToAction("CarsView", "AdminDashboard");


        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }

        private bool ProjectUserExists(decimal Userid)
        {
            return (_context.ProjectUsers?.Any(e => e.Userid == Userid)).GetValueOrDefault();
        }

        private bool ProjectViolationExists(decimal Violationid)
        {
            return (_context.ProjectViolations?.Any(e => e.Violationid == Violationid)).GetValueOrDefault();
        }

        private bool ProjectHeaderExist(decimal Headerid)
        {
            return (_context.ProjectHeaderSections?.Any(e => e.Headerid == Headerid)).GetValueOrDefault();
        }

        private bool ProjectContactExist(decimal Contactid)
        {
            return (_context.ProjectContactSections?.Any(e => e.Contactid == Contactid)).GetValueOrDefault();
        }

        private bool ProjectAboutExist(decimal Aboutid)
        {
            return (_context.ProjectAboutSections?.Any(e => e.Aboutid == Aboutid)).GetValueOrDefault();
        }

        private bool ProjectServiceExist(decimal Serviceid)
        {
            return (_context.ProjectServiceSections?.Any(e => e.Serviceid == Serviceid)).GetValueOrDefault();
        }

        private bool ProjectFactExist(decimal Serviceid)
        {
            return (_context.ProjectServiceSections?.Any(e => e.Serviceid == Serviceid)).GetValueOrDefault();
        }

        private bool ProjectTestimonialExists(decimal Testimonialid)
        {
            return (_context.ProjectTestimonials?.Any(e => e.Testimonialid == Testimonialid)).GetValueOrDefault();
        }
        private bool ProjectCarExists(decimal Carid)
        {
            return (_context.ProjectCars?.Any(e => e.Carid == Carid)).GetValueOrDefault();
        }

        private IEnumerable<ViolationJoin> ViolationJoinReturn()
        {
            var violations = _context.ProjectViolations.ToList();
            var violationTypes = _context.ProjectViolationTypes.ToList();
            var violaionStates = _context.ProjectViolationStates.ToList();
            var cars = _context.ProjectCars.ToList();



            var result = from v in violations
                         join vt in violationTypes on v.Violationtypeid equals vt.Violationtypeid
                         join vs in violaionStates on v.Stateid equals vs.Stateid
                         join c in cars on v.Carid equals c.Carid
                         select new ViolationJoin { car = c, violation = v, violationType = vt, violationState = vs };

            return result;
        }


    }
}
