using System.ComponentModel.DataAnnotations;

namespace Tiles.Models.Data
{
    public class LoginData
    {
        [Required(ErrorMessage = "Не указан логин")]
        public string Login { get; set; }
        
        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
