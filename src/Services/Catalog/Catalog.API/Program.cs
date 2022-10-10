using AutoMapper;
using Catalog.API.DTOs.CatalogItem;
using Catalog.API.Profiles;
using Catalog.Core.Models;
using Catalog.DataAccess;
using Catalog.DataAccess.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Catalog.API {
	public class Program {
		public static async Task Main(string[] args) {
			WebApplicationBuilder webApplicationBuilder = WebApplication.CreateBuilder(args);
			AddServicesToContainer(webApplicationBuilder);
			WebApplication webApplication = webApplicationBuilder.Build();
			ConfigureHttpRequestPipeline(webApplication);
			await PrepareDatabase.MigrateAndSeedAsync(webApplication);
			webApplication.Run();
		}

		private static void AddServicesToContainer(WebApplicationBuilder webApplicationBuilder) {
			webApplicationBuilder.Services.AddOptions();
			webApplicationBuilder.Services.AddLogging();
			// webApplicationBuilder.Services.Configure<CatalogOptions>(webApplicationBuilder.Configuration);
			webApplicationBuilder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			webApplicationBuilder.Services.AddEndpointsApiExplorer();
			webApplicationBuilder.Services.AddSwaggerGen();

			// Register AutoMapper service #1
			//MapperConfiguration mapConfiguration = new MapperConfiguration(configure: config => {
			//	config.CreateMap<CatalogItem, CatalogItemReadDTO>();
			//	//or
			//	config.AddProfile(new CatalogItemProfile());
			//});
			//IMapper mapper = mapConfiguration.CreateMapper();
			//webApplicationBuilder.Services.AddSingleton(mapper);

			// Register AutoMapper service #2
			//webApplicationBuilder.Services.AddAutoMapper(configAction: mapperConfigurationExpression => mapperConfigurationExpression.AddProfile(new CatalogItemProfile()));

			// Register AutoMapper service #3
			webApplicationBuilder.Services.AddAutoMapper(assemblies: AppDomain.CurrentDomain.GetAssemblies());
			webApplicationBuilder.Services.AddRouting(opt => opt.LowercaseUrls = true);

			webApplicationBuilder.Services.AddDbContext<CatalogContext>(options => {
				options.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("CatalogConnectionString"),
									 options => options.MigrationsAssembly("Catalog.DataAccess")
													   .EnableRetryOnFailure(
														   maxRetryCount: 6,
														   maxRetryDelay: TimeSpan.FromSeconds(30),
														   errorNumbersToAdd: null));
				options.ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryPossibleUnintendedUseOfEqualsWarning));
			});
			webApplicationBuilder.Services.AddTransient<ICatalogBrandRepository, CatalogBrandRepository>();
			webApplicationBuilder.Services.AddTransient<ICatalogItemRepository, CatalogItemRepository>();
			webApplicationBuilder.Services.AddTransient<ICatalogTypeRepository, CatalogTypeRepository>();
			webApplicationBuilder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
		}

		private static void ConfigureHttpRequestPipeline(WebApplication webApplication) {
			if (webApplication.Environment.IsDevelopment()) {
				webApplication.UseSwagger();
				webApplication.UseSwaggerUI(options => {
					options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
					//options.RoutePrefix = string.Empty;
				});
			}

			webApplication.UseHttpsRedirection();
			webApplication.UseAuthorization();
			webApplication.MapControllers();
		}
	}
}