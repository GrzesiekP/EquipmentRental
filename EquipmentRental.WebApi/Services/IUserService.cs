using EquipmentRental.WebApi.Controllers;

namespace EquipmentRental.WebApi.Services
{
    public interface IUserService
    {
        public string Authenticate(LoginData loginData);
    }
}