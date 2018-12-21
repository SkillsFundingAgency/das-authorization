namespace SFA.DAS.Authorization.TestHarness
{
    public static class TestOptions
    {
        public const string Namespace = "TestHarnessAuthorizationHandlerNamespace";

        public const string DasAuthorizeTrueNoNamespace = "DasAuthorizeTrue";
        public const string DasAuthorizeFalseNoNamespace = "DasAuthorizeFalse";
        public const string DasAuthorizeFalseWithErrorsNoNamespace = "DasAuthorizeFalseWithErrors";

        public const string DasAuthorizeTrue = Namespace + "." + DasAuthorizeTrueNoNamespace;
        public const string DasAuthorizeFalse = Namespace + "." + DasAuthorizeFalseNoNamespace;
        public const string DasAuthorizeFalseWithErrors = Namespace + "." + DasAuthorizeFalseWithErrorsNoNamespace;


    }
}