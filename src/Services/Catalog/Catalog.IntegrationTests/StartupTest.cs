using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Catalog.IntegrationTests {
	[Parallelizable(ParallelScope.Children | ParallelScope.Self)]
	internal class StartupTest {
		[Ignore("Need to setup probe health check code")]
		[Test]
		public async Task TestApplicationStarts() {
			var factory = new WebApplicationFactory();
			var client = factory.CreateClient();

			var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(5)).Token;
			var token = CancellationTokenSource.CreateLinkedTokenSource(timeout).Token;

			var response = await client.GetAsync("/probe/health", token);

			response.EnsureSuccessStatusCode();

			var content = await response.Content.ReadAsStringAsync(token);
			var result = JsonSerializer.Deserialize<ProbeResult>(content, new JsonSerializerOptions {
				PropertyNameCaseInsensitive = true
			});

			result.Status.Should().BeEquivalentTo("Healthy");
		}

		public class ProbeResult {
			public string Status { get; set; }
		}
	}
}
