using System.Globalization;
using System.Threading;
using System.Web;
using WebApplication1.App_Start;
using WebApplication1.Resources;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof (LanguageHandler), "Start")]

namespace WebApplication1.App_Start
{
    public class LanguageHandler : IHttpModule
    {
        public static void Start()
        {
            Microsoft.Web.Infrastructure.DynamicModuleHelper.DynamicModuleUtility.RegisterModule(
                typeof (LanguageHandler));
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += context_BeginRequest;
        }

        private void context_BeginRequest(object sender, System.EventArgs e)
        {
            HttpApplication application = (HttpApplication) sender;
            HttpContext context = application.Context;


            // REF for l10n: http://msdn.microsoft.com/en-us/library/bz9tc508%28v=vs.100%29.ASPX
            var req = context.Request;
            var hasLanguage = req.Cookies.Get("_language") != null
                              || req.QueryString.Get("_l") != null;

            if (!hasLanguage) return;
            var language = Languages.DefaultLanguage;

            if (!string.IsNullOrWhiteSpace((req.Cookies.Get("_language") ?? new HttpCookie("_language")).Value))
                language = req.Cookies.Get("_language").Value;

            if (!string.IsNullOrWhiteSpace(req.QueryString.Get("_l")))
                language = req.QueryString["_l"];


            var languageCulture = Languages.ReduceToSupportedLanguage(language);
            Thread.CurrentThread.CurrentCulture =
                CultureInfo.CreateSpecificCulture(languageCulture.Name);
            Thread.CurrentThread.CurrentUICulture = new
                CultureInfo(languageCulture.Name);
        }

        public void Dispose()
        {
        }
    }
}