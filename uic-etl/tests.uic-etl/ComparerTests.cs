using System.Collections.Generic;
using domain.uic_etl.xml;
using uic_etl.comparers;
using Xunit;

namespace tests.uic_etl
{
    public class ComparerTests
    {
        [Fact]
        public void HashSetWithReferenceObject()
        {
            var hash = new HashSet<ResponseDetail>(new ResponseDetailComparer());

            var responseOne = new ResponseDetail
            {
                EnforcementActionDate = "yyyyMMdd",
                EnforcementActionType = "INF",
                ResponseEnforcementIdentifier = "ID1",
                ResponseViolationIdentifier = "FID1"
            };

            hash.Add(responseOne);

            var responseTwo = new ResponseDetail
            {
                EnforcementActionDate = "yyyyMMdd",
                EnforcementActionType = "INF",
                ResponseEnforcementIdentifier = "ID1",
                ResponseViolationIdentifier = "FID1"
            };

            hash.Add(responseTwo);

            Assert.Equal(1, hash.Count);
        }
    }
}
