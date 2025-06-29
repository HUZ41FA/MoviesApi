namespace Movies.Api.Auth
{
    public static class AuthConstants
    {
        public const string AdminPolicyName = "AllowAdmin";
        public const string AdminUserClaimName = "admin";

        public const string TrustedMemberPolicyName = "AllowTrustedMember";
        public const string TrustedMemberUserClaimName = "trusted_member";
    }
}
