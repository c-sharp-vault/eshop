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

namespace Catalog.API
{
    public class Program {
		public static async Task Main(string[] args) {
			WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
			AddServicesToContainer(builder);
			WebApplication app = builder.Build();
			ConfigureHttpRequestPipeline(app);
			AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
			await PrepareDatabase.MigrateAndSeedAsync(app);
			app.Run();
		}

		private static void AddServicesToContainer(WebApplicationBuilder builder) {
			builder.Services.AddOptions();
			builder.Services.AddLogging();
			// webApplicationBuilder.Services.Configure<CatalogOptions>(webApplicationBuilder.Configuration);
			builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

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
			builder.Services.AddAutoMapper(assemblies: AppDomain.CurrentDomain.GetAssemblies());
			builder.Services.AddRouting(opt => opt.LowercaseUrls = true);

			builder.Services.AddDbContext<CatalogContext>(options => {
				options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQLCatalogConnectionString"),
									 options => options.MigrationsAssembly("Catalog.DataAccess")
													   .EnableRetryOnFailure(
														   maxRetryCount: 6,
														   maxRetryDelay: TimeSpan.FromSeconds(30),
														   errorCodesToAdd: null));
				options.ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryPossibleUnintendedUseOfEqualsWarning));
			});
			builder.Services.AddTransient<ICatalogBrandRepository, CatalogBrandRepository>();
			builder.Services.AddTransient<ICatalogItemRepository, CatalogItemRepository>();
			builder.Services.AddTransient<ICatalogTypeRepository, CatalogTypeRepository>();
			builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
		}

		private static void ConfigureHttpRequestPipeline(WebApplication app) {
			if (app.Environment.IsDevelopment()) {
				app.UseSwagger();
				app.UseSwaggerUI(options => {
					options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
					//options.RoutePrefix = string.Empty;
				});
			}

			app.UseHttpsRedirection();
			app.UseAuthorization();
			app.MapControllers();
		}
	}
}