﻿using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Syra.Admin.DbContexts;
using Syra.Admin.Entities;
using Syra.Admin.Models;
using Syra.Admin.Helper;

namespace Syra.Admin.Controllers
{
    //[Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private SyraDbContext db = new SyraDbContext();
        private Response response = new Response();
        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = "";
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model ,string returnUrl)
        {
            //AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = model.RememberMe }, identity);
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if(ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if(user==null)
                {
                    ModelState.AddModelError("", "Invalid EmailId or Password");
                    return View(model);
                }
                if (!await UserManager.IsEmailConfirmedAsync(user.Id))
                {
                    Session.Abandon();
                    ModelState.AddModelError("", "You need to confirm your email.");
                    return View(model);
                }
            }
            //HttpCookie httpCookie = new HttpCookie("user",model.Email);
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            if(model.RememberMe==true)
            {
                Response.Cookies["userid"].Value = model.Email;
                Response.Cookies["pwd"].Value = model.Password;
                Response.Cookies["userid"].Expires = DateTime.Now.AddDays(15);
                Response.Cookies["pwd"].Expires = DateTime.Now.AddDays(15);
            }
            else
            {
                Response.Cookies["userid"].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies["pwd"].Expires = DateTime.Now.AddDays(-1);

            }
            switch (result)
            {
                case SignInStatus.Success:
                    var user = new ClaimsPrincipal(AuthenticationManager.AuthenticationResponseGrant.Identity);
                    if (user.IsInRole("Admin"))
                    {
                        return RedirectToLocal("/home/#/managecustomer");
                    }
                    else
                    {
                        return RedirectToLocal("/#/Profile");
                    }
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.HttpMethod == "POST")
            {
                LoginViewModel model = new LoginViewModel();
                if (Request.Cookies["userid"] != null)
                {
                    model.Email= Request.Cookies["userid"].Value;
                }
                if (Request.Cookies["pwd"] != null)
                {
                    model.Password = Request.Cookies["pwd"].Value;
                }  
                if (Request.Cookies["userid"] != null && Request.Cookies["pwd"] != null)
                    model.RememberMe= true;
            }
        }
        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            SyraDbContext db = new SyraDbContext();
            ViewBag.PlanId = new SelectList(db.Plans, "Id", "Name");
            return View();
        }

        [HttpGet]
        public string GetPlans()
        {
            var plans = db.Plans.ToList();
            response.Data = from a in plans
                            select new
                            {
                                a.Id,
                                a.Name
                            };
            return response.GetResponse();
        }


        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult RegisterAdmin()
        {
           
            return View();
        }

        public JsonResult IsUserExists(string Email)
        {
            //check if any of the UserName matches the UserName specified in the Parameter using the ANY extension method.  
            return Json(!db.Users.Any(x => x.Email == Email), JsonRequestBehavior.AllowGet);
        }
        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if(ModelState.IsValid)
            {
                if (model != null)
                {
                    var userStore = new UserStore<ApplicationUser>(db);
                    var manager = new UserManager<ApplicationUser>(userStore);
                    var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                    var customer = new Customer();
                    var existingUser = await UserManager.FindByNameAsync(model.Email);
                    if (existingUser == null)
                    {
                        var result = await UserManager.CreateAsync(user, model.Password);
                        if (result.Succeeded)
                        {
                            var customerPlan = new CustomerPlan();
                            customerPlan.PlanId = model.PlanId;
                            customerPlan.IsActive = true;
                            customerPlan.CustomerId = customer.Id;
                            customer.UserId = user.Id;
                            customer.JobTitle = model.JobTitle;
                            customerPlan.ActivationDate = DateTime.Now;
                            customerPlan.ExpiryDate = DateTime.Now;
                            customer.Email = model.Email;
                            customer.RegisterDate = DateTime.Now;
                            customer.BusinessRequirement = model.BusinessRequirement;
                            //model.CategoryList = new SelectList(db.Plans, "Id", "Name");
                            var check_duplicate = db.Customer.Where(x => x.Email == customer.Email).FirstOrDefault();
                            if (check_duplicate == null)
                            {
                                customer.CustomerPlans.Add(customerPlan);
                                db.Customer.Add(customer);
                                db.SaveChanges();
                            }
                            await UserManager.AddToRoleAsync(user.Id, "Customer");

                            //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                            // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                            // Send an email with this link
                            string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                            var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                            await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking this link: \"" + callbackUrl + "\"");

                            return RedirectToAction("AccountConfirmation", "Account");
                        }
                        else
                        {
                            AddErrors(result);
                            ModelState.AddModelError("", result.ToString());
                            //model.ErrorMessage = result.Errors.ToList();
                        }
                    }
                    else
                    {
                        //ViewBag.ErrorMessage("Email already exists");
                        ModelState.AddModelError("Email", "Email already exists.");
                        
                    }
                    return View(model);
                }
            }
            return View(model);
            //return View(model);
        }

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public string Register(RegisterViewModel model)
        //{
        //    if (model != null)
        //    {
        //        var userStore = new UserStore<ApplicationUser>(db);
        //        var manager = new UserManager<ApplicationUser>(userStore);

        //        var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
        //        var customer = new Customer();
        //        var existingUser = UserManager.FindByEmail(model.Email);
        //        if (existingUser == null)
        //        {
        //            var result = manager.Create(user, model.Password);
        //            if (result.Succeeded)
        //            {
        //                var customerPlan = new CustomerPlan();
        //                customerPlan.PlanId = model.PlanId;
        //                customerPlan.IsActive = true;
        //                customerPlan.CustomerId = customer.Id;
        //                customer.UserId = user.Id;
        //                customer.JobTitle = model.JobTitle;
        //                customerPlan.ActivationDate = DateTime.Now;
        //                customerPlan.ExpiryDate = customerPlan.ActivationDate.AddYears(1);
        //                customer.Email = model.Email;
        //                customer.RegisterDate = DateTime.Now;
        //                customer.BusinessRequirement = model.BusinessRequirement;
        //                //var check_duplicate = db.Customer.Where(x => x.Email == customer.Email).FirstOrDefault();
        //                if (existingUser == null)
        //                {
        //                    customer.CustomerPlans.Add(customerPlan);
        //                    db.Customer.Add(customer);
        //                    db.SaveChanges();
        //                }
        //                manager.AddToRoleAsync(user.Id, "Customer");

        //                var task1=SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false).Status;

        //                //For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
        //                //Send an email with this link

        //                //string code = UserManager.GenerateEmailConfirmationToken(user.Id);
        //                //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
        //                //UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>");

        //                //return RedirectToAction("AccountConfirmation", "Account");

        //                response.IsSuccess = true;
        //                response.Message = "Your account is registered successfully. Please check your email";
        //            }
        //            else
        //            {
        //                response.IsSuccess = false;
        //                response.Message = "Customer does not exist";
        //            }
        //        }
        //        else
        //        {
        //            response.IsSuccess = false;
        //            response.Message = "Email already exists.";
        //        }
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return response.GetResponse();
        //}


        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterAdmin(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser {UserName = model.Email, Email = model.Email};
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    string CurrentUserId = user.Id;

                   await UserManager.AddToRoleAsync(user.Id, "Admin");

                    //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [AllowAnonymous]
        public ActionResult AccountConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AccountConfirmation(ForgotPasswordViewModel model)
        {

            return View(model);
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public string ChangePassWord(ResetPasswordViewModel model)
        {
            if(model!=null)
            {
                _signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
                _userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var useremail = HttpContext.User.Identity.Name;
                var aspnetuser = _userManager.FindByEmailAsync(useremail).Result;

                //var userStore = new UserStore<ApplicationUser>(db);
                //var manager = new UserManager<ApplicationUser>(userStore);

                //var user = new ApplicationUser { UserName = aspnetuser.Email };
                //var existingUser = UserManager.FindByEmail(model.Email);
                //if(existingUser==null || !(UserManager.IsEmailConfirmed(existingUser.Id)))
                //{
                //    response.Message = "Email not found";
                //}
                string resetToken = UserManager.GeneratePasswordResetToken(aspnetuser.Id);
                model.Code = resetToken;
                var result = UserManager.ResetPassword(aspnetuser.Id, model.Code, model.Password);
                if(result.Succeeded)
                {
                    response.IsSuccess = true;
                    response.Message = "Password is reset successfully";
                }
                else
                {
                    response.IsSuccess = false;
                }
            }
            return response.GetResponse();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                //Send an email with this link
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            //return Redirect(returnUrl);
            //if (Url.IsLocalUrl(returnUrl))
            //{
            //    return Redirect(returnUrl);
            //}
            
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}