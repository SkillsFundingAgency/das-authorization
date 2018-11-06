using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.Authorization
{
    public class AuthorizationResult
    {
        public bool IsAuthorized => !_errors.Any();
        public IEnumerable<AuthorizationError> Errors => _errors;

        private readonly ConcurrentBag<AuthorizationError> _errors = new ConcurrentBag<AuthorizationError>();

        public AuthorizationResult()
        {
        }

        public AuthorizationResult(AuthorizationError error)
        {
            _errors.Add(error);
        }

        public AuthorizationResult(IEnumerable<AuthorizationError> errors)
        {
            foreach( var error in errors)
                _errors.Add(error);
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