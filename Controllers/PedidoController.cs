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
using System.Drawing.Drawing2D;
using OpenAI_API;
using AppCurso.ViewModels;

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
            var pedidos = _context.Pedidos?
                            .Include(p => p.ProdutoPedidos)
                            .Where(x => x.Cliente == User.Identity!.Name)
                            .ToListAsync();

            // Obtenha a lista de produtos do banco de dados
            var produtos = _context.Produtos!.ToList();
            // Armazene o SelectList em ViewBag para uso na visão
            ViewBag.Produtos = produtos;

            return View(await pedidos!);
        }

        // GET: Modulo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Pedidos == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos
                .Include(p => p.ProdutoPedidos)
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
        public async Task<IActionResult> Create(PedidoViewModel pedidoViewModel)
        {
            var pedido = new Pedido();
            pedido.Cliente = pedidoViewModel.Cliente;
            pedido.Total = pedidoViewModel.TotalPedido;
            pedido.Status = "Pedido criado. Aguardando pagamento";

            var produtosPedidos = new List<ProdutoPedido>();
            foreach (var produto in pedidoViewModel.Produtos)
            {
                var produtoPedido = new ProdutoPedido();
                if (produto.ValorTotal > 0)
                {
                    produtoPedido.ProdutoId = produto.ProdutoId;
                    produtoPedido.Descricao = produto.Descricao;
                    produtoPedido.Preco = produto.Preco;
                    produtoPedido.Quantidade = produto.Quantidade;
                    produtoPedido.Total = produto.ValorTotal;
                    produtosPedidos.Add(produtoPedido);
                }
            }
            pedido.ProdutoPedidos = produtosPedidos.ToList();

            _context.Add(pedido);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Modulo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Pedidos == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos
                                .Include(p => p.ProdutoPedidos)
                                .FirstOrDefaultAsync(m => m.Id == id);
            if (pedido == null)
            {
                return NotFound();
            }

            var pedidoViewModel = new PedidoViewModel();
            pedidoViewModel.Id = pedido.Id;
            pedidoViewModel.Cliente = pedido.Cliente;
            pedidoViewModel.TotalPedido = pedido.Total;

            var produtoViewModels = new List<ProdutoViewModel>();
            var produtos = _context.Produtos!.ToList();
            foreach (var produto in produtos)
            {
                var produtoViewModel = new ProdutoViewModel();
                produtoViewModel.ProdutoId = produto.Id;
                produtoViewModel.Descricao = produto.Descricao;
                produtoViewModel.Preco = produto.Preco;
                foreach (var item in pedido.ProdutoPedidos)
                {
                    if (item.ProdutoId == produto.Id)
                    {
                        produtoViewModel.Quantidade = item.Quantidade;
                        produtoViewModel.ValorTotal = item.Total;
                    }
                }
                produtoViewModels.Add(produtoViewModel);
            }

            pedidoViewModel.Produtos = produtoViewModels.ToList();

            return View(pedidoViewModel);
        }

        // POST: Modulo/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PedidoViewModel pedidoViewModel)
        {
            try
            {
                Pedido? pedido = await _context.Pedidos!
                            .Include(p => p.ProdutoPedidos)
                            .FirstOrDefaultAsync(m => m.Id == pedidoViewModel.Id);

                if (pedido != null)
                {
                    // Atualizar as propriedades do pedido com os valores do ViewModel
                    pedido.Cliente = pedidoViewModel.Cliente;
                    pedido.Total = pedidoViewModel.TotalPedido;

                    // Limpar os produtos associados ao pedido
                    pedido.ProdutoPedidos.Clear();

                    // Adicionar os novos produtos ao pedido
                    foreach (var produto in pedidoViewModel.Produtos)
                    {
                        if (produto.ValorTotal > 0)
                        {
                            var produtoPedido = new ProdutoPedido
                            {
                                ProdutoId = produto.ProdutoId,
                                Descricao = produto.Descricao,
                                Preco = produto.Preco,
                                Quantidade = produto.Quantidade,
                                Total = produto.ValorTotal
                            };

                            pedido.ProdutoPedidos.Add(produtoPedido);
                        }
                    }

                    _context.Update(pedido);
                }

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PedidoExists(pedidoViewModel.Id))
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
                                .Include(p => p.ProdutoPedidos)
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
                    .Include(p => p.ProdutoPedidos)
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
                                .Include(p => p.ProdutoPedidos)
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
                    .Include(p => p.ProdutoPedidos)
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
                                .Include(p => p.ProdutoPedidos)
                                .FirstOrDefaultAsync(m => m.Id == id);
            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssessCreate(int id, string avaliacao)
        {
            try
            {
                Pedido? pedido = await _context.Pedidos!
                    .Include(p => p.ProdutoPedidos)
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (pedido == null)
                {
                    return NotFound();
                }

                pedido.Avaliacao = avaliacao;
                pedido.Resposta = await GetOpenAIResponse(avaliacao);
                pedido.Status = "Pedido recebido e avaliado";

                _context.Update(pedido);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Avaliacao");
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

        public async Task<string> GetOpenAIResponse(string avaliacao)
        {
            var resposta = "";

            OpenAIAPI api = new OpenAIAPI(Configuration.OPENAI);
            var chat = api.Chat.CreateConversation();

            chat.AppendSystemMessage("Você é um atendente de uma cafeteria que responda a avaliação do cliente");
            chat.AppendUserInput(avaliacao);
            resposta = await chat.GetResponseFromChatbotAsync();
            return resposta;
        }
    }
}

