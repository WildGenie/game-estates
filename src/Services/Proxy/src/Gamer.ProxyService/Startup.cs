﻿using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace Gamer.ProxyService
{
    public class Startup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ILogger>(LogManager.GetLogger(string.Empty));
            services.AddTransient<ProxyService>();
        }
    }
}