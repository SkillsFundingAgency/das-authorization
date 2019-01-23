namespace SFA.DAS.Authorization.NetFrameworkTestHarness
{
    public static class TestOption
    {
        public const string Namespace = "TestOption";
        public const string AuthorizedOption = "Authorized";
        public const string UnauthorizedSingleErrorOption = "UnauthorizedSingleError";
        public const string UnauthorizedMultipleErrorsOptions = "UnauthorizedMultipleErrors";
        
        public const string Authorized = Namespace + "." + AuthorizedOption;
        public const string UnauthorizedSingleError = Namespace + "." + UnauthorizedSingleErrorOption;
        public const string UnauthorizedMultipleErrors = Namespace + "." + UnauthorizedMultipleErrorsOptions;
    }
}