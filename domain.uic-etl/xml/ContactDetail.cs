using System;
using FluentValidation;

namespace domain.uic_etl.xml
{
    public class ContactDetail
    {
        private string _telephoneNumberText;
        public string ContactIdentifier { get; set; }

        public string TelephoneNumberText
        {
            get
            {
                if (string.IsNullOrEmpty(_telephoneNumberText))
                {
                    return _telephoneNumberText;
                }

                _telephoneNumberText = _telephoneNumberText.Replace(".", "");
               
                return _telephoneNumberText;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _telephoneNumberText = value;
                    return;
                }

                _telephoneNumberText = value.Replace(" ", "");
            }
        }

        public string IndividualFullName { get; set; }
        public string ContactCityName { get; set; }
        public string ContactAddressStateCode { get; set; }
        public string ContactAddressText { get; set; }
        public string ContactAddressPostalCode { get; set; }
    }

    public class ContactDetailValidator : AbstractValidator<ContactDetail>
    {
        public ContactDetailValidator()
        {
            RuleSet("R1", () =>
            {
                RuleFor(src => src.ContactIdentifier)
                    .NotNull()
                    .Length(20);

                RuleFor(src => src.IndividualFullName)
                    .NotEmpty()
                    .Length(1, 70);

                RuleFor(src => src.ContactAddressText)
                    .NotEmpty()
                    .Length(1, 150);
            });

            RuleSet("R2", () =>
            {
                RuleFor(src => src.TelephoneNumberText)
                    .Length(0, 15)
                    .Unless(src => string.IsNullOrEmpty(src.TelephoneNumberText));

                RuleFor(src => src.ContactAddressPostalCode)
                    .Length(5, 14)
                    .Unless(src => string.IsNullOrEmpty(src.ContactAddressPostalCode));

                RuleFor(src => src.ContactCityName)
                    .NotEmpty()
                    .Length(1, 30);

                RuleFor(src => src.ContactAddressStateCode)
                    .NotEmpty()
                    .Length(1, 50);
            });
        }
    }
}