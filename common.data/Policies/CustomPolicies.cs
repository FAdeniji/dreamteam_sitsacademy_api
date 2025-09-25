using Microsoft.AspNetCore.Authorization;

namespace common.data;
public static class CustomPolicies
{
    public const string CoIdeaOwner = "CoIdeaOwner";

    public static AuthorizationPolicy CoIdeaOwnerPolicy()
    {
        return new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .RequireRole(CoIdeaOwner)
            .Build();
    }

    public const string IdeaOwner = "IdeaOwner";

    public static AuthorizationPolicy IdeaOwnerPolicy()
    {
        return new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .RequireRole(IdeaOwner)
            .Build();
    }

    public const string Administrator = "Administrator";

    public static AuthorizationPolicy AdministratorPolicy()
    {
        return new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .RequireRole(Administrator)
            .Build();
    }

    public const string Student = "Student";

    public static AuthorizationPolicy StudentPolicy()
    {
        return new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .RequireRole(Student)
            .Build();
    }

    public const string Lecturer = "Lecturer";

    public static AuthorizationPolicy LecturerPolicy()
    {
        return new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .RequireRole(Lecturer)
            .Build();
    }

    public const string LearningInstitution = "LearningInstitution";

    public static AuthorizationPolicy LearningInstitutionPolicy()
    {
        return new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .RequireRole(LearningInstitution)
            .Build();
    }

    public const string Investor = "Investor";

    public static AuthorizationPolicy InvestorPolicy()
    {
        return new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .RequireRole(Investor)
            .Build();
    }

    public const string Institution = "Institution";

    public static AuthorizationPolicy InstitutionPolicy()
    {
        return new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .RequireRole(Institution)
            .Build();
    }

    public const string Everyone = "IdeaOwner, CoIdeaOwner, Administrator, Investor, Institution, LearningInstitution, Lecturer, Student";

    public static AuthorizationPolicy EveryonePolicy()
    {
        return new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .RequireRole(Everyone)
            .Build();
    }

    public const string LIManager = "Administrator,LearningInstitution,Lecturer";

    public static AuthorizationPolicy LIManagerPolicy()
    {
        return new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .RequireRole(LIManager)
            .Build();
    }

    public const string LIAdmin = "Administrator,LearningInstitution";

    public static AuthorizationPolicy LIAdminPolicy()
    {
        return new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .RequireRole(LIAdmin)
            .Build();
    }

    public const string ALLLI = "Administrator,LearningInstitution,Lecturer,Student";

    public static AuthorizationPolicy ALLLIPolicy()
    {
        return new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .RequireRole(ALLLI)
            .Build();
    }
}

