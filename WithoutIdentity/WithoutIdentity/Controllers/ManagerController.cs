using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WithoutIdentity.Models;
using WithoutIdentity.Models.ManageViewModel;

namespace WithoutIdentity.Controllers
{
    public class ManagerController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ManagerController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;

        }
        [TempData]
        public string StatusMessage { get; set; }
        
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if(user == null)
            {
                throw new ApplicationException($"Não foi possível carregar o usuário com o Id '{_userManager.GetUserId(User)}'");
            }
            var model = new IndexViewModel
            {
                Username = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IsEmailConfirmed = user.EmailConfirmed,
                StatusMessage = StatusMessage 
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index (IndexViewModel indexViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(indexViewModel);
            }
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Não foi possível carregar o usuário com o Id '{_userManager.GetUserId(User)}'");
            }

            var email = user.Email;
            if(email != indexViewModel.Email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, indexViewModel.Email);
                if (!setEmailResult.Succeeded)
                {
                    throw new ApplicationException($"Erro inesperado ao atribuir um email para o usuário com o Id '{_userManager.GetUserId(User)}'");
                }
            }
            
            var phonenumber = user.PhoneNumber;
            if(phonenumber != indexViewModel.PhoneNumber)
            {
                var setPhoneNumberResult = await _userManager.SetPhoneNumberAsync(user, indexViewModel.PhoneNumber);
                if (!setPhoneNumberResult.Succeeded)
                {
                    throw new ApplicationException($"Erro inesperado ao atribuir um telefone para o usuário com o Id '{_userManager.GetUserId(User)}'");
                }
            }

            StatusMessage = "Seu perfil foi atualizado!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await _userManager.GetUserAsync(User);
            if(user == null)
            {
                throw new ApplicationException($"Não foi possível carregar os dados do usuário com id {_userManager.GetUserId(User)}");
            }
            var model = new ChangePasswordViewModel { StatusMessage = StatusMessage };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(changePasswordViewModel);
            }
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Não foi possível carregar os dados do usuário com id {_userManager.GetUserId(User)}");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, changePasswordViewModel.OldPassword, changePasswordViewModel.NewPassword);

            if (!changePasswordResult.Succeeded)
            {
                foreach (var erro in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, erro.Description);
                }
                return View(changePasswordViewModel);
            }
            await _signInManager.SignInAsync(user, isPersistent: false);
            StatusMessage = "Sua senha foi alterada com sucesso.";

            return RedirectToAction(nameof(ChangePassword));
        }
    }
}
