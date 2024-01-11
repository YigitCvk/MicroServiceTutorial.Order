using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class SigninInput
    {
        [Display(Name = "Email ")]
        public string Email { get; set; }
        [Display(Name = "Şifre ")]
        public string Password { get; set; }
        [Display(Name = "Beni Hatırla ")]
        public bool IsRemember { get; set; }
    }
}
