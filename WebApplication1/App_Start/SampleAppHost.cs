using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Funq;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Caching;
using ServiceStack.Razor;
using ServiceStack.Redis;
using ServiceStack.Validation;
using WebApplication1.App_Start;

[assembly:WebActivatorEx.PostApplicationStartMethod(typeof(SampleAppHost),"Start")]

namespace WebApplication1.App_Start
{
    public class UserAuthSession : AuthUserSession
    {
        public string UserId { get; set; }

        public override void OnAuthenticated(IServiceBase authService, IAuthSession session, IAuthTokens tokens, Dictionary<string, string> authInfo)
        {
            base.OnAuthenticated(authService, session, tokens, authInfo);
            var service = authService.ResolveService<IUserAuthRepository>();
            var userAuth = service.GetUserAuth(session, tokens);
            UserId = userAuth.RefIdStr;

            // we want to save after we set the custom fields
            authService.SaveSession(this);
        }
    }

    public class SampleAppHost : AppHostBase
    {
        private const bool TryUsingRedis = true;

        public SampleAppHost(string serviceName, params Assembly[] assembliesWithServices) : base(serviceName, assembliesWithServices)
        {
            Plugins.Add(new RazorFormat());

        }

        public override void Configure(Container container)
        {
            //Cache
            if (TryUsingRedis && 
                System.Diagnostics.Process.GetProcesses()
                    .Any(p => p.ProcessName.ToLowerInvariant().StartsWith("redis-server")))
            {
                var clientManager = new BasicRedisClientManager();
                container.Register(c => clientManager.GetCacheClient()).ReusedWithin(ReuseScope.Request);
            }
            else
            {
                container.Register<ICacheClient>(new MemoryCacheClient());
            }

            Plugins.Add(new AuthFeature(() => new UserAuthSession(),
               new IAuthProvider[]
                {
                    new BasicAuthProvider(),
                    new CredentialsAuthProvider(), 
                }, htmlRedirect: "/login"));


            Plugins.Add(new RegistrationFeature());

            var userStore = new InMemoryAuthRepository();
            userStore.CreateUserAuth(new UserAuth() { UserName = "test", Email = "teste@example.com", RefIdStr = "users/1" }, "password");
            userStore.CreateUserAuth(new UserAuth() { UserName = "test2", Email = "teste2@example.com", RefIdStr = "users/2" }, "password");
            container.Register<IUserAuthRepository>(userStore);

        }

        public static void Start()
        {
            HostInstance = new SampleAppHost("Sample", typeof(SampleAppHost).Assembly);
            HostInstance.Init();
        }

        public static SampleAppHost HostInstance { get; private set; }
    }


}