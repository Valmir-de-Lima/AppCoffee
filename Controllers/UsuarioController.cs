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
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public UsuarioController(ApplicationDbContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
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
        public async Task<IActionResult> Login(string email, string senha)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, senha))
            {
                // Falha na autenticação, exiba uma mensagem de erro
                ModelState.AddModelError(string.Empty, "Usuário ou senha inválidos.");
                return RedirectToAction("Index", "Usuario"); // Volte para a tela de login
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
        public async Task<IActionResult> Register(string email, string senha)
        {
            // Verifica se o modelo é válido
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                // Verifica se o e-mail já está em uso
                if (await _userManager.FindByEmailAsync(email) != null)
                {
                    ModelState.AddModelError(string.Empty, "O e-mail já está em uso.");
                    return View();
                }

                // Cria um novo usuário
                var user = new IdentityUser { UserName = email, Email = email };
                var result = await _userManager.CreateAsync(user, senha);

                if (result.Succeeded)
                {
                    // Autentica o usuário após o registro
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    // Redireciona para o painel principal
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Se houver erros durante a criação do usuário, adicione-os ao ModelState
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    return View();
                }
            }
            catch (Exception ex)
            {
                // Trate a exceção de maneira adequada (log, mensagem, etc.)
                ModelState.AddModelError(string.Empty, "Erro ao registrar o usuário: " + ex);
                return View();
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
