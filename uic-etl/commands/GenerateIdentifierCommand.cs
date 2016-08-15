using System;

namespace uic_etl.commands
{
    /// <summary>
    /// Given a guid returns the PrimacyCode + the last 15 of the guid
    /// </summary>
    public class GenerateIdentifierCommand
    {
        private readonly Guid _guid;
        private const string PrimacyCode = "UTEQ";

        public GenerateIdentifierCommand(Guid guid)
        {
            _guid = guid;
        }

        public string Execute()
        {
            return string.Format("{0}{1}", PrimacyCode, _guid.ToString("N").Substring(16, 16));
        } 
    }
}