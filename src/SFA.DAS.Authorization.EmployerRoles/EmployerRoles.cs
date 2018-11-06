namespace SFA.DAS.Authorization.EmployerRoles
{
    public static class EmployerRoles
    {
        internal const string Namespace = "EmployerRoles";
        
        public const string Any = Namespace + ".Any";
        public const string Owner = Namespace + ".Owner";
        public const string OwnerOrTransactor = Owner + "," + Transactor;
        public const string Transactor = Namespace + ".Transactor";
        public const string Viewer = Namespace + ".Viewer";
    }
}