using System.Security.Claims;

namespace EquipmentRental.WebApi
{
    public static class ClaimsHelper
    {
        public static string Email(this ClaimsPrincipal user) => user.FindFirst(ClaimTypes.Email)?.Value;
    }
}