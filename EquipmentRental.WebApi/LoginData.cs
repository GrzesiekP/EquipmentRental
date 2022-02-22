using System.Security.AccessControl;

namespace EquipmentRental.WebApi.Controllers
{
    public class LoginData
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}