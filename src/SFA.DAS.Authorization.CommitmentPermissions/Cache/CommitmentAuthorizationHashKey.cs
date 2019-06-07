using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.Authorization.CommitmentPermissions.Cache
{
    public class CommitmentAuthorizationHashKey
    {
        private readonly int _hashCode;

        public CommitmentAuthorizationHashKey(Party partyType, long partyId, long cohortId, IReadOnlyCollection<string> options)
        {
            PartyType = partyType;
            PartytId = partyId;
            CohortId = cohortId;
            Options = options;
            _hashCode = CalculateHashCode();
        }

        public Party PartyType { get; }
        public long PartytId { get; }
        public long CohortId { get; }
        public IReadOnlyCollection<string> Options { get; }

        public override int GetHashCode()
        {
            return _hashCode;
        }

        public override bool Equals(object obj)
        {
            return obj is CommitmentAuthorizationHashKey other &&
                   other.PartyType == PartyType &&
                   other.PartytId == PartytId &&
                   other.CohortId == CohortId &&
                   other.Options.IsSameAs(Options);
        }

        public static bool operator ==(CommitmentAuthorizationHashKey lhs, CommitmentAuthorizationHashKey rhs)
        {
            if (ReferenceEquals(lhs, rhs))
            {
                return true;
            }

            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
            {
                return false;
            }

            return lhs.Equals(rhs);
        }

        public static bool operator !=(CommitmentAuthorizationHashKey lhs, CommitmentAuthorizationHashKey rhs)
        {
            return !(lhs == rhs);
        }

        private int CalculateHashCode()
        {
            unchecked
            {
                int hash = (int)2166136261;
                hash = (hash * 16777619) ^ PartyType.GetHashCode();
                hash = (hash * 16777619) ^ PartytId.GetHashCode();
                hash = (hash * 16777619) ^ CohortId.GetHashCode();
                if (Options != null)
                {
                    foreach (var option in Options)
                    {
                        hash = (hash * 16777619) ^ option.GetHashCode();
                    }
                }

                return hash;
            }
        }
    }
}