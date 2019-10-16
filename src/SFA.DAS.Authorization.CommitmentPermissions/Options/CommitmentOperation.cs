namespace SFA.DAS.Authorization.CommitmentPermissions.Options
{
    public class CommitmentOperation
    {
        internal const string Prefix = "CommitmentOperation.";
        internal const string AccessCohortOption = "AccessCohort";
        internal const string IgnoreEmptyCohortOption = "IgnoreEmptyCohort";
        
        public const string AccessCohort = Prefix + AccessCohortOption;

        public const string AllowEmptyCohort = Prefix + IgnoreEmptyCohortOption;
    }
}