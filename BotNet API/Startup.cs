// This code is licensed under the isc license. You can improve the code by keeping this comments 
// (or by any other means, with saving authorship by Zerumi and PizhikCoder retained)
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BotNet_API.Models;
using BotNet_API.Hubs;
using System;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Threading;
using Microsoft.AspNetCore.SignalR;
using BotNet_API.Auth;
using BotNet_API.Auth.Requirements;

namespace BotNet_API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ClientContext>(opt =>
                   opt.UseInMemoryDatabase("ClientList"));
            services.AddDbContext<CommandContext>(opt =>
                   opt.UseInMemoryDatabase("CommandList"));
            services.AddDbContext<ResponseContext>(opt =>
                   opt.UseInMemoryDatabase("ResponseList"));
            services.AddDbContext<ScreenContext>(opt =>
                   opt.UseInMemoryDatabase("ScreenList"));
            services.AddSignalR();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Administrator", policy =>
                    policy.Requirements.Add(new AdminRequirement(true)));
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseSignalR(routes =>
            {
                routes.MapHub<ResponseHub>("/signalr/response");
            });
            app.UseMvc();
        }
    }
}