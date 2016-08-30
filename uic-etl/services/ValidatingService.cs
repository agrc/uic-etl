using System;
using System.Collections.Generic;
using domain.uic_etl.xml;
using FluentValidation;
using FluentValidation.Results;

namespace uic_etl.services
{
    public class ValidatingService
    {
        private readonly ConstituentDetailValidator _constituentValidator;
        private readonly ContactDetailValidator _contactValidator;
        private readonly CorrectionDetailValidator _correctionValidator;
        private readonly LocationDetailValidator _locationValidator;
        private readonly MiTestDetailValidator _miTestValidator;
        private readonly PermitActivityDetailValidator _permitActivityValidator;
        private readonly ResponseDetailValidator _responseValidator;
        private readonly ViolationDetailValidator _violationValidator;
        private readonly WellStatusDetailValidator _wellStatusValidator;
        private readonly WellTypeDetailValidator _wellTypeValidator;
        private readonly WellInspectionDetailValidator _wellInspectionValidator;
        private readonly EngineeringDetailValidator _engineeringDetailValidator;
        private readonly WasteDetailValidator _wasteDetailValidator;
        public readonly Dictionary<string, Dictionary<string, IEnumerable<ValidationFailure>>> Results;

        public ValidatingService()
        {
            Results = new Dictionary<string, Dictionary<string, IEnumerable<ValidationFailure>>>();
            _constituentValidator = new ConstituentDetailValidator();
            _contactValidator = new ContactDetailValidator();
            _correctionValidator = new CorrectionDetailValidator();
            _locationValidator = new LocationDetailValidator();
            _miTestValidator = new MiTestDetailValidator();
            _permitActivityValidator = new PermitActivityDetailValidator();
            _responseValidator = new ResponseDetailValidator();
            _violationValidator = new ViolationDetailValidator();
            _wellStatusValidator = new WellStatusDetailValidator();
            _wellTypeValidator = new WellTypeDetailValidator();
            _wellInspectionValidator = new WellInspectionDetailValidator();
            _engineeringDetailValidator = new EngineeringDetailValidator();
            _wasteDetailValidator = new WasteDetailValidator();
        }

        public bool IsValid<T>(T model)
        {
            AbstractValidator<T> validator = null;
            var key = "";
            var id = "";

            if (model is ConstituentDetail)
            {
                validator = _constituentValidator as AbstractValidator<T>;
                key = "ConstituentDetail";
                id = (model as ConstituentDetail).ConstituentIdentifier;
            }
            else if (model is ContactDetail)
            {
                validator = _contactValidator as AbstractValidator<T>;
                key = "ContactIdentifier";
                id = (model as ContactDetail).ContactIdentifier;
            }
            else if (model is CorrectionDetail)
            {
                validator = _correctionValidator as AbstractValidator<T>;
                key = "CorrectionDetail";
                id = (model as CorrectionDetail).CorrectionIdentifier;
            }
            else if (model is LocationDetail)
            {
                validator = _locationValidator as AbstractValidator<T>;
                key = "LocationDetail";
                id = (model as LocationDetail).LocationIdentifier;
            }
            else if (model is MiTestDetail)
            {
                validator = _miTestValidator as AbstractValidator<T>;
                key = "MiTestDetail";
                id = (model as MiTestDetail).MechanicalIntegrityTestIdentifier;
            }
            else if (model is PermitActivityDetail)
            {
                validator = _permitActivityValidator as AbstractValidator<T>;
                key = "PermitActivityDetail";
                id = (model as PermitActivityDetail).PermitActivityIdentifier;
            }
            else if (model is ResponseDetail)
            {
                validator = _responseValidator as AbstractValidator<T>;
                key = "ResponseDetail";
                id = (model as ResponseDetail).ResponseViolationIdentifier;
            }
            else if (model is ViolationDetail)
            {
                validator = _violationValidator as AbstractValidator<T>;
                key = "ViolationDetail";
                id = (model as ViolationDetail).ViolationIdentifier;
            }
            else if (model is WellStatusDetail)
            {
                validator = _wellStatusValidator as AbstractValidator<T>;
                key = "WellStatusDetail";
                id = (model as WellStatusDetail).WellStatusIdentifier;
            }
            else if (model is WellTypeDetail)
            {
                validator = _wellTypeValidator as AbstractValidator<T>;
                key = "WellTypeDetail";
                id = (model as WellTypeDetail).WellTypeIdentifier;
            }
            else if (model is WellInspectionDetail)
            {
                validator = _wellInspectionValidator as AbstractValidator<T>;
                key = "WellInspectionDetail";
                id = (model as WellInspectionDetail).InspectionIdentifier;
            }
           
            if (validator == null)
            {
                throw new ArgumentException("Model has special needs and needs to be validated outside of this service.", "model");
            }

            var result = validator.Validate(model, ruleSet: "R1");
            var conditionalResult = validator.Validate(model, ruleSet: "R1C");

            var valid = result.IsValid && conditionalResult.IsValid;

            var errors = new Dictionary<string, IEnumerable<ValidationFailure>>();
            errors["R1"] = result.Errors;
            errors["R1C"] = conditionalResult.Errors;

            foreach (var ruleSet in new []{"R2", "R2C"})
            {
                var warnings = validator.Validate(model, ruleSet: ruleSet);
                errors[ruleSet] = warnings.Errors;
            }

            Results[string.Format("{0} with the id: {1}", key, id)] = errors;

            return valid;
        }

        // use on conditionals the require branching in program
        public bool IsValid<T>(T model, string ruleSet)
        {
            AbstractValidator<T> validator = null;
            var key = "";
            var id = "";

            if (model is EngineeringDetail)
            {
                validator = _engineeringDetailValidator as AbstractValidator<T>;
                key = "EngineeringDetail";
                id = (model as EngineeringDetail).EngineeringIdentifier;
            }
            else if (model is WasteDetail)
            {
                validator = _wasteDetailValidator as AbstractValidator<T>;
                key = "WasteDetail";
                id = (model as WasteDetail).WasteIdentifier;
            }
            
            var result = validator.Validate(model, ruleSet: ruleSet);

            var valid = result.IsValid;

            var errors = new Dictionary<string, IEnumerable<ValidationFailure>>();
            errors[ruleSet] = result.Errors;

            Results[string.Format("{0} with the id: {1}", key, id)] = errors;

            return valid;
        }
    }
}