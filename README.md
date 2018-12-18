# SFA.DAS.Authorization

This package includes:

* Facade to aggregate multiple authorization concerns into a single call including:
  * Employer features - Toggling, toggling by user whitelisting, toggling by agreement signing.
  * Employer roles - User membership checks for an account, user role checks for an account.
  * Provider permissions - Provider permission checks for an organisation.
* Cross cutting authorization infrastructure for Mvc and WebApi.
* Model binding infrastructure for Mvc and WebApi.
* Html helper extensions for Mvc.

## Configuration

In addition to the `SFA.DAS.Authorization` package one or more of the following packages should be referenced depending on your application's requirements:

* `SFA.DAS.Authorization.EmployerFeatures`
* `SFA.DAS.Authorization.EmployerRoles`
* `SFA.DAS.Authorization.ProviderPermissions`

### StructureMap

The authorization packages include StructureMap registries for wiring up their components:

```c#
c.AddRegistry<AuthorizationRegistry>();
c.AddRegistry<EmployerFeaturesAuthorizationRegistry>();
c.AddRegistry<EmployerRolesAuthorizationRegistry>();
c.AddRegistry<ProviderPermissionsAuthorizationRegistry>();
```

### MVC

```c#
binders.UseAuthorizationModelBinder();
filters.AddAuthorizationFilter();
filters.AddUnauthorizedAccessExceptionFilter();
```

### WebApi

```c#
config.Services.UseAuthorizationModelBinder();
config.Filters.AddAuthorizationFilter();
config.Filters.AddUnauthorizedAccessExceptionFilter();
```

### Table Storage

All of the `SFA.DAS.Authorization` packages bootstrap their own configuration from table storage using the `SFA.DAS.AutoConfiguration` package except for `SFA.DAS.Authorization.EmployerFeatures` which requires an instance of `SFA.DAS.Authorization.EmployerFeatures.EmployerFeaturesConfiguration` registering in your application's container.

If you're looking to deserialize an instance of `EmployerFeaturesConfiguration` from table storage and then register it in your container the JSON should look similar to the following: 

```json
{
    "FeatureToggles": [{
        "Feature": "ProviderRelationships",
        "IsEnabled": true,
        "Whitelist": [{
            "AccountId": 111111111,
            "UserEmails": ["foo1@foo.com", "foo2@foo.com"]
        }, {
           "AccountId": 222222222,
           "UserEmails": ["bar1@bar.com", "bar2@bar.com"]
        }]
    }]
}
```

### Authorization context

Each authorization package needs to know the context of the current operation it's running in for it to be able to do any authorization checks. For example to check if a user has access to an account then the user's ID and the account's ID are needed. For your application to be able to to provide this context to the authorization package a custom implementation of `IAuthorizationContextProvider` will need registering in your application's container:

```c#
public class AuthorizationContextProvider : IAuthorizationContextProvider
{
    public IAuthorizationContext GetAuthorizationContext()
    {
        var authorizationContext = new AuthorizationContext();
        var accountId = 123456; // e.g. From the URL querystring
        var accountLegalEntityId = 1; // e.g. From the URL querystring
        var ukprn = 12345678 // e.g. From the URL querystring
        var userRef = "abcdef" // e.g. From the authentication claims
        var userEmail = "foo@bar.com" // e.g. From the authentication claims
        
        authorizationContext.AddEmployerFeatureValues(accountId, userEmail);
        authorizationContext.AddEmployerRoleValues(accountId, userRef);
        authorizationContext.AddProviderPermissionValues(accountLegalEntityId, ukprn);

        return authorizationContext;
    }
}
```

## Usage

The `IAuthorizationService` interface can be used to check users' authorization. Its signature currently looks like:

```c#
public interface IAuthorizationService
{
    void Authorize(params string[] options);
    Task AuthorizeAsync(params string[] options);
    AuthorizationResult GetAuthorizationResult(params string[] options);
    Task<AuthorizationResult> GetAuthorizationResultAsync(params string[] options);
    bool IsAuthorized(params string[] options);
    Task<bool> IsAuthorizedAsync(params string[] options);
}
```

The options that can be included in an authorization check will depend on which authorization packages have been referenced from your application:

### SFA.DAS.Authorization.EmployerFeatures

To check if a feature is enabled:

```c#
var isAuthorized = _authorizationService.IsAuthorized(EmployerFeatures.ProviderRelationships);
```

Alternatively, if you're interested in why an authorization check has failed:

```c#
var authorizationResult = _authorizationService.GetAuthorizationResult(EmployerFeatures.ProviderRelationships);

if (!authorizationResult.IsAuthorized)
{
    if (authorizationResult.HasError<EmployerFeatureNotEnabled>())
    {
        // Handle feature not enabled
    }
    else if (authorizationResult.HasError<EmployerFeatureUserNotWhitelisted>())
    {
        // Handle user not whitelisted
    }
    else if (authorizationResult.HasError<EmployerFeatureAgreementNotSigned>())
    {
        // Handle agreement not signed
    }
}
```

> `AccountId` & `UserEmail` authorization context values are required for this package.

### SFA.DAS.Authorization.EmployerRoles

To check if a user has the required role:

```c#
var isAuthorized = _authorizationService.IsAuthorized(EmployerRoles.Owner);
```

Alternatively, if you're interested in why an authorization check has failed:

```c#
var authorizationResult = _authorizationService.GetAuthorizationResult(EmployerRoles.Owner);

if (!authorizationResult.IsAuthorized)
{
    if (authorizationResult.HasError<EmployerRoleNotAuthorized>())
    {
        // Handle role not authorized
    }
}
```

> `AccountId` & `UserRef` authorization context values are required for this package.

### SFA.DAS.Authorization.ProviderPermissions

To check if a provider has the required permission:

```c#
var isAuthorized = _authorizationService.IsAuthorized(ProviderPermissions.CreateCohort);
```

Alternatively, if you're interested in why an authorization check has failed:

```c#
var authorizationResult = _authorizationService.GetAuthorizationResult(ProviderPermissions.CreateCohort);

if (!authorizationResult.IsAuthorized)
{
    if (authorizationResult.HasError<ProviderPermissionNotGranted>())
    {
        // Handle permission not granted
    }
}
```

> `AccountLegalEntityId` & `Ukprn` authorization context values are required for this package.

### MVC

The `DasAuthorizeAttribute` attribute can be used to check users' authorization. It inherits from the default `System.Web.Mvc.AuthorizeAttribute` attribute and so can replace any current usages to ensure the current user is authenticated:

```c#
[DasAuthorize]
[RoutePrefix("legalentities")]
public class LegalEntitiesController : Controller
{
    [Route]
    public ActionResult Index()
    {
        return View();
    }
    
    [DasAuthorize(EmployerRoles.Owner)]
    [Route("add")]
    public ActionResult Add()
    {
        return View(new AddLegalEntityViewModel());
    }

    [HttpPost]
    [DasAuthorize(EmployerRoles.Owner)]
    [Route("add")]
    public ActionResult Add(AddLegalEntityViewModel model)
    {
        return RedirectToAction("Index");
    }
}
```

By adding the `IAuthorizationContextModel` marker interface to a controller action's model then any properties on the model of which a corresponding property can be found in the authorization context will be set:

```c#
public class AddLegalEntityViewModel: IAuthorizationContextModel
{
    public long AccountId { get; set; }
    public string UserRef { get; set; }
}
```

Html helpers can also be used that includes the synchronous methods from `IAuthorizationService`:

```razor
if (@Html.IsAuthorized(EmployerRoles.Owner))
{
    <a href="@Url.Action("Add", "LegalEntities")">Add legal entity</a>
}
```

### WebApi

The `DasAuthorizeAttribute` attribute can be used to check users' authorization. It inherits from the default `System.Web.Http.AuthorizeAttribute` attribute and so can replace any current usages to ensure the current user is authenticated:

```c#
[DasAuthorize]
[RoutePrefix("legalentities")]
public class LegalEntitiesController : ApiController
{
    [Route]
    public IHttpActionResult GetAll()
    {
        return Ok();
    }

    [DasAuthorize(EmployerRoles.Owner)]
    [Route]
    public IHttpActionResult Post(PostLegalEntityModel model)
    {
        return Ok();
    }
}
```

By adding the `IAuthorizationContextModel` marker interface to a controller action's model then any properties on the model of which a corresponding property can be found in the authorization context will be set:

```c#
public class PostLegalEntityModel: IAuthorizationContextModel
{
    public long AccountId { get; set; }
    public string UserRef { get; set; }
}
```