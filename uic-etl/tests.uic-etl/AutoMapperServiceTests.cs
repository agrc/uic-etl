﻿using uic_etl.services;
using Xunit;

namespace tests.uic_etl
{
    public class AutoMapperServiceTests
    {
        [Fact]
        public void MappingsAreValid()
        {
            var mapper = EtlMappingService.CreateMappings();

            mapper.ConfigurationProvider.AssertConfigurationIsValid();

            Assert.True(true);
        }
    }
}