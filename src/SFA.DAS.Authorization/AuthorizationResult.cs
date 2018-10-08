using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.Authorization
{
    public class AuthorizationResult
    {
        public bool IsAuthorized => !Errors.Any();
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
    }
}