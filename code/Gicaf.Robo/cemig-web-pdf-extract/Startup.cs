using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;

namespace cemig_web_pdf_extract
{
    public class Startup
    {
        Extractor extractor;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            extractor = new Extractor(env);

            var routeBuilder = new RouteBuilder(app);

            routeBuilder.MapGet("", context => context.Response.WriteAsync("Hello from root!"));

            routeBuilder.MapPost("/api/v1/crawly/sped", context =>
            {
                try
                {
                    using (var reader = new StreamReader(context.Request.Body))
                    {
                        var body = reader.ReadToEnd();

                        var files = context.Request.Form.Files;

                        byte[] content = null;
                        using (var ms = new MemoryStream())
                        {
                            //file.CopyTo(ms);
                            content = ms.ToArray();
                        }


                        dynamic payload = JObject.Parse(body);

                        if (payload.pdf == null)
                        {
                            throw new ArgumentException("Invalid input.", "pdf");
                        }

                        try
                        {
                            var data = extractor.Extract(((string)payload.pdf));
                            return context.Response.WriteAsync(data.ToString());
                        } 
                        catch (FormatException e)
                        {
                            throw e;
                        }
                        
                    }

                } 
                catch (Exception e)
                {
                    var error = new JObject();
                    error.Add("ErrorType", e.GetType().Name);
                    error.Add("Error", e.Message);

                    context.Response.StatusCode = 400;
                    return context.Response.WriteAsync(error.ToString());
                }
            });

            var routes = routeBuilder.Build();
            app.UseRouter(routes);
        }
    }
}
