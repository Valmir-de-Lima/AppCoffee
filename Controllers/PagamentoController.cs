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
    public class PagamentoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PagamentoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Curso
        public async Task<IActionResult> Index()
        {
            return _context.Pagamentos != null ?
                        View(await _context.Pagamentos
                            .Where(x => x.Cliente == User.Identity!.Name)
                            .ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Cursos'  is null.");
        }
    }
}
