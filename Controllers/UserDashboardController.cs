using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Text;
using Traffic_Violation.Models;
using MimeKit;


namespace Traffic_Violation.Controllers
{
    public class UserDashboardController : Controller
	{
		private readonly ModelContext _context;
		private readonly IWebHostEnvironment _webHostEnviroment;

		public UserDashboardController(ModelContext context, IWebHostEnvironment webHostEnviroment)
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

			if (Userid != null)
			{
				var User = _context.ProjectUsers.FirstOrDefault(x => x.Userid == Userid);
				var userLicense = _context.ProjectLicenses.FirstOrDefault(x => x.Userid == Userid);
				var UserCars = _context.ProjectCars.Where(x => x.Userid == User.Userid).ToList();
				var UserViolations = _context.ProjectCars.Where(x => x.Userid == User.Userid).ToList();

				var result = ViolationJoinReturn().Where(x => x.violationState.Stateid == 2 && x.car.Userid == Userid);

				var tuple = Tuple.Create<ProjectUser, ProjectLicense, IEnumerable<ProjectCar>, IEnumerable<ViolationJoin>>(User, userLicense, UserCars, result);

				ViewBag.BirthYear = User.Birthdate.Value.Year;
				ViewBag.Age = DateTime.Now.Year - User.Birthdate.Value.Year;

				

				return View(tuple);
			}

			return RedirectToAction("Login", "LogReg");
		}

		public IActionResult Profile()
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


			return View(user);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Profile(decimal id, [Bind("Userid,Fname,Lname,Birthdate,Phonenumber,Email,ImgFile, Imgpath, Address")] ProjectUser projectUser)
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
					return RedirectToAction("Index", "UserDashboard");
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
			
			if(ModelState.IsValid)
            {
				if (ProjectUserExists(id))
				{
					if (projectUserLogin.Password == OldPassword)
					{
						projectUserLogin.Password = NewPassword;

						_context.Update(projectUserLogin);
						await _context.SaveChangesAsync();
						return RedirectToAction("Profile", "UserDashboard");
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
						return RedirectToAction("Profile", "UserDashboard");
					}
				}
			}


