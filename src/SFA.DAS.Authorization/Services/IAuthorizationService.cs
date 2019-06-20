using System.Threading.Tasks;
using SFA.DAS.Authorization.Results;

namespace SFA.DAS.Authorization.Services
{
    public interface IAuthorizationService
    {
        void Authorize(params string[] options);
        Task AuthorizeAsync(params string[] options);
        AuthorizationResult GetAuthorizationResult(params string[] options);
        Task<AuthorizationResult> GetAuthorizationResultAsync(params string[] options);
        bool IsAuthorized(params string[] options);
        Task<bool> IsAuthorizedAsync(params string[] options);
    }
}