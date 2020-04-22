using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Localization.Web.UI.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        #region Fiels(s)

        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        #endregion

        #region Constructor

        public LoginModel(SignInManager<IdentityUser> signInManager,
            ILogger<LoginModel> logger,
            UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        #endregion

        #region Properties
        
        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        #endregion

        #region Class
        
        public class InputModel
        {
            // **********
            [System.ComponentModel.DataAnnotations.Display
                (ResourceType = typeof(Resource.Models.SchemaDbo.AspNetUsers.AspNetUser),
                Name = nameof(Resource.Models.SchemaDbo.AspNetUsers.AspNetUser.Email))]

            [System.ComponentModel.DataAnnotations.Required
                (AllowEmptyStrings = false,
                    ErrorMessageResourceType = typeof(Resource.Messages.Message),
                    ErrorMessageResourceName = nameof(Resource.Messages.Message.Required))]

            [System.ComponentModel.DataAnnotations.StringLength
                (maximumLength: 256,
                    ErrorMessageResourceType = typeof(Resource.Messages.Message),
                    ErrorMessageResourceName = nameof(Resource.Messages.Message.StringLength))]

            [System.ComponentModel.DataAnnotations.RegularExpression
                (Common.RegularExpressions.Patterns.Email,
                    ErrorMessageResourceType = typeof(Resource.Messages.Message),
                    ErrorMessageResourceName = nameof(Resource.Messages.Message.RegularExpressionForEmail))]
            public string Email { get; set; }
            // **********

            // **********
            [System.ComponentModel.DataAnnotations.Display
                (ResourceType = typeof(Resource.Web.UI.Identity.Account.Logins.Login),
                Name = nameof(Resource.Web.UI.Identity.Account.Logins.Login.Password))]

            [System.ComponentModel.DataAnnotations.Required
                (AllowEmptyStrings = false,
                    ErrorMessageResourceType = typeof(Resource.Messages.Message),
                    ErrorMessageResourceName = nameof(Resource.Messages.Message.Required))]

            [System.ComponentModel.DataAnnotations.DataType
                (System.ComponentModel.DataAnnotations.DataType.Password)]
            public string Password { get; set; }
            // **********

            // **********
            [System.ComponentModel.DataAnnotations.Display
                (ResourceType = typeof(Resource.Web.UI.Identity.Account.Logins.Login),
                Name = nameof(Resource.Web.UI.Identity.Account.Logins.Login.RememberMe))]
            public bool RememberMe { get; set; }
            // **********
        } 

        #endregion

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}