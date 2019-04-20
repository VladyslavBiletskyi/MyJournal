using System.ComponentModel.DataAnnotations;

namespace MyJournal.WebApi.Models
{
    public class RegisterModel
    {
        [Required]
        [StringLength(60, MinimumLength = 5)]
        public string Login { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string GroupId { get; set; }

        [Required]
        public bool IsTeacher { get; set; }
    }
}