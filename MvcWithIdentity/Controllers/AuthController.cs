using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MvcWithIdentity.Attributes;
using MvcWithIdentity.VMs;

namespace MvcWithIdentity.Controllers;

public class AuthController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }
    
    [CheckLogged]
    public IActionResult Register()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Register(RegisterVM registerVm)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        var checkUser = await _userManager.FindByEmailAsync(registerVm.Email);
        if (checkUser is not null)
        {
            ModelState.AddModelError("User","User already exists");
        }
        var user = new IdentityUser() { UserName = registerVm.Name, Email = registerVm.Email };
        var result = await _userManager.CreateAsync(user, registerVm.Password);

        if (result.Succeeded)
        {
            //await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Email, registerVm.Email));
            return RedirectToAction("Index", "Home");
        }

        return View();
    }

    [CheckLogged]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginVM loginVm)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(loginVm.Name, loginVm.Password, loginVm.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid login attempt");
        }

        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}