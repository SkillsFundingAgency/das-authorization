using System.Threading.Tasks;

namespace SFA.DAS.Authorization
{
    public interface IAuthorizationHandler
    {
        Task<AuthorizationResult> GetAuthorizationResultAsync(string[] options);
    }
}