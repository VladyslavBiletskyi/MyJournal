using System.ComponentModel.DataAnnotations;

namespace MyJournal.WebApi.Models.Account
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "Поле обязательно к заполнению")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Поле обязательно к заполнению")]
        [StringLength(60, MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Поле обязательно к заполнению")]
        [StringLength(60, MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string NewPasswordConfirmation { get; set; }
    }
}
