using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ROYALHOTEL.Models;

namespace ROYALHOTEL.Controllers
{
    public class LoginandRegisterController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnviroment;

        public LoginandRegisterController (ModelContext context, IWebHostEnvironment webHostEnviroment)
        {
            _context = context;
            _webHostEnviroment = webHostEnviroment;
        }


        public IActionResult Index()
        {
            return View();
        }

        //Register

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Userid,Userfname,Userlname,Email,ImageFile")] User user, string username, string password)
        {
            if (!ModelState.IsValid)
            {
                if (user.ImageFile != null)
                {
                    // Handle image upload
                    string wwwRootPath = _webHostEnviroment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + "_" + user.ImageFile.FileName;
                    string path = Path.Combine(wwwRootPath + "/Images", fileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await user.ImageFile.CopyToAsync(fileStream);
                    }

                    user.Imagepath = fileName;
                    _context.Add(user);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    // Set a default image if no image file is provided
                    user.Imagepath = "cust1.jpg"; // Ensure this default image exists in the Images folder
                }

                
                
                Login login = new Login();
                login.Userid = user.Userid;
                login.Password = password;
                login.Username = username;
       
                login.Roleid = 2;
                _context.Add(login);
               await _context.SaveChangesAsync();
                return RedirectToAction("UserLogin", "LoginandRegister");

            }
            return View(user);
        }



        //Login


        public IActionResult UserLogin()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> UserLogin([Bind("Username,Password")] Login userlogin)
        {
            var auth= _context.Logins.Where(x=> x.Password==userlogin.Password && x.Username== userlogin.Username).SingleOrDefault();
           
            if (auth == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(userlogin);
            }

            if (auth.Roleid == 2)
            {
                if (auth.Userid.HasValue)
                {
                    HttpContext.Session.SetInt32("Userid", Decimal.ToInt32(auth.Userid.Value)); // Convert from decimal to int
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Userid is null.");
                    return View(userlogin);
                }
                HttpContext.Session.SetString("UserNAME", auth.Username);
                return RedirectToAction("Index", "User");
            }
            else if (auth.Roleid == 1)
            {
                HttpContext.Session.SetInt32("ADMINid", (int)auth.Loginid);
                HttpContext.Session.SetString("UserNAME", auth.Username);
                return RedirectToAction("Index", "AdminController1"); // Ensure this action and controller exist
            }

            return View(userlogin);
        }





    }
}