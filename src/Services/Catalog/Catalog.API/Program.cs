using Catalog.DataAccess;
using Catalog.Infrastructure.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebAPI {
	public class Program {
		public static void Main(string[] args) {
			WebApplicationBuilder webApplicationBuilder = WebApplication.CreateBuilder(args);
			AddServicesToContainer(webApplicationBuilder);
			WebApplication webApplication = webApplicationBuilder.Build();
			ConfigureHttpRequestPipeline(webApplication);
			webApplication.Run();
		}

		private static void AddServicesToContainer(WebApplicationBuilder webApplicationBuilder) {
			webApplicationBuilder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			webApplicationBuilder.Services.AddEndpointsApiExplorer();
			webApplicationBuilder.Services.AddSwaggerGen();
			webApplicationBuilder.Services.Configure<CatalogOptions>(
				webApplicationBuilder.Configuration.GetSection(CatalogOptions.KEY));
			webApplicationBuilder.Services.AddDbContext<CatalogContext>(options => {
				options.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("CatalogConnectionString"));
			});
		}

		private static void ConfigureHttpRequestPipeline(WebApplication webApplication) {
			if (webApplication.Environment.IsDevelopment()) {
				webApplication.UseSwagger();
				webApplication.UseSwaggerUI(options => {
					options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
					options.RoutePrefix = string.Empty;
				});
			}

			webApplication.UseHttpsRedirection();
			webApplication.UseAuthorization();
			webApplication.MapControllers();
		}
	}
}