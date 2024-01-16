using System.ComponentModel.DataAnnotations;

namespace MvcWithIdentity.VMs;

public class RegisterVM
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}