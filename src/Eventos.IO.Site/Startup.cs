using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Eventos.IO.Infra.CrossCutting.IoC;
using AutoMapper;
using Eventos.IO.Application.AutoMapper;
using Eventos.IO.Domain.Interfaces;
using Eventos.IO.Infra.CrossCutting.Data;
using Eventos.IO.Infra.CrossCutting.Identity.Models;
using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Logging;
using MediatR;

namespace Eventos.IO.Site
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

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // As variáveis a seguir existem apenas para debugar a string de conexão
            var a = AppDomain.CurrentDomain.BaseDirectory;
            var b = Configuration.GetConnectionString("SqlServerConnection");

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    // Aqui podemos usar o "DefaultConnection" para trabalhar com o LocalDB ou o "SqlServerConnection" para trabalhar com o Sql Server 2017 instalado
                    Configuration.GetConnectionString("SqlServerConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("PodeLerEventos", policy => policy.RequireClaim("Eventos", "Ler"));
                options.AddPolicy("PodeGravar", policy => policy.RequireClaim("Eventos", "Gravar"));
            });

            services.AddLogging();

            // Configuração de rotas para achar as páginas do Identity, conforme auxílio do Patrick
            services.ConfigureApplicationCookie(options =>
            {
                // Define quantos por quantos minutos reconecta com o último usuário logado
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.LoginPath = "/Identity/Account/Login";
                options.LogoutPath = "/Identity/Account/Logout";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.Cookie.HttpOnly = true;
                // Aqui de fato faz o login automático usando o cookie (considerando o tempo do ExpireTimeSpan)
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.SlidingExpiration = true;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            /* ssbcvp - desliguei os filtros do Elmah
            services.AddMvc(options =>
            {
                options.Filters.Add(new ServiceFilterAttribute(typeof(GlobalExceptionHandlingFilter)));
                options.Filters.Add(new ServiceFilterAttribute(typeof(GlobalActionLoggerr)));
            })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            */

            services.AddAutoMapper();
            
            // Código incluido manualmente para funcionar o auto mapper
            Mapper.Initialize(x =>
            {
                x.AddProfile<DomainToViewModelMappingProfile>();
                x.AddProfile<ViewModelToDomainMappingProfile>();
            });

            AutoMapperConfiguration.RegisterMappings();

            // Add application services.
            services.AddScoped<IUser, AspNetUser>();

            services.AddMediatR(typeof(Startup));

            // Add application services.
            RegisterServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
                              IHostingEnvironment env,
                              IHttpContextAccessor acessor,
                              ILoggerFactory loggerFactory)
        {
            // loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            // loggerFactory.AddDebug();

            // ssbcvp - Desabilitei o Elmah por questões de performance, para testar é só descomentar as linhas abaixo
            // loggerFactory.AddElmahIo("ccde64921fbd4ef69bd01c1a097251af", new Guid("809c3d10-450a-4395-bd4a-7fa23aaae94f"));
            // app.UseElmahIo("ccde64921fbd4ef69bd01c1a097251af", new Guid("809c3d10-450a-4395-bd4a-7fa23aaae94f"));

            if (env.IsDevelopment())
            {
                // ssbcvp - Desabilitei os comandos abaixo para fazer igual a produção
                // app.UseDeveloperExceptionPage();
                // app.UseDatabaseErrorPage();
                app.UseExceptionHandler("/erro-de-aplicacao");
                app.UseStatusCodePagesWithReExecute("/erro-de-aplicacao/{0}");
            }
            else
            {
                app.UseExceptionHandler("/erro-de-aplicacao");
                app.UseStatusCodePagesWithReExecute("/erro-de-aplicacao/{0}");
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

        }

        private static void RegisterServices(IServiceCollection services)
        {
            NativeInjectorBootStrapper.RegisterServices(services);
        }

    }
}
