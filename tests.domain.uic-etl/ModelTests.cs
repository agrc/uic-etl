using domain.uic_etl.sde;
using domain.uic_etl.xml;
using Xunit;

namespace tests.domain.uic_etl
{
    public class ModelTests
    {
        [Fact]
        public void ContactTelephoneStripsExtensions()
        {
            var contact = new ContactDetail
            {
                TelephoneNumberText = "435.471.2209 x35"
            };

            Assert.Equal("435.471.2209", contact.TelephoneNumberText);

            contact.TelephoneNumberText = "801.972.4587X1363";

            Assert.Equal("801.972.4587", contact.TelephoneNumberText);

            contact.TelephoneNumberText = "801.972.4587";

            Assert.Equal("801.972.4587", contact.TelephoneNumberText);

            contact.TelephoneNumberText = null;

            Assert.Null(contact.TelephoneNumberText);

            contact.TelephoneNumberText = string.Empty;

            Assert.Empty(contact.TelephoneNumberText);
        }
    }

    public class SdeModels
    {
        [Fact]
        public void SeeMilepostFacilityAddressShouldGetMilepostValue()
        {
            const string milepost = "Using milepost";
            var facility = new FacilitySdeModel
            {
                FacilityAddress = "See Facility Mile Post",
                FacilityMilePost = milepost
            };

            Assert.Equal(facility.FacilityAddress, milepost);
        }

        [Fact]
        public void EmptyFacilityAddressShouldGetMilepostValue()
        {
            const string milepost = "Using milepost";
            var facility = new FacilitySdeModel
            {
                FacilityAddress = "See Facility Mile Post",
                FacilityMilePost = milepost
            };

            Assert.Equal(facility.FacilityAddress, milepost);
        }

        [Fact]
        public void FacilityAddressShouldAddressValue()
        {
            const string milepost = "Using milepost";
            const string address = "Using address";
            var facility = new FacilitySdeModel
            {
                FacilityAddress = address,
                FacilityMilePost = milepost
            };

            Assert.Equal(facility.FacilityAddress, address);
        }

        [Fact]
        public void NullFacilityAddressAndMilepostShouldBeNull()
        {
            const string milepost = null;
            const string address = null;
            var facility = new FacilitySdeModel
            {
                FacilityAddress = address,
                FacilityMilePost = milepost
            };

            Assert.Null(facility.FacilityAddress);
        }

        [Fact]
        public void EmptyFacilityAddressAndMilepostShouldBeEmpty()
        {
            const string milepost = "";
            const string address = "";
            var facility = new FacilitySdeModel
            {
                FacilityAddress = address,
                FacilityMilePost = milepost
            };

            Assert.Empty(facility.FacilityAddress);
        }

        [Fact]
        public void VerticalWellEventContainsDecimal()
        {
            var depth = new VerticalWellEventSdeModel
            {
                Length = "20"
            };

            Assert.Equal("20.0", depth.Length);

            depth.Length = "20.1";

            Assert.Equal("20.0", depth.Length);
        }
    }
}
