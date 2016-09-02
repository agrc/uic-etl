using System.Collections.Generic;
using FluentValidation.Results;

namespace uic_etl.models.dtos
{
    public class LogModel
    {
        public LogModel(string key, string id, Dictionary<string, IEnumerable<ValidationFailure>> errors)
        {
            Identifier = string.Format("{0} with the id: {1}", key, id);
            Failures = errors;
        }

        public string Identifier { get; set; }
        public Dictionary<string, IEnumerable<ValidationFailure>> Failures { get; set; }
    }
}