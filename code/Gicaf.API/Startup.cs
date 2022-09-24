using System.Reflection.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Gicaf.API.Authentication.HttpClientSample;
using Gicaf.API.Controllers.Authenticator;
using Gicaf.Infra.Data.Repositories;
using Gicaf.Infra.CrossCutting.IoC;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VGPartner.Infra.CrossCutting.Utils.RazorMailer;

namespace Gicaf.API
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddEnvironmentVariables();

            bool reloadOnChange = env.IsDevelopment();
            var appSettings = env.IsProduction() ? "appsettings.json" : ../../"appsettings.{env.EnvironmentName}.json";
            builder.AddJsonFile(appSettings, optional: false, reloadOnChange: reloadOnChange);

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<EmailConfig>(Configuration.GetSection("Email"));
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder
                        .AllowAnyOrigin() 
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                    });
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            RegisterServices(services, Configuration);
            ConfigureAuthentication(services);                       
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IOptions<EmailConfig> emailConf)
        {
            AuthenticatorSettings.UrlBase = Configuration.GetSection("Urls").GetValue<string>("Authenticator");
            //InjectorBootStrapper.LoadConfiguration(Configuration);            

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                // Make sure you call this before calling app.UseMvc()
                app.UseCors("AllowAll");
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.UseMvc();
            
            EmailManager.Create(emailConf.Value)
                .CompileTemplate<Infra.CrossCutting.IoC.ConfirmacaoEmail>("BemVindo")
                .CompileTemplate<Infra.CrossCutting.IoC.ResultadoAnaliseEmail>("Aprovacao")
                .CompileTemplate<Infra.CrossCutting.IoC.ResultadoAnaliseEmail>("Reprovacao")
                .CompileTemplate<Gicaf.API.Controllers.Authenticator.ConfirmacaoEmail>("TrocaEmail");
            //app.UseGraphQl(env.WebRootPath);
            /*
            app.UseRouter((router) => {
                router.MapRoute("Default", "gql/api/{controller}/{obj}");
                router.MapRoute("Default", "gql/api/{controller}");
            });*/
        }

        private static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            // Adding dependencies from another layers (isolated from Presentation)
            InjectorBootStrapper.RegisterServices(services, configuration);
        }

        private void ConfigureAuthentication(IServiceCollection services)
        {
            var signingConfigurations = new SigningConfigurations();
            services.AddSingleton(signingConfigurations);

            var tokenConfigurations = new TokenConfigurations();
            new ConfigureFromConfigurationOptions<TokenConfigurations>(
                Configuration.GetSection("TokenConfigurations"))
                    .Configure(tokenConfigurations);
            services.AddSingleton(tokenConfigurations);

            services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearerOptions =>
            {
                var paramsValidation = bearerOptions.TokenValidationParameters;
                paramsValidation.IssuerSigningKey = signingConfigurations.Key;
                paramsValidation.ValidAudience = tokenConfigurations.Audience;
                paramsValidation.ValidIssuer = tokenConfigurations.Issuer;

                // Valida a assinatura de um token recebido
                paramsValidation.ValidateIssuerSigningKey = true;

                // Verifica se um token recebido ainda é válido
                paramsValidation.ValidateLifetime = true;

                // Tempo de tolerância para a expiração de um token (utilizado
                // caso haja problemas de sincronismo de horário entre diferentes
                // computadores envolvidos no processo de comunicação)
                paramsValidation.ClockSkew = TimeSpan.Zero;
            });

            // Ativa o uso do token como forma de autorizar o acesso
            // a recursos deste projeto
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new JwtAuthorizationPolicyBuilder().Configure());
                    //.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                    //.RequireAuthenticatedUser().Build());
            });
        }
    }
}
