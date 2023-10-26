using Microsoft.AspNetCore.Mvc;
using Traffic_Violation.Models;

namespace Traffic_Violation.Controllers
{
    public class LogRegController : Controller
    {


        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnviroment;

        public LogRegController(ModelContext context, IWebHostEnvironment webHostEnviroment)
        {
            this._context = context;
            this._webHostEnviroment = webHostEnviroment;
        }

        public IActionResult Register()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Userid,Fname,Lname,Birthdate,Phonenumber,Email,ImgFile,Address")] ProjectUser projectUser, string userName, string Password)
        {
            if (ModelState.IsValid)
            {
                bool emailCheck = _context.ProjectUsers.Any(x => x.Email == projectUser.Email);
                bool userNameCheck = _context.ProjectUserLogins.Any(x => x.Username == userName);


                if (emailCheck || userNameCheck)
                {
                    return View();
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

                _context.Add(projectUser);
                await _context.SaveChangesAsync();

                ProjectUserLogin projectUserLogin = new ProjectUserLogin();
                projectUserLogin.Username = userName;
                projectUserLogin.Password = Password;
                projectUserLogin.Roleid = 2;
                projectUserLogin.Userid = projectUser.Userid;


                _context.Add(projectUserLogin);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login", "LogReg");



            }

            return View();


        }

        public IActionResult Login()
        {
            return View();
        }

		[HttpPost]
        public IActionResult Login([Bind("Username, Password")] ProjectUserLogin projectUserLogin)
        {
            var auth = _context.ProjectUserLogins.Where(x => x.Username == projectUserLogin.Username && x.Password == projectUserLogin.Password).SingleOrDefault();

            if(auth != null)
			{
                HttpContext.Session.SetInt32("UserID", (int)auth.Userid);

                switch (auth.Roleid)
				{

                    case 1:
                        return RedirectToAction("Index", "AdminDashboard");
                    case 2:
                        return RedirectToAction("Index", "UserDashboard");
				}
			}

            return View();
        }




    }
}
