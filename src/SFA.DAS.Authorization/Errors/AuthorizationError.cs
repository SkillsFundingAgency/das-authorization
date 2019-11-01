namespace SFA.DAS.Authorization.Errors
{
    public abstract class AuthorizationError
    {
        public string Message { get; }

        protected AuthorizationError(string message)
        {
            Message = message;
        }
    }    
}