using System.ComponentModel.DataAnnotations;

namespace MyJournal.WebApi.Models.Account
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Поле обязательно к заполнению")]
        [StringLength(60, MinimumLength = 5)]
        public string Login { get; set; }

        [Required(ErrorMessage = "Поле обязательно к заполнению")]
        [StringLength(60, MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Поле обязательно к заполнению")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Поле обязательно к заполнению")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Поле обязательно к заполнению")]
        public string Surname { get; set; }

        public int GroupId { get; set; }

        [Required(ErrorMessage = "Поле обязательно к заполнению")]
        public bool IsTeacher { get; set; }
    }
}