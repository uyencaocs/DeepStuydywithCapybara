using System.Security.Claims;

namespace DeeplearningwithCapybara.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static bool IsSubscribe(this ClaimsPrincipal user)
        {
            // Example: check for a claim named "IsSubscribe" with value "true"
            return user.HasClaim(c => c.Type == "IsSubscribe" && c.Value == "true");
        }
    }
}