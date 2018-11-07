namespace SFA.DAS.Authorization.EmployerRoles
{
    public static class EmployerRole
    {
        internal const string Namespace = "EmployerRole";
        internal const string AnyOption = "Any";
        internal const string OwnerOption = "Owner";
        internal const string TransactorOption = "Transactor";
        internal const string ViewerOption = "Viewer";
        
        public const string Any = Namespace + "." + AnyOption;
        public const string Owner = Namespace + "." + OwnerOption;
        public const string OwnerOrTransactor = Owner + "," + Transactor;
        public const string Transactor = Namespace + "." + TransactorOption;
        public const string Viewer = Namespace + "." + ViewerOption;
    }
}