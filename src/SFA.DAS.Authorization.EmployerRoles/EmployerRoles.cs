namespace SFA.DAS.Authorization.EmployerRoles
{
    public static class EmployerRoles
    {
        public const string Any = "EmployerRoles.Any";
        public const string Owner = "EmployerRoles.Owner";
        public const string OwnerOrTransactor = Owner + "," + Transactor;
        public const string Transactor = "EmployerRoles.Transactor";
        public const string Viewer = "EmployerRoles.Viewer";
    }
}