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
    public class PedidoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PedidoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Modulo
        public async Task<IActionResult> Index()
        {
            var list = _context.Pedidos?
                            .Include(p => p.Produtos)
                            .Where(x => x.Cliente == User.Identity!.Name)
                            .ToListAsync();
            return View(await list!);
        }

        // GET: Modulo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Pedidos == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos
                .Include(p => p.Produtos)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        // GET: Modulo/Create
        public IActionResult Create()
        {
            // Obtenha a lista de produtos do banco de dados
            var produtos = _context.Produtos!.ToList();

            // Armazene o SelectList em ViewBag para uso na visão
            ViewBag.Produtos = produtos;
            return View();
        }

        // POST: Modulo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Cliente,Total")] Pedido pedido, string[] ProdutoSelecionado)
        {
            if (ModelState.IsValid)
            {
                if (ProdutoSelecionado != null && ProdutoSelecionado.Any())
                {
                    // Aqui você pode usar ProdutosSelecionados para obter os IDs dos produtos selecionados
                    // e atribuir esses produtos ao pedido da maneira desejada.

                    List<Produto> produtosSelecionados = await _context.Produtos?
                        .Where(p => ProdutoSelecionado.Contains(p.Id.ToString()))
                        .ToListAsync()!;

                    // Exemplo: atribuir a lista de produtos selecionados ao pedido
                    pedido.Produtos = produtosSelecionados;

                    decimal total = 0;
                    foreach (Produto produto in pedido.Produtos.ToList())
                    {
                        total += produto.Preco;
                    }
                    pedido.Total = total;
                }
                pedido.Status = "Pedido criado. Aguardando pagamento";

                _context.Add(pedido);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pedido);
        }

        // GET: Modulo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Pedidos == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos
                                .Include(p => p.Produtos)
                                .FirstOrDefaultAsync(m => m.Id == id);
            if (pedido == null)
            {
                return NotFound();
            }

            // Obtenha a lista de produtos do banco de dados
            var produtos = _context.Produtos!.ToList();

            // Armazene o SelectList em ViewBag para uso na visão
            ViewBag.Produtos = produtos;

            return View(pedido);
        }

        // POST: Modulo/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Cliente,Total,Produtos")] Pedido pedido, string[] ProdutoSelecionado)
        {
            if (id != pedido.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (ProdutoSelecionado != null && ProdutoSelecionado.Any())
                    {
                        // Aqui você pode usar ProdutosSelecionados para obter os IDs dos produtos selecionados
                        // e atribuir esses produtos ao pedido da maneira desejada.

                        List<Produto> produtosSelecionados = await _context.Produtos?
                            .Where(p => ProdutoSelecionado.Contains(p.Id.ToString()))
                            .ToListAsync()!;

                        // Exemplo: atribuir a lista de produtos selecionados ao pedido
                        pedido.Produtos = produtosSelecionados;

                        decimal total = 0;
                        foreach (Produto produto in pedido.Produtos.ToList())
                        {
                            total += produto.Preco;
                        }
                        pedido.Total = total;
                    }

                    Pedido? ped = await _context.Pedidos!
                                .Include(p => p.Produtos)
                                .FirstOrDefaultAsync(m => m.Id == id);
                    ped!.Cliente = pedido.Cliente;
                    ped.Total = pedido.Total;
                    ped.Produtos = pedido.Produtos.ToList();

                    _context.Update(ped);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PedidoExists(pedido.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(pedido);
        }

        // GET: Modulo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Pedidos == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        // POST: Modulo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Pedidos == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Modulos'  is null.");
            }
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido != null)
            {
                _context.Pedidos.Remove(pedido);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PedidoExists(int id)
        {
            return (_context.Pedidos?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        // GET: Modulo/Payment/6
        public async Task<IActionResult> Payment(int? id)
        {
            if (id == null || _context.Pedidos == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos
                                .Include(p => p.Produtos)
                                .FirstOrDefaultAsync(m => m.Id == id);
            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PaymentCreate(int id, string formaPagamento)
        {
            try
            {
                Pedido? pedido = await _context.Pedidos!
                    .Include(p => p.Produtos)
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (pedido == null)
                {
                    return NotFound();
                }

                Pagamento pagamento = new Pagamento
                {
                    Cliente = pedido.Cliente,
                    FormaPagamento = formaPagamento,
                    Total = pedido.Total,
                    PedidoId = pedido.Id,
                    Pedido = pedido
                };

                _context.Add(pagamento);
                await _context.SaveChangesAsync();

                pedido.Status = "Pedido pago. Aguardando recebimento";

                _context.Update(pedido);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PedidoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // GET: Modulo/Payment/6
        public async Task<IActionResult> Recept(int? id)
        {
            if (id == null || _context.Pedidos == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos
                                .Include(p => p.Produtos)
                                .FirstOrDefaultAsync(m => m.Id == id);
            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReceptCreate(int id)
        {
            try
            {
                Pedido? pedido = await _context.Pedidos!
                    .Include(p => p.Produtos)
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (pedido == null)
                {
                    return NotFound();
                }

                pedido.Status = "Pedido recebido. Aguardando avaliacao";
                pedido.Recebido = true;

                _context.Update(pedido);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PedidoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<IActionResult> Assess(int? id)
        {
            if (id == null || _context.Pedidos == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos
                                .Include(p => p.Produtos)
                                .FirstOrDefaultAsync(m => m.Id == id);
            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssessCreate(int id, string avaliacao, string resposta)
        {
            try
            {
                Pedido? pedido = await _context.Pedidos!
                    .Include(p => p.Produtos)
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (pedido == null)
                {
                    return NotFound();
                }

                pedido.Avaliacao = avaliacao;
                pedido.Resposta = resposta;
                pedido.Status = "Pedido recebido e avaliado";

                _context.Update(pedido);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PedidoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
