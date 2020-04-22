using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Localization;

namespace Localization.Web.UI.Infrastructure
{
    public class BaseController : Controller
    {
        #region Field(s)

        private string _currentLanguage;

        #endregion

        #region Properties
        
        private string CurrentLanguage
        {
            get
            {
                if (!string.IsNullOrEmpty(_currentLanguage))
                    return _currentLanguage;

                if (string.IsNullOrEmpty(_currentLanguage))
                {
                    var feature = HttpContext.Features.Get<IRequestCultureFeature>();
                    _currentLanguage = feature.RequestCulture.Culture.TwoLetterISOLanguageName.ToLower();
                    _currentLanguage = feature.RequestCulture.Culture.IetfLanguageTag;
                }

                return _currentLanguage;
            }
        }

        #endregion

        public ActionResult RedirectToDefaultCulture()
        {
            var culture = CurrentLanguage;

            return RedirectToAction(actionName: "Index", controllerName: "Home", routeValues: new { culture });
        }

        public ActionResult ChangeCurrentCulture(string cultureName)
        {
            System.Uri UrlReferrer = new System.Uri(Request.Headers["Referer"].ToString());
            string currentUrl = string.Empty;

            string Scheme = Request.Scheme;
            string Host = Request.Host.ToString();

            if (UrlReferrer is null)
                return Redirect(url: $"{Scheme}://{Host}/");

            switch (cultureName)
            {
                case Common.Infrastructure.Culture.English_UnitedStates:
                    {
                        currentUrl = Request.Headers["Referer"].ToString().Replace(oldValue: Common.Infrastructure.Culture.Farsi_Iran, newValue: cultureName)?.ToString();
                    }
                    break;

                case Common.Infrastructure.Culture.Farsi_Iran:
                    {
                        currentUrl = Request.Headers["Referer"].ToString().Replace(oldValue: Common.Infrastructure.Culture.English_UnitedStates, newValue: cultureName)?.ToString();
                    }
                    break;
            }

            return Redirect(url: currentUrl);
        }
    }
}