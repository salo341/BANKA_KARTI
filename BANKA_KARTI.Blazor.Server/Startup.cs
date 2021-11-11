using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevExpress.ExpressApp.Security;
using DevExpress.Blazor.Reporting;
using DevExpress.ExpressApp.ReportsV2.Blazor;
using DevExpress.ExpressApp.Dashboards.Blazor;
using DevExpress.ExpressApp.Office.Blazor;
using DevExpress.ExpressApp.Blazor.Services;
using DevExpress.Persistent.Base;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BANKA_KARTI.Blazor.Server.Services;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.ExpressApp.WebApi.Services;
using DevExpress.ExpressApp.WebApi.Swashbuckle;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.OData;

namespace BANKA_KARTI.Blazor.Server {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services) {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddHttpContextAccessor();
            services.AddSingleton<XpoDataStoreProviderAccessor>();
            services.AddScoped<CircuitHandler, CircuitHandlerProxy>();
            services.AddXaf<BANKA_KARTIBlazorApplication>(Configuration);
            services.AddXafReporting();
            services.AddXafDashboards();
            services.AddXafOffice();
            services.AddXafSecurity(options => {
                options.RoleType = typeof(PermissionPolicyRole);
                // ApplicationUser descends from PermissionPolicyUser and supports the OAuth authentication. For more information, refer to the following topic: https://docs.devexpress.com/eXpressAppFramework/402197
                // If your application uses PermissionPolicyUser or a custom user type, set the UserType property as follows:
                options.UserType = typeof(BANKA_KARTI.Module.BusinessObjects.ApplicationUser);
                // ApplicationUserLoginInfo is only necessary for applications that use the ApplicationUser user type.
                // If you use PermissionPolicyUser or a custom user type, comment out the following line:
                options.UserLoginInfoType = typeof(BANKA_KARTI.Module.BusinessObjects.ApplicationUserLoginInfo);
                options.Events.OnSecurityStrategyCreated = securityStrategy => ((SecurityStrategy)securityStrategy).RegisterXPOAdapterProviders();
                options.SupportNavigationPermissionsForTypes = false;
            }).AddExternalAuthentication<HttpContextPrincipalProvider>()
            .AddAuthenticationStandard(options => {
                options.IsSupportChangePassword = true;
            });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => {
                options.LoginPath = "/LoginPage";
            });
            services.AddXafWebApi(options => {
                // Use options.BusinessObject<YourBusinessObject>() to make the Business Object available in Web API and generate for it the GET, POST, PUT, and DELETE HTTP methods.
            });
            services.AddControllers().AddOData(options => {
                options
                    .AddRouteComponents("api/odata", new XafApplicationEdmModelBuilder(services).GetEdmModel())
                    .EnableQueryFeatures(100);
            });
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo {
                    Title = "BANKA_KARTI API",
                    Version = "v1",
                    Description = @"Use AddXafWebApi(options) in the BANKA_KARTI.Blazor.Server\Startup.cs file to make Business Objects available in Web API."
                });
                c.SchemaFilter<XpoSchemaFilter>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if(env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "BANKA_KARTI WebApi v1");
                });
            }
            else {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. To change this for production scenarios, see: https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseRequestLocalization();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseXaf();
            app.UseDevExpressBlazorReporting();
            app.UseXafDashboards();
            app.UseEndpoints(endpoints => {
                endpoints.MapXafDashboards();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
                endpoints.MapControllers();
            });
        }
    }
}
