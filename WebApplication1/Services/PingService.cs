using System;
using System.Threading;
using System.Threading.Tasks;
using ServiceStack;

namespace WebApplication1.Services
{
    [Route("/ping")]
    public class EchoRequest
    {
    }
    
    [Route("/async_ping")]
    public class AsyncEchoRequest
    {
    }

    public class Dependency : IDisposable
    {
        public static int DisposedCount;

        public void Dispose()
        {
            DisposedCount++;
            System.Diagnostics.Trace.Write("Disposed");
        }
    }

    public class PingService : Service
    {
        private readonly Dependency _dependency;

        public PingService(Dependency dependency)
        {
            _dependency = dependency;
        }

        public string Get(EchoRequest request)
        {
            return "Dependency was disposed {0} times".Fmt(Dependency.DisposedCount);
        }
    }
    
    public class AsyncPingService : Service
    {
        private readonly Dependency _dependency;

        public AsyncPingService(Dependency dependency)
        {
            _dependency = dependency;
        }

        public async Task<string> Get(AsyncEchoRequest request)
        {
            return await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(TimeSpan.FromMilliseconds(100));
                return "Dependency was disposed {0} times".Fmt(Dependency.DisposedCount);
            });
        }
    }
}