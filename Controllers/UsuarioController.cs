using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppCurso.Data;
using AppCurso.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using AppCurso.ViewModels;

namespace AppCurso
{
    public class UsuarioController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public UsuarioController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: Curso
        [HttpGet]
        public IActionResult Index()
        {
            var usuarioViewModel = new UsuarioViewModel();
            return View(usuarioViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(UsuarioViewModel usuarioViewModel)
        {
            if (usuarioViewModel.Email == null || usuarioViewModel.Senha == null)
            {
                ModelState.AddModelError(string.Empty, "É necessário e-mail e senha.");
                return View(usuarioViewModel); // Volte para a tela de login
            }

            var user = await _userManager.FindByEmailAsync(usuarioViewModel.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, usuarioViewModel.Senha))
            {
                // Falha na autenticação, exiba uma mensagem de erro
                ModelState.AddModelError(string.Empty, "Usuário ou senha inválidos.");
                return View(usuarioViewModel); // Volte para a tela de login
            }

            // Use o ASP.NET Core Identity para autenticar o usuário
            await _signInManager.SignInAsync(user, isPersistent: false);

            return RedirectToAction("Index", "Home"); // Redirecionar para o painel principal
        }

        [HttpGet]
        public IActionResult Register()
        {
            var usuarioViewModel = new UsuarioViewModel();
            return View(usuarioViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register(UsuarioViewModel usuarioViewModel)
        {
            // Verifica se o modelo é válido
            if (!ModelState.IsValid)
            {
                return View(usuarioViewModel);
            }

            try
            {
                // Verifica se o e-mail já está em uso
                if (await _userManager.FindByEmailAsync(usuarioViewModel.Email) != null)
                {
                    ModelState.AddModelError(string.Empty, "O e-mail já está em uso.");
                    return View(usuarioViewModel);
                }

                // Cria um novo usuário
                var user = new IdentityUser { UserName = usuarioViewModel.Email, Email = usuarioViewModel.Email };
                var result = await _userManager.CreateAsync(user, usuarioViewModel.Senha);

                if (result.Succeeded)
                {
                    // Autentica o usuário após o registro
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    // Redireciona para o painel principal
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Erro ao registrar o usuário");

                    return View(usuarioViewModel);
                }
            }
            catch (Exception)
            {
                // Trate a exceção de maneira adequada (log, mensagem, etc.)
                ModelState.AddModelError(string.Empty, "Erro ao registrar o usuário: ");
                return View(usuarioViewModel);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            // Faz logout do usuário
            _signInManager.SignOutAsync();

            // Limpa os cookies de autenticação
            HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

            return RedirectToAction("Index", "Home");
        }

    }
}
