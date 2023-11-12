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

namespace AppCurso
{
    [Authorize]
    public class AvaliacaoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AvaliacaoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Curso
        public async Task<IActionResult> Index()
        {
            return _context.Pedidos != null ?
                        View(await _context.Pedidos
                            .Where(x => x.Cliente == User.Identity!.Name && x.Recebido == true)
                            .ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Cursos'  is null.");
        }
    }
}
