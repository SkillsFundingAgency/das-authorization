using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Authorization.ProviderPermissions.Types;

namespace SFA.DAS.Authorization.ProviderPermissions.Client
{
    public interface IProviderRelationshipsApiClient
    {
        Task<bool> HasPermission(HasPermissionRequest request, CancellationToken cancellationToken = default);
    }
}