using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.Authorization
{
    public class AuthorizationResult
    {
        public bool IsAuthorized => !_errors.Any();
        public IEnumerable<AuthorizationError> Errors => _errors;

        private readonly List<AuthorizationError> _errors = new List<AuthorizationError>();

        public AuthorizationResult()
        {
        }

        public AuthorizationResult(AuthorizationError error)
        {
            _errors.Add(error);
        }

        public AuthorizationResult(IEnumerable<AuthorizationError> errors)
        {
            _errors.AddRange(errors);
        }

        public AuthorizationResult AddError(AuthorizationError error)
        {
            _errors.Add(error);

            return this;
        }

        public bool HasError<T>() where T : AuthorizationError
        {
            return _errors.OfType<T>().Any();
        }
    }
}