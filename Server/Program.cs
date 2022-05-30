using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var server = serviceProvider.GetService<IServer>();

            Console.WriteLine("Iniciando a aplicação");

            server.ExecuteServer();
        }
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IServerService, ServerService>();
            services.AddTransient<IServer, Server>();
        }
    }
}
