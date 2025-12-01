using System.ComponentModel.DataAnnotations;

namespace E_Banking_MVC.Models
{
    public class Account
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The Holder field is required")]
        public string Holder { get; set; }

        [Required(ErrorMessage = "The Account Number is required")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "You can enter only alphanumeric characters")]
        public string Number { get; set; }

        public bool IsActive { get; set; }

        public bool HasOnlineBanking { get; set; }
    }
}
