using System.Collections.Generic;
using uic_etl.models.dtos;
using uic_etl.services;
using Xunit;

namespace tests.uic_etl
{
    public class AutoMapperServiceTests
    {
        [Fact]
        public void MappingsAreValid()
        {
            var mapper = AutoMapperService.CreateMappings(new Dictionary<string, IndexFieldMap>
            {
                {"GUID", new IndexFieldMap(0, "")},
                {"FacilityID", new IndexFieldMap(0, "")},
                {"FacilityName", new IndexFieldMap(0, "")},
                {"FacilityAddress", new IndexFieldMap(0, "")},
                {"FacilityCity", new IndexFieldMap(0, "")},
                {"FacilityState", new IndexFieldMap(0, "")},
                {"FacilityZip", new IndexFieldMap(0, "")},
                {"FacilityType", new IndexFieldMap(0, "")},
                {"NoMigrationPetStatus", new IndexFieldMap(0, "")}
            });

            mapper.ConfigurationProvider.AssertConfigurationIsValid();

            Assert.True(true);
        }
    }
}