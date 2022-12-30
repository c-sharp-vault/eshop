using AutoMapper;
using Catalog.DataAccess.DTOs.CatalogItem;
using Catalog.API.Profiles;
using Catalog.Core.Models;
using NUnit.Framework;
using System;
using System.Linq.Expressions;

namespace Catalog.UnitTests.Catalog.API.AutoMapper {
	public class AutoMapperTests {
		private MapperConfiguration _mapperConfiguration;

		[SetUp]
		public void SetUp() {
			this._mapperConfiguration = new MapperConfiguration(configure => configure.AddProfile<CatalogItemProfile>());
			//LambdaExpression executionPlan = this._mapperConfiguration.BuildExecutionPlan(typeof(CatalogItem), typeof(CatalogItemReadDTO));
		}

		[TearDown]
		public void TearDown() {
			this._mapperConfiguration = null;
			GC.Collect();
		}

		[Test]
		public void TestAutoMapperConfigurations() {
			// Arrange
			TestDelegate testDelegate = () => this._mapperConfiguration.AssertConfigurationIsValid();

			// Assert
			Assert.DoesNotThrow(testDelegate, "AssertConfigurationIsValid shouldn't throw an exception");
		}
	}
}
