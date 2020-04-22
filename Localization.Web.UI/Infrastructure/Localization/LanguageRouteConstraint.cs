using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Localization.Web.UI.Infrastructure.Localization
{
    public class LanguageRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (!values.ContainsKey("culture"))
                return false;

            var culture = values["culture"].ToString();
            return culture == Common.Infrastructure.Culture.Farsi_Iran ||
                   culture == Common.Infrastructure.Culture.English_UnitedStates;
        }
    }
}