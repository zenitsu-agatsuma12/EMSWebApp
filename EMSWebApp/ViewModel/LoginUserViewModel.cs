using System.ComponentModel.DataAnnotations;

namespace EMSWebApp.ViewModel
{
    public class LoginUserViewModel
    {
        [Required(ErrorMessage = "This field is required")]
        [EmailAddress]
        public string UserName { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
