
using Catalog.DataAccess.DTOs;
using Catalog.DataAccess.Managers;
using Catalog.IntegrationTests.Initialization;
using Catalog.IntegrationTests.Services;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.IntegrationTests.Services {
	public class APIEndpointTestBase : IDisposable, IAsyncDisposable {
		private readonly WebApplicationFactory _webApplicationFactory;
		private readonly HttpClient _httpClient;

		public APIEndpointTestBase() {
			_webApplicationFactory = new WebApplicationFactory();
			_httpClient = _webApplicationFactory.CreateClient();
		}

		HttpClient HTTPClient => _httpClient;

		static CancellationToken GetCancellationToken(int timeoutSeconds = 5) =>
			CancellationTokenSource.CreateLinkedTokenSource(new CancellationTokenSource(TimeSpan.FromSeconds(timeoutSeconds)).Token).Token;

		static TResponse ToResponse<TResponse>(string json) =>
			JsonSerializer.Deserialize<TResponse>(json, new JsonSerializerOptions() {
				PropertyNameCaseInsensitive = true,
				IncludeFields = true,
				MaxDepth = 10
			});

		static string ToJson<TRequest>(TRequest request) where TRequest : EntityTypeDTO =>
			JsonSerializer.Serialize(request);

		protected async Task<TResponse> GETAsync<TResponse>(string endpoint, string parameters, bool ensureSuccess = true, HttpStatusCode expectedStatusCode = HttpStatusCode.OK)
			where TResponse : class {
			var response = await HTTPClient.GetAsync(string.Concat(endpoint, parameters));

			if (ensureSuccess) response.EnsureSuccessStatusCode();

			Assert.That(response.StatusCode, Is.EqualTo(expectedStatusCode));

			var responseString = await response.Content.ReadAsStringAsync(GetCancellationToken());

			return ToResponse<TResponse>(responseString);
		}

		protected async Task<TResponse> POSTAsync<TRequest, TResponse>(string endpoint, TRequest request, bool ensureSuccess = true, HttpStatusCode expectedStatusCode = HttpStatusCode.OK)
			where TRequest : EntityTypeDTO
			where TResponse : class {
			var jsonRequest = ToJson<TRequest>(request);

			var response = await HTTPClient.PostAsync(endpoint, new StringContent(content: jsonRequest, encoding: Encoding.UTF8, mediaType: "application/json"));

			if (ensureSuccess) response.EnsureSuccessStatusCode();

			Assert.That(response.StatusCode, Is.EqualTo(expectedStatusCode));

			var responseString = await response.Content.ReadAsStringAsync(GetCancellationToken());

			return ToResponse<TResponse>(responseString);
		}

		protected async Task<TResponse> PUTAsync<TRequest, TResponse>(string endpoint, TRequest request, bool ensureSuccess = true, HttpStatusCode expectedStatusCode = HttpStatusCode.OK)
			where TRequest : EntityTypeDTO
			where TResponse : class {
			var jsonRequest = ToJson<TRequest>(request);

			var response = await HTTPClient.PutAsync(endpoint, new StringContent(content: jsonRequest, encoding: Encoding.UTF8, mediaType: "application/json"));

			if (ensureSuccess) response.EnsureSuccessStatusCode();

			Assert.That(response.StatusCode, Is.EqualTo(expectedStatusCode));

			var responseString = await response.Content.ReadAsStringAsync(GetCancellationToken());

			return ToResponse<TResponse>(responseString);
		}

		protected async Task<TResponse> DELETEAsync<TResponse>(string endpoint, string parameters, bool ensureSuccess = true, HttpStatusCode expectedStatusCode = HttpStatusCode.OK)
			where TResponse : class {
			var response = await HTTPClient.DeleteAsync(string.Concat(endpoint, parameters));

			if (ensureSuccess) response.EnsureSuccessStatusCode();

			Assert.That(response.StatusCode, Is.EqualTo(expectedStatusCode));

			var responseString = await response.Content.ReadAsStringAsync(GetCancellationToken());

			return ToResponse<TResponse>(responseString);
		}

		public void Dispose() => _webApplicationFactory.Dispose();

		public ValueTask DisposeAsync() => _webApplicationFactory.DisposeAsync();
	} 
}

public abstract class APIEndpointTestBase<T> : APIEndpointTestBase where T : IHaveTestData, new() {
	[OneTimeSetUp] 
	public void OneTimeSetUp() { 
		new T().SetupTestData();
	}

	[OneTimeTearDown]
	public void OneTimeTearDown() {
		new T().ClearData();
	}
}