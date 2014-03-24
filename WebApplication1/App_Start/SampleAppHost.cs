using System.Reflection;
using Funq;
using ServiceStack;
using ServiceStack.Razor;
using WebApplication1.App_Start;
using WebApplication1.Services;

[assembly:WebActivatorEx.PostApplicationStartMethod(typeof(SampleAppHost),"Start")]

namespace WebApplication1.App_Start
{
    public class SampleAppHost : AppHostBase
    {
        public SampleAppHost(string serviceName, params Assembly[] assembliesWithServices) : base(serviceName, assembliesWithServices)
        {
            Plugins.Add(new RazorFormat());
        }

        public override void Configure(Container container)
        {
            container.Register(c => new Dependency()).ReusedWithin(ReuseScope.Request);
        }

        public static void Start()
        {
            HostInstance = new SampleAppHost("Sample", typeof(SampleAppHost).Assembly);
            HostInstance.Init();
        }

        public static SampleAppHost HostInstance { get; private set; }
    }


}