			return View(projectUserLogin);

		}

		public IActionResult CreateLicense()
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

			ViewBag.Userid = (int)HttpContext.Session.GetInt32("UserID");

			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateLicense(decimal id, [Bind("Licenseid, Issdate, Expdate, Userid")] ProjectLicense projectLicense)
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

			if (projectLicense.Issdate <= DateTime.Now && projectLicense.Expdate > projectLicense.Issdate)
			{
				_context.Add(projectLicense);
				await _context.SaveChangesAsync();

				return RedirectToAction("Index", "UserDashboard");

			}

			

			return View();
		}

		public IActionResult EditLicense(decimal id)
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

			var License = _context.ProjectLicenses.FirstOrDefault(x => x.Userid == Userid);

			if (License != null && id == License.Licenseid)
			{
				return View(License);
			}

			return RedirectToAction("Index", "UserDashboard");



		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> EditLicense(decimal id, [Bind("Licenseid, Issdate, Expdate, Userid")] ProjectLicense projectLicense)
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

			if (ModelState.IsValid && id == projectLicense.Licenseid)
			{
				if (projectLicense.Issdate <= DateTime.Now && projectLicense.Expdate > projectLicense.Issdate)
				{
					_context.Update(projectLicense);
					await _context.SaveChangesAsync();

					return RedirectToAction("Index", "UserDashboard");

				}

			}

			return View();
		}

		public IActionResult CarManagement()
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



			var UserCars = _context.ProjectCars.Where(x => x.Userid == Userid).ToList();
			return View(UserCars);

		}

		public IActionResult CreateCar()
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

			ViewBag.Userid = (int)HttpContext.Session.GetInt32("UserID");
			return View();

		}



		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateCar([Bind("Carid, Brand, Model, Color, Platenumber, Userid")] ProjectCar projectCar)
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
				if (Userid == projectCar.Userid)
				{
					bool plateCheck = _context.ProjectCars.Any(x => x.Platenumber == projectCar.Platenumber);

					if (!plateCheck)
                    {
						_context.Add(projectCar);
						await _context.SaveChangesAsync();

						return RedirectToAction("CarManagement", "UserDashboard");
                    }
					

				}

			}

			return View();

		}

		public IActionResult EditCar(decimal id)
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

			var Car = _context.ProjectCars.FirstOrDefault(x => x.Carid == id); 
			return View(Car);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> EditCar(decimal id, [Bind("Carid, Brand, Model, Color, Platenumber, Userid")] ProjectCar projectCar)
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
                if (id == projectCar.Carid)
                {
					bool plate = _context.ProjectCars.Any(x => x.Platenumber == projectCar.Platenumber && x.Carid != projectCar.Carid);

					if (!plate)
					{
						_context.Update(projectCar);
						await _context.SaveChangesAsync();

						return RedirectToAction("CarManagement", "UserDashboard");
					}
	
				}
				

			}

			return View();
		}

		public IActionResult DeleteCar(decimal id)
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

			var Car = _context.ProjectCars.FirstOrDefault(x => x.Carid == id);
			return View(Car);
		}
		
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(decimal Carid)
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

			var car = _context.ProjectCars.FirstOrDefault(x => x.Carid == Carid);

			if (car != null)
			{
				_context.ProjectCars.Remove(car);

				await _context.SaveChangesAsync();
				return RedirectToAction("CarManagement", "UserDashboard");
			}

			return RedirectToAction("DeleteCar", "UserDashboard");


		}


		public IActionResult ViolationManagement(DateTime? startDate, DateTime? endDate)
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

				var unPayed = result.Where(x => x.violation.Stateid == 2 && x.car.Userid == Userid);


				return View(unPayed);
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

				var unPayed = result.Where(x => x.violation.Stateid == 2 && x.car.Userid == Userid);


				return View(unPayed);

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

				var unPayed = result.Where(x => x.violation.Stateid == 2 && x.car.Userid == Userid);


				return View(unPayed);
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

				var unPayed = result.Where(x => x.violation.Stateid == 2 && x.car.Userid == Userid);


				return View(unPayed);

			}

		}

		public IActionResult PayViolation(decimal id)
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

			if (ProjectViolationExists(id))
			{
				var violation = _context.ProjectViolations.FirstOrDefault(x => x.Violationid == id);
				ViewBag.Fine = _context.ProjectViolationTypes.FirstOrDefault(x => x.Violationtypeid == violation.Violationtypeid).Fine;
				return View(violation);
			}

			return RedirectToAction("ViolationManagement", "UserDashboard");


		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> PayViolation(decimal id, [Bind("Violationid, Violationdate, Location, Violationtypeid, Stateid, Carid")] ProjectViolation projectViolation)
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
				if (ProjectViolationExists(id))
				{
					projectViolation.Stateid = 1;

					_context.ProjectViolations.Update(projectViolation);
					await _context.SaveChangesAsync();

					var fullViolation = ViolationJoinReturn().FirstOrDefault(x => x.violation.Violationid == id);
					var mailKitController = new MailKitControllercs(_context);
					IActionResult emailResult = mailKitController.SendlInvoice(fullViolation);


				}

			}

			return RedirectToAction("ViolationManagement", "UserDashboard");


		}

		[HttpPost]
		public IActionResult PayViolationMultiple(List<decimal> selectedViolations)
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

			decimal fineTotal = 0;
			var violations = new List<ProjectViolation>();
			foreach(var id in selectedViolations)
            {
				if (ProjectViolationExists(id))
				{
					violations.Add(_context.ProjectViolations.FirstOrDefault(x => x.Violationid == id));
					decimal typeId = violations.Last().Violationtypeid;
					fineTotal = fineTotal + _context.ProjectViolationTypes.FirstOrDefault(x => x.Violationtypeid == typeId).Fine;
				}

			}

			if(violations.Count() > 0)
            {
				ViewBag.Fine = fineTotal;
				return View(violations);
			}
			

			return RedirectToAction("ViolationManagement", "UserDashboard");

		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> PayViolationMultiples([Bind("Violationid, Violationdate, Location, Violationtypeid, Stateid, Carid")] List<ProjectViolation> projectViolations)
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
				var fullViolation = new List<ViolationJoin>();
				for (int i = 0; i < projectViolations.Count(); i++)
                {
					if (ProjectViolationExists(projectViolations[i].Violationid))
					{
                        Console.WriteLine(projectViolations[i].Violationid);

						projectViolations[i].Stateid = 1;

					}

				}
				_context.ProjectViolations.UpdateRange(projectViolations);
				await _context.SaveChangesAsync();

				var mailKitController = new MailKitControllercs(_context);
				IActionResult emailResult = mailKitController.SendlInvoiceMultiple(projectViolations, Userid);

				return RedirectToAction("ViolationManagement", "UserDashboard");

			}

			return RedirectToAction("PayViolationMultiple", "UserDashboard");


		}

		public IActionResult PayedViolations()
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


			var violations = _context.ProjectViolations.ToList();
			var violationTypes = _context.ProjectViolationTypes.ToList();
			var violaionStates = _context.ProjectViolationStates.ToList();
			var cars = _context.ProjectCars.ToList();



			var result = ViolationJoinReturn();

			var payed = result.Where(x => x.violation.Stateid == 1 && x.car.Userid == Userid);
			return View(payed);
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
			var testimonialStates = _context.ProjectTestimonialStates;


			var result = from t in testimonls
						 join ts in testimonialStates on t.Stateid equals ts.Stateid
					     select new TestimonialJoin { testimonial = t, testimonialState= ts};

			var userTestimoials = result.Where(x => x.testimonial.Userid == Userid && x.testimonial.Stateid == 1);
			return View(userTestimoials);
		}

		public IActionResult PastTestimonials()
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
			var testimonialStates = _context.ProjectTestimonialStates;


			var result = from t in testimonls
						 join ts in testimonialStates on t.Stateid equals ts.Stateid
						 select new TestimonialJoin { testimonial = t, testimonialState = ts };

			var userTestimoials = result.Where(x => x.testimonial.Userid == Userid && x.testimonial.Stateid != 1);
			return View(userTestimoials);
		}



		public IActionResult CreateTestimonial()
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

			ViewBag.Userid = Userid;
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateTestimonial([Bind("Testimonialid, Content, Dateadded, Stateid, Userid")] ProjectTestimonial projectTestimonial)
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

            
           
				
			_context.ProjectTestimonials.Add(projectTestimonial);
			await _context.SaveChangesAsync();
			return RedirectToAction("TestimonialManagement", "UserDashboard");
				
			
			

			return View(projectTestimonial);
		}

		public IActionResult EditTestimonial(decimal id)
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

			return RedirectToAction("TestimonialManagement", "UserDashboard");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> EditTestimonial(decimal id,[Bind("Testimonialid, Content, Dateadded, Stateid, Userid")] ProjectTestimonial projectTestimonial)
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
				_context.ProjectTestimonials.Update(projectTestimonial);
				await _context.SaveChangesAsync();
				return RedirectToAction("TestimonialManagement", "UserDashboard");
			}

			return RedirectToAction("TestimonialManagement", "UserDashboard");
		}

		[HttpGet]
		public IActionResult DeleteTestimonial(decimal id)
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

			return RedirectToAction("TestimonialManagement", "UserDashboard");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> TestimonialDeleted(decimal Testimonialid)
		{


			if (ProjectTestimonialExists(Testimonialid))
			{
				var testimonial = _context.ProjectTestimonials.FirstOrDefault(x => x.Testimonialid == Testimonialid);

				_context.ProjectTestimonials.Remove(testimonial);

				await _context.SaveChangesAsync();
				return RedirectToAction("TestimonialManagement", "UserDashboard");
			}

			return RedirectToAction("DeleteTestimonial", "UserDashboard");


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
