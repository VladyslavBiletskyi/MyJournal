using System.ComponentModel.DataAnnotations;

namespace MyJournal.WebApi.Models.Account
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

        public bool Remember { get; set; }

        public string ReturnUrl { get; set; }
    }
}
