# SFA.DAS.Authorization

[![Build status](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_build?definitionId=1231&metric=alert_status)](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_build?definitionId=1231&metric=alert_status)
[![NuGet Badge](https://img.shields.io/nuget/v/SFA.DAS.Authorization.svg)](https://img.shields.io/nuget/v/SFA.DAS.Authorization.svg/)
[![License](https://img.shields.io/badge/license-MIT-lightgrey.svg?longCache=true&style=flat-square)](https://en.wikipedia.org/wiki/MIT_License)

The SFA.DAS.Authorization package provides a way to aggregate multiple authorization concerns into a single call. It also provides cross cutting authorization infrastructure for Mvc and WebApi.

The project supports various configurations depending on the application's requirements, including:
    * MVC Core: Integration with ASP.NET Core MVC.
    * MVC: Integration with traditional ASP.NET MVC.
    * WebApi: Integration with ASP.NET WebApi.
    * Dependency Injection: Extensions for Microsoft Dependency Injection and StructureMap.

This package includes:

* Facade to aggregate multiple authorization concerns into a single call including:
  * Commitment permissions - Commitment permission checks for an employer or provider.
  * Employer features - Toggling, toggling by account whitelisting, toggling by user whitelisting, toggling by agreement signing.
  * Employer user roles - User membership checks for an account, user role checks for an account.
  * Features - Toggling.
  * Provider features - Toggling, toggling by provider whitelisting, toggling by user whitelisting.
  * Provider permissions - Provider permission checks for an organisation.
* Cross cutting authorization infrastructure for Mvc and WebApi.
* Model binding infrastructure for Mvc and WebApi.

## Installation

### Pre-Requisites
* A clone of this repository(https://github.com/SkillsFundingAgency/das-authorization)
* A code editor that supports .Net standard 2.0 & .Net4.5 and above
* A storage emulator like Azurite (https://learn.microsoft.com/en-us/azure/storage/common/storage-use-emulator)
* An Azure Active Directory account with the appropriate roles as per the [das-employer-config repository](https://github.com/SkillsFundingAgency/das-employer-config/blob/master)

## Integration:

The project includes attributes like DasAuthorizeAttribute for MVC Core, MVC, and WebApi to ensure user authentication and authorization at the controller level. It also supports model binding to automatically set authorization context values.

## Configuration

In addition to the `SFA.DAS.Authorization` package one or more of the following packages should be referenced depending on your application's requirements:

* `SFA.DAS.Authorization.CommitmentPermissions`
* `SFA.DAS.Authorization.EmployerFeatures`
* `SFA.DAS.Authorization.EmployerUserRoles`
* `SFA.DAS.Authorization.Features`
* `SFA.DAS.Authorization.ProviderFeatures`
* `SFA.DAS.Authorization.ProviderPermissions`

### MVC Core

```c#
services.AddAuthorization<AuthorizationContextProvider>();
services.AddMvc(o => o.AddAuthorization());
app.UseUnauthorizedAccessExceptionHandler();
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

### Microsoft Dependency Injection

If you're using Microsoft Dependency Injection then the authorization packages also include `IServiceCollection` extensions for wiring up their components:

```c#
services.AddCommitmentPermissionsAuthorization();
services.AddEmployerFeaturesAuthorization();
services.AddFeaturesAuthorization();
services.AddProviderFeaturesAuthorization();
services.AddProviderPermissionsAuthorization();
```

> Please note, the `SFA.DAS.Authorization.EmployerUserRoles` package doesn't currently include .NET Core DI `IServiceCollection` extensions.

### StructureMap

If you're using StructureMap then the authorization packages also include registries for wiring up their components:

```c#
c.AddRegistry<AuthorizationRegistry>();
c.AddRegistry<CommitmentPermissionsAuthorizationRegistry>();
c.AddRegistry<EmployerFeaturesAuthorizationRegistry>();
c.AddRegistry<EmployerUserRolesAuthorizationRegistry>();
c.AddRegistry<FeaturesAuthorizationRegistry>();
c.AddRegistry<ProviderFeaturesAuthorizationRegistry>();
c.AddRegistry<ProviderPermissionsAuthorizationRegistry>();
```

### SFA.DAS.Authorization.CommitmentPermissions

`SFA.DAS.Authorization.CommitmentPermissions` requires an instance of `SFA.DAS.Authorization.CommitmentPermissions.Configuration.CommitmentPermissionsApiClientConfiguration` registering in your application's container.

### SFA.DAS.Authorization.Features

`SFA.DAS.Authorization.Features` requires an instance of `SFA.DAS.Authorization.Features.Configuration.FeaturesConfiguration` registering in your application's container. If you're looking to deserialize an instance of `FeaturesConfiguration` from table storage and then register it in your container the JSON should look similar to the following: 

```json
{
    "FeatureToggles": [{
        "Feature": "ProviderRelationships",
        "IsEnabled": true
    }]
}
```

### SFA.DAS.Authorization.EmployerFeatures

`SFA.DAS.Authorization.EmployerFeatures` requires an instance of `SFA.DAS.Authorization.EmployerFeatures.Configuration.EmployerFeaturesConfiguration` registering in your application's container. If you're looking to deserialize an instance of `EmployerFeaturesConfiguration` from table storage and then register it in your container the JSON should look similar to the following: 

```json
{
    "FeatureToggles": [{
        "Feature": "ProviderRelationships",
        "IsEnabled": true,
      	"AccountWhitelist": [11111,22222],
	"EmailWhitelist" : ["foo1@foo.com", "foo2@foo.com"]
    }]
}
```

### SFA.DAS.Authorization.ProviderFeatures

`SFA.DAS.Authorization.ProviderFeatures` requires an instance of `SFA.DAS.Authorization.ProviderFeatures.Configuration.ProviderFeaturesConfiguration` registering in your application's container. If you're looking to deserialize an instance of `ProviderFeaturesConfiguration` from table storage and then register it in your container the JSON should look similar to the following: 

```json
{
    "FeatureToggles": [{
        "Feature": "ProviderRelationships",
        "IsEnabled": true,
        "Whitelist": [{
            "Ukprn": 111111111,
            "UserEmails": ["foo1@foo.com", "foo2@foo.com"]
        }, {
            "Ukprn": 222222222,
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
        var userRef = "abcdef" // e.g. From the authentication claims
        var userEmail = "foo@bar.com" // e.g. From the authentication claims
        
        authorizationContext.AddEmployerFeatureValues(accountId, userEmail);
        authorizationContext.AddEmployerUserRoleValues(accountId, userRef);

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

### SFA.DAS.Authorization.CommitmentPermissions

To check if a party has the required permission:

```c#
var isAuthorized = _authorizationService.IsAuthorized(CommitmentOperation.AccessCohort);
```

Alternatively, if you're interested in why an authorization check has failed:

```c#
var authorizationResult = _authorizationService.GetAuthorizationResult(CommitmentOperation.AccessCohort);

if (!authorizationResult.IsAuthorized)
{
    if (authorizationResult.HasError<CommitmentPermissionNotGranted>())
    {
        // Handle permission not granted
    }
}
```

### SFA.DAS.Authorization.EmployerFeatures

To check if a feature is enabled:

```c#
var isAuthorized = _authorizationService.IsAuthorized("EmployerFeature.ProviderRelationships");
```

Alternatively, if you're interested in why an authorization check has failed:

```c#
var authorizationResult = _authorizationService.GetAuthorizationResult("EmployerFeature.ProviderRelationships");

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

### SFA.DAS.Authorization.EmployerUserRoles

To check if a user has the required role:

```c#
var isAuthorized = _authorizationService.IsAuthorized(EmployerUserRole.Owner);
```

Alternatively, if you're interested in why an authorization check has failed:

```c#
var authorizationResult = _authorizationService.GetAuthorizationResult(EmployerUserRole.Owner);

if (!authorizationResult.IsAuthorized)
{
    if (authorizationResult.HasError<EmployerUserRoleNotAuthorized>())
    {
        // Handle role not authorized
    }
}
```

> `AccountId` & `UserRef` authorization context values are required for this package.

### SFA.DAS.Authorization.Features

To check if a feature is enabled:

```c#
var isAuthorized = _authorizationService.IsAuthorized("Feature.ProviderRelationships");
```

Alternatively, if you're interested in why an authorization check has failed:

```c#
var authorizationResult = _authorizationService.GetAuthorizationResult("Feature.ProviderRelationships");

if (!authorizationResult.IsAuthorized)
{
    if (authorizationResult.HasError<FeatureNotEnabled>())
    {
        // Handle feature not enabled
    }
}
```

### SFA.DAS.Authorization.ProviderFeatures

To check if a feature is enabled:

```c#
var isAuthorized = _authorizationService.IsAuthorized("ProviderFeature.ProviderRelationships");
```

Alternatively, if you're interested in why an authorization check has failed:

```c#
var authorizationResult = _authorizationService.GetAuthorizationResult("ProviderFeature.ProviderRelationships");

if (!authorizationResult.IsAuthorized)
{
    if (authorizationResult.HasError<ProviderFeatureNotEnabled>())
    {
        // Handle feature not enabled
    }
    else if (authorizationResult.HasError<ProviderFeatureUserNotWhitelisted>())
    {
        // Handle user not whitelisted
    }
}
```

> `Ukprn` & `UserEmail` authorization context values are required for this package.

### SFA.DAS.Authorization.ProviderPermissions

To check if a provider has the required permission:

```c#
var isAuthorized = _authorizationService.IsAuthorized(ProviderPermission.CreateCohort);
```

Alternatively, if you're interested in why an authorization check has failed:

```c#
var authorizationResult = _authorizationService.GetAuthorizationResult(ProviderPermission.CreateCohort);

if (!authorizationResult.IsAuthorized)
{
    if (authorizationResult.HasError<ProviderPermissionNotGranted>())
    {
        // Handle permission not granted
    }
}
```

> `AccountLegalEntityId` & `Ukprn` authorization context values are required for this package.

### MVC Core

The `DasAuthorizeAttribute` attribute can be used to check users' authorization. It inherits from the default `Microsoft.AspNetCore.Authorization.AuthorizeAttribute` attribute and so can replace any current usages to ensure the current user is authenticated:

```c#
[DasAuthorize]
[Route("legalentities")]
public class LegalEntitiesController : Controller
{
    public ActionResult Index()
    {
        return View();
    }
    
    [DasAuthorize(EmployerUserRole.Owner)]
    [Route("add")]
    public ActionResult Add()
    {
        return View(new AddLegalEntityViewModel());
    }

    [HttpPost]
    [DasAuthorize(EmployerUserRole.Owner)]
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
    
    [DasAuthorize(EmployerUserRole.Owner)]
    [Route("add")]
    public ActionResult Add()
    {
        return View(new AddLegalEntityViewModel());
    }

    [HttpPost]
    [DasAuthorize(EmployerUserRole.Owner)]
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

    [DasAuthorize(EmployerUserRole.Owner)]
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

## License

Licensed under the [MIT license](LICENSE)