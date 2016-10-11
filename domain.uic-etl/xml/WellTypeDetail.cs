using System.Collections.Generic;
using System.Linq;
using FluentValidation;

namespace domain.uic_etl.xml
{
    public class WellTypeDetail
    {
        private string _wellTypeCode;
        private int _wellClass;
        public string WellTypeIdentifier { get; set; }
        public string WellTypeCode
        {
            get
            {
                switch (_wellTypeCode)
                {
                    case "1000":
                        {
                            return "1I";
                        }
                    case "1001":
                        {
                            return "1H";
                        }
                    case "1002":
                        {
                            return "1M";
                        }
                    case "1003":
                        {
                            return "1R";
                        }
                    case "1999":
                        {
                            return "1X";
                        }
                    case "3000":
                        {
                            return "3C";
                        }
                    case "3001":
                        {
                            return "3N";
                        }
                    case "3002":
                        {
                            return "3A";
                        }
                    case "3003":
                        {
                            return "3S";
                        }
                    case "3004":
                        {
                            return "3U";
                        }
                    case "3999":
                        {
                            return "3X";
                        }
                    case "4000":
                        {
                            return "4P";
                        }
                    case "4001":
                    case "4002":
                        {
                            return "4A";
                        }
                    case "5000":
                        {
                            return "5H2";
                        }
                    case "5001":
                        {
                            return "5B1";
                        }
                    case "5002":
                        {
                            return "5B6";
                        }
                    case "5003":
                        {
                            return "5C3";
                        }
                    case "5004":
                        {
                            return "5D";
                        }
                    case "5005":
                        {
                            return "5C4";
                        }
                    case "5006":
                        {
                            return "5G";
                        }
                    case "5007":
                        {
                            return "5C5";
                        }
                    case "5008":
                        {
                            return "5C2";
                        }
                    case "5010":
                        {
                            return "5A";
                        }
                    case "5011":
                        {
                            return "5A25";
                        }
                    case "5012":
                        {
                            return "5A1";
                        }
                    case "5013":
                        {
                            return "5A2";
                        }
                    case "5014":
                        {
                            return "5A3";
                        }
                    case "5015":
                        {
                            return "5A4";
                        }
                    case "5016":
                        {
                            return "5A5";
                        }
                    case "5017":
                        {
                            return "5A6";
                        }
                    case "5018":
                        {
                            return "5A7";
                        }
                    case "5019":
                        {
                            return "5A8";
                        }
                    case "5020":
                        {
                            return "5A9";
                        }
                    case "5021":
                        {
                            return "5A10";
                        }
                    case "5022":
                        {
                            return "5A11";
                        }
                    case "5023":
                        {
                            return "5A12";
                        }
                    case "5024":
                        {
                            return "5A13";
                        }
                    case "5025":
                        {
                            return "5A14";
                        }
                    case "5026":
                        {
                            return "5A15";
                        }
                    case "5027":
                        {
                            return "5A16";
                        }
                    case "5028":
                        {
                            return "5A17";
                        }
                    case "5029":
                        {
                            return "5A18";
                        }
                    case "5030":
                        {
                            return "5A19";
                        }
                    case "5031":
                        {
                            return "5A20";
                        }
                    case "5032":
                        {
                            return "5A21";
                        }
                    case "5034":
                        {
                            return "5A23";
                        }
                    case "5035":
                        {
                            switch (_wellClass)
                            {
                                case 3:
                                    return "3G";
                                case 5:
                                    return "5L2";
                            }

                            return _wellTypeCode;
                        }
                    case "5037":
                        {
                            return "5I";
                        }
                    case "5038":
                        {
                            return "5K";
                        }
                    case "5040":
                        {
                            return "5B2";
                        }
                    case "5041":
                        {
                            return "5F";
                        }
                    case "5042":
                        {
                            return "5F";
                        }
                    case "5043":
                        {
                            return "5F";
                        }
                    case "5044":
                        {
                            return "5L1";
                        }
                    case "5045":
                        {
                            return "5H3";
                        }
                    case "5046":
                        {
                            return "5C1";
                        }
                    case "5047":
                        {
                            return "5H1";
                        }
                    case "5048":
                        {
                            return "5B3";
                        }
                    case "5050":
                        {
                            return "5B4";
                        }
                    case "5100":
                        {
                            return "5E";
                        }
                    case "5101":
                        {
                            return "5F";
                        }
                    case "5998":
                        {
                            return "5G2";
                        }
                    case "5999":
                        {
                            return "5X";
                        }
                    case "6001":
                        {
                            return "6A";
                        }
                    case "6002":
                        {
                            return "6B";
                        }
                    default:
                        {
                            return _wellTypeCode;
                        }
                }
            }
            set { _wellTypeCode = value; }
        }
        public string WellTypeDate { get; set; }
        public string WellTypeWellIdentifier { get; set; }

        public int WellClass
        {
            set { _wellClass = value; }
        }
    }

    public class WellTypeDetailValidator : AbstractValidator<WellTypeDetail>
    {
        private readonly IEnumerable<string> _wellTypes;

        public WellTypeDetailValidator()
        {
            var codeOne = new[] {"1H", "1I", "1M", "1R", "1W", "1X"};
            var codeTwo = new[] {"2A", "2D", "2F", "2H", "2M", "2R", "2C", "2X"};
            var codeThree = new[] {"3A", "3G", "3U", "3S", "3C", "3N", "3X"};
            var codeFour = new[] {"4A", "4P"};
            var codeFive = new[] {"5A", "5B", "5C", "5D", "5E", "5F", "5G", "5H", "5I", "5K", "5L", "5X"};
            var codeSix = new[] {"6A", "6B"};

            _wellTypes = codeOne.Concat(codeTwo)
                .Concat(codeThree)
                .Concat(codeFour)
                .Concat(codeFive)
                .Concat(codeSix);

            RuleSet("R1", () =>
            {
                RuleFor(src => src.WellTypeIdentifier)
                    .NotEmpty()
                    .Length(20);

                RuleFor(src => src.WellTypeCode)
                    .NotEmpty()
                    .Length(1, 4)
                    .Matches("^[1-5]")
                    .Must(code => _wellTypes.Any(code.StartsWith))
                    .Unless(src => src.WellTypeCode == "CV");

                RuleFor(src => src.WellTypeDate)
                    .NotEmpty()
                    .Length(8)
                    .Matches(@"\d{8}");

                RuleFor(src => src.WellTypeWellIdentifier)
                    .NotEmpty()
                    .Length(20);
            });
        }
    }
}