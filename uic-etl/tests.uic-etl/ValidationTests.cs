using FluentValidation;
using Xunit;

namespace tests.uic_etl
{
    public class ValidationTests
    {
        public class Test
        {
            public string Validation { get; set; }
        }

        public class TestValidator : AbstractValidator<Test>
        {
            public TestValidator()
            {
                RuleSet("here", () =>
                {
                    RuleFor(x => x.Validation).NotEmpty();
                });
            }
        }

        [Fact]
        public void ReturnTrueWhenRuleSetIsMissing()
        {
            var validator = new TestValidator();
            var response = validator.Validate(new Test(), ruleSet: "notHere");

            Assert.True(response.IsValid);
        }

        [Fact]
        public void ReturnFalse()
        {
            var validator = new TestValidator();
            var response = validator.Validate(new Test(), ruleSet: "here");

            Assert.False(response.IsValid);
        }

        [Fact]
        public void ReturnTrue()
        {
            var validator = new TestValidator();
            var response = validator.Validate(new Test {Validation = "valid"}, ruleSet: "here");

            Assert.True(response.IsValid);
        }
    }
}