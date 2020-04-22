using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;

namespace Localization.Web.UI.Infrastructure.Localization
{
    public class LocalizationPipeline
    {
        public void Configure(IApplicationBuilder app)
        {
            var options = new RequestLocalizationOptions();
            ConfigureOptions(options);

            app.UseRequestLocalization(options);
        }

        public static void ConfigureOptions(RequestLocalizationOptions options)
        {
            var supportedCultures = new List<CultureInfo>
                                {
                                    new CultureInfo(Common.Infrastructure.Culture.English_UnitedStates),
                                    new CultureInfo(Common.Infrastructure.Culture.Farsi_Iran),
                                };

            options.DefaultRequestCulture = new RequestCulture(culture: Common.Infrastructure.Culture.Farsi_Iran, uiCulture: Common.Infrastructure.Culture.Farsi_Iran);
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
            options.RequestCultureProviders = new[] {
                new RouteDataRequestCultureProvider()
                {
                    Options = options,
                    RouteDataStringKey = "culture",
                    UIRouteDataStringKey = "culture"
                }
            };
        }
    }
}