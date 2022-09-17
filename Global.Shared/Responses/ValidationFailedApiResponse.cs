using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Global.Shared.Responses
{
    internal class ValidationFailedApiResponse : FailedApiResponse
    {
        public ValidationErrorCollection Errors { get; }

        public ValidationFailedApiResponse(ValidationErrorCollection errors, string message) : base(message)
        {
            Errors = errors;
        }
    }

    public class ValidationError
    {
        public string PropertyName { get; }

        [JsonPropertyName("messages")]
        public string[] Errors { get; }

        public int ErrorCount { get; }

        public ValidationError(string propertyName, string[] errors)
        {
            PropertyName = propertyName;
            Errors = errors;
            ErrorCount = errors.Length;
        }
    }

    public class ValidationErrorCollection : IReadOnlyCollection<ValidationError>
    {
        private readonly ValidationError[] _errors;

        public int Count => _errors.Length;

        public bool IsEmpty => Count == 0;

        public ValidationErrorCollection(params ValidationError[] erros)
        {
            _errors = erros;
        }

        public IEnumerator<ValidationError> GetEnumerator()
        {
            return _errors.ToList().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new System.NotImplementedException();
        }
    }
}
