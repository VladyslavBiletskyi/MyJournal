using System.ComponentModel.DataAnnotations;

namespace MyJournal.Models
{
    public class LoginModel
    {
        [Required]
        [StringLength(60, MinimumLength = 5)]
        public string Login { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public LoginModel()
        {

        }

        public LoginModel(string login, string password)
        {
            Login = login;
            Password = password;
        }
    }
}
