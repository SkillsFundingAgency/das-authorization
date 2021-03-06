﻿namespace SFA.DAS.Authorization.NetCoreTestHarness.Authorization
{
    public static class TestOption
    {
        public const string Prefix = "TestOption.";
        public const string AuthorizedOption = "Authorized";
        public const string UnauthorizedSingleErrorOption = "UnauthorizedSingleError";
        public const string UnauthorizedMultipleErrorsOptions = "UnauthorizedMultipleErrors";
        
        public const string Authorized = Prefix + AuthorizedOption;
        public const string UnauthorizedSingleError = Prefix + UnauthorizedSingleErrorOption;
        public const string UnauthorizedMultipleErrors = Prefix + UnauthorizedMultipleErrorsOptions;

        public const string IoCPrefix = "Test IoC.";
        public const string GetCommitmentsAuthorizationHandler = IoCPrefix + "Commitments Authorization Handler";
        public const string GetIAuthorizationService = IoCPrefix + "IAuthorizationService";
    }
}