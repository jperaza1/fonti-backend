using cod.application.Common.Interfaces;
using cod.application.EnumDefinition.Queries.GetDetailStatus;
using cod.application.EnumDefinition.Queries.GetDishType;
using cod.application.EnumDefinition.Queries.GetEmploymentStatusType;
using cod.application.EnumDefinition.Queries.GetEmploymentType;
using cod.application.EnumDefinition.Queries.GetGenderType;
using cod.application.EnumDefinition.Queries.GetInvoiceType;
using cod.application.EnumDefinition.Queries.GetPayFrequencyType;
using cod.application.EnumDefinition.Queries.GetPaymentMethod;
using cod.application.EnumDefinition.Queries.GetPaymentType;
using cod.application.EnumDefinition.Queries.GetPurchaseOrderStatus;
using cod.application.EnumDefinition.Queries.GetRequestType;
using cod.application.EnumDefinition.Queries.GetSaleType;
using cod.application.EnumDefinition.Queries.GetSaleTypeDish;
using cod.application.EnumDefinition.Queries.GetShiftType;
using cod.application.EnumDefinition.Queries.GetSpecialServiceStatus;
using cod.data.DependencyInjection;
using cod.data.Utility;
using cod.domain.Features;
using cod.domain.Features.Enums;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;

namespace cod.data
{
    public partial class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IGetEnumDefinitionValuesQuery<PurchaseOrderStatus>, GetPurchaseOrderStatusQuery>();
            services.AddScoped<IGetEnumDefinitionValuesQuery<DetailStatus>, GetDetailStatusQuery>();
            services.AddScoped<IGetEnumDefinitionValuesQuery<PaymentType>, GetPaymentTypeQuery>();
            services.AddScoped<IGetEnumDefinitionValuesQuery<InvoiceType>, GetInvoiceTypeQuery>();
            services.AddScoped<IGetEnumDefinitionValuesQuery<PaymentMethod>, GetPaymentMethodQuery>();
            services.AddScoped<IGetEnumDefinitionValuesQuery<SaleType>, GetSaleTypeQuery>();
            services.AddScoped<IGetEnumDefinitionValuesQuery<SpecialServiceStatus>, GetSpecialServiceStatusQuery>();
            services.AddScoped<IGetEnumDefinitionValuesQuery<GenderType>, GetGenderType>();
            services.AddScoped<IGetEnumDefinitionValuesQuery<EmploymentStatusType>, GetEmploymentStatusTypeQuery>();
            services.AddScoped<IGetEnumDefinitionValuesQuery<EmploymentType>, GetEmploymentTypeQuery>();
            services.AddScoped<IGetEnumDefinitionValuesQuery<ShiftType>, GetShiftTypeQuery>();
            services.AddScoped<IGetEnumDefinitionValuesQuery<PayFrequencyType>, GetPayFrequencyTypeQuery>();
            services.AddScoped<IGetEnumDefinitionValuesQuery<DishType>, GetDishTypeQuery>();
            services.AddScoped<IGetEnumDefinitionValuesQuery<SaleTypeDish>, GetSaleTypeDishQuery>();
            services.AddScoped<IGetEnumDefinitionValuesQuery<VacationRequestType>, GetRequestTypeQuery>();

            CustomAssemblyLoadContext context = new CustomAssemblyLoadContext();

            //context.LoadUnmanagedLibrary(Path.Combine(Directory.GetCurrentDirectory(), "libwkhtmltox.dll"));

            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local; // this should be set if you always expect UTC dates in method bodies, if not, you can use RoundTrip instead.
                });

            services.AddDistributedMemoryCache();

            this.SwaggerConfigure(services);

            services.AddTransient<AuthService>();

            this.CorsConfigure(services);

            this.DatabaseConfigure(services);

            this.IdentityConfigure(services);

            var container = new Container();

            container.Configure(config =>
            {
                config.Scan(scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                    scan.AssembliesFromApplicationBaseDirectory(filter => filter.FullName.Contains("cod"));
                });
                config.AddRegistry(new StructureMapRegistry());
                config.Populate(services);
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });


            

            return container.GetInstance<IServiceProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areaRoute",
                    pattern: "{area:exists}/{controller}/{action}",
                    defaults: new { action = "Index" });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" });

                endpoints.MapControllerRoute(
                    name: "api",
                    pattern: "{controller}/{id?}");
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "Data v2");
            });

        }

    }

}