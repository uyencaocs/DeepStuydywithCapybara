using System.Security.Claims;

public static class ClaimsPrincipalExtensions
{
    public static bool IsSubscribe(this ClaimsPrincipal user)
    {
        // Replace with your actual logic for checking subscription
        // For example, check for a claim or role:
        return user.HasClaim("Subscription", "Active");
    }
}