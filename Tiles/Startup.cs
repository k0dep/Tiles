using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;
using Tiles.Models;
using Tiles.Models.Data;

namespace Tiles
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddTransient(MongoFactory);
            services.AddTransient<PostsRepository>();
            services.AddTransient<TileRepository>();
            services.AddTransient<UsersRepository>();

            ConfigureYandexMetrica(services);

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(opt =>
                {
                    opt.LoginPath = new PathString("/account/signin");
                });
        }

        private void ConfigureYandexMetrica(IServiceCollection services)
        {
            var yaId = Configuration.GetValue<int?>("YandexMetricaId") ?? int.MaxValue;
            var metrica = new YandexMetricaCounterId(yaId);

            services.AddSingleton(metrica);
        }


        public void Configure(IApplicationBuilder app, IHostingEnvironment env, UsersRepository users)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();

                Task.Run(async () =>
                {
                    if (await users.GetUser("admin") == null)
                        await users.SaveUser(new User()
                        {
                            Login = "admin",
                            Password = "admin"
                        });
                });
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Tile}/{action=Index}/{id?}");
            });
        }



        IMongoDatabase MongoFactory(IServiceProvider serviceProvider)
        {
            string connectionString = Configuration.GetValue<string>("Mongo");
            var connection = new MongoUrlBuilder(connectionString);
            var client = new MongoClient(connectionString);
            return client.GetDatabase(connection.DatabaseName);
        }
    }
}
