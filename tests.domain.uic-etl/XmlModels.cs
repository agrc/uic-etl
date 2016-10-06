using domain.uic_etl.xml;
using Xunit;

namespace tests.domain.uic_etl
{
    public class XmlModels
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
}
