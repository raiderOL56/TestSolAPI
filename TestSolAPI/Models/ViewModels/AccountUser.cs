using System.ComponentModel.DataAnnotations;

namespace TestSolAPI.Models.ViewModels
{
    public class AccountUser
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}