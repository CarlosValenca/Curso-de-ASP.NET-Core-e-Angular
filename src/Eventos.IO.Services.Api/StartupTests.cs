using AutoMapper;
using Eventos.IO.Application.AutoMapper;
using Eventos.IO.Infra.CrossCutting.Data;
using Eventos.IO.Infra.CrossCutting.Identity.Authorization;
using Eventos.IO.Infra.CrossCutting.Identity.Models;
using Eventos.IO.Infra.CrossCutting.IoC;
using Eventos.IO.Services.Api.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Eventos.IO.Infra.CrossCutting.AspNetFilters;
using MediatR;
using System.Reflection;

namespace Eventos.IO.Services.Api
{
    public class StartupTests
    {
        public StartupTests(IHostingEnvironment env)
        {
            var app = $"appsettings.{env.EnvironmentName}.json";
            // Uma coisa que eu apanhei muito por bobeira, ao criar um novo ambiente é necessário clicar no arquivo
            // appsettings(deste ambiente novo), colocar a opção Always Copy e compilar a aplicação para que este
            // arquivo vá para a pasta "Eventos.IO\tests\Eventos.IO.Tests.API\bin\Debug\netcoreapp2.2"
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile(app, optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // Token(core 2.2)
        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // ssbcvp - instrução dada pelo Rafael da Scania
            services.AddCors();

            services.AddDbContext<ApplicationDbContext>(options =>
                  options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Token(core 2.2)
            var tokenConfigurations = new TokenDescriptor();
            new ConfigureFromConfigurationOptions<TokenDescriptor>(
                    Configuration.GetSection("JwtTokenOptions"))
                .Configure(tokenConfigurations);
            services.AddSingleton(tokenConfigurations);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearerOptions =>
            {
                bearerOptions.RequireHttpsMetadata = false;
                bearerOptions.SaveToken = true;

                var paramsValidation = bearerOptions.TokenValidationParameters;

                paramsValidation.IssuerSigningKey = SigningCredentialsConfiguration.Key;
                paramsValidation.ValidAudience = tokenConfigurations.Audience;
                paramsValidation.ValidIssuer = tokenConfigurations.Issuer;

                paramsValidation.ValidateIssuerSigningKey = true;
                paramsValidation.ValidateLifetime = true;
                paramsValidation.ClockSkew = TimeSpan.Zero;

            });

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddOptions();
            services.AddMvc(options =>
            {
                // Não retornarei nada em Xml, apenas em Json
                options.OutputFormatters.Remove(new XmlDataContractSerializerOutputFormatter());
                // O version determina que eu preciso informar o número da versão da minha action da api
                options.UseCentralRoutePrefix(new RouteAttribute("api/v{version}"));

                var policy = new AuthorizationPolicyBuilder()
                                   .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                                   .RequireAuthenticatedUser()
                                   .Build();
                // Todo lugar que eu decorar com o Authorize, a validação será feita com esta policy aqui,
                //  todas as requisições protegidas eu terei que apresentar um token
                options.Filters.Add(new AuthorizeFilter(policy));
                // Filtro para ser utilizado pelo Elmah
                options.Filters.Add(new ServiceFilterAttribute(typeof(GlobalActionLoggerr)));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddAuthorization(options =>
            {
                options.AddPolicy("PodeLerEventos", policy => policy.RequireClaim("Eventos", "Ler"));
                options.AddPolicy("PodeGravar", policy => policy.RequireClaim("Eventos", "Gravar"));
            });


            // Estou usando o try catch como um quebra galho para evitar o erro "Mapper already initialized"
            try
            {
                Mapper.Reset();

                // Código incluido manualmente para funcionar o auto mapper
                Mapper.Initialize(x =>
                {
                    x.AddProfile<DomainToViewModelMappingProfile>();
                    x.AddProfile<ViewModelToDomainMappingProfile>();
                });

                // AutoMapper
                // Necessário add os assemblies para TestServer
                var assembly = typeof(Program).GetTypeInfo().Assembly;
                services.AddAutoMapper(assembly);
            }
            catch (Exception error)
            {
                // Não faz nada apenas continua o código normalmente uma vez que o Mapper já está inicializado
                error = error;
            }

            services.AddMediatR(typeof(StartupTests));

            // Registrar todos os DI (Dependency Injections)
            RegisterServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
                              IHostingEnvironment env,
                              IHttpContextAccessor acessor,
                              ILoggerFactory loggerFactory)
        {

            // Só colocando este código foi possível habilitar a comunicação entre o Angular e os serviços Asp Net
            app.Use(async (ctx, next) =>
            {
                await next();
                if (ctx.Response.StatusCode == 204)
                {
                    ctx.Response.ContentLength = 0;
                }
            });

            app.UseDeveloperExceptionPage();

            // ssbcvp - voltar aqui - tirei este código
            // app.UseHttpsRedirection();

            // Garante que a aplicação só responda para domínios que ela conheça, só vou aceitar requisições de um determinado site, ou requests de um determinado verbo
            // Por enquanto estamos deixando tudo liberado para requisições externas, logo abaixo tem exemplo de como restringir alguma coisa para requisiçoes externas
            // Por padrão as requisições externas são restringidas se não usar o Cors
            app.UseCors(c =>
            {
                c.AllowAnyHeader();
                c.AllowAnyMethod();
                c.AllowAnyOrigin();
                // Exemplo de restrição de utilização da sua api, requisições de fora só aceitarão os 2 sites abaixo
                // e somente aceitará POSTs, só serve para requisições externas
                // c.WithOrigins("www.eventos.io,www.site.com");
                // c.WithMethods("POST");
            });

            app.UseStaticFiles();
            // Microsoft sugeriu incluir isto no lugar do UseJwtBearerAuthentication que está obsoleto
            // Código veio de https://docs.microsoft.com/pt-br/aspnet/core/migration/1x-to-2x/identity-2x?view=aspnetcore-2.2
            // Token(core 2.2) - usado no lugar de UseIdentity que está obsoleto
            app.UseAuthentication();
            app.UseMvc();
        }

        private static void RegisterServices(IServiceCollection services)
        {
            NativeInjectorBootStrapper.RegisterServices(services);
        }
    }
}
