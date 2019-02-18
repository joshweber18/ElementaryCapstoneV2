using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using V2Capstone.Models;

namespace V2Capstone.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            public string Email { get; set; }

            [Required]
            public string Password { get; set; }

            [Display(Name = "Teacher")]
            public bool isTeacher { get; set; }

            [Display(Name = "Parent")]
            public bool isParent { get; set; }

            [Display(Name = "Student")]
            public bool isStudent { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = Input.Email, Email = Input.Email };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    if (!await _roleManager.RoleExistsAsync(StaticDetailsModel.ParentEndUser))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(StaticDetailsModel.ParentEndUser));
                    }
                    if (!await _roleManager.RoleExistsAsync(StaticDetailsModel.TeacherEndUser))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(StaticDetailsModel.TeacherEndUser));
                    }
                    if (!await _roleManager.RoleExistsAsync(StaticDetailsModel.StudentEndUser))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(StaticDetailsModel.StudentEndUser));
                    }
                    if (Input.isTeacher)
                    {
                        await _userManager.AddToRoleAsync(user, StaticDetailsModel.TeacherEndUser);
                        return RedirectToAction("Create", "TeacherModels");
                    }
                    if (Input.isStudent)
                    {
                        await _userManager.AddToRoleAsync(user, StaticDetailsModel.StudentEndUser);
                        return RedirectToAction("Create", "StudentModels");
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, StaticDetailsModel.ParentEndUser);
                        return RedirectToAction("Create", "ParentModels");
                    }

                    //_logger.LogInformation("User created a new account with password.");

                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //var callbackUrl = Url.Page(
                    //    "/Account/ConfirmEmail",
                    //    pageHandler: null,
                    //    values: new { userId = user.Id, code = code },
                    //    protocol: Request.Scheme);

                    //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                    //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    //await _signInManager.SignInAsync(user, isPersistent: false);
                    //return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
