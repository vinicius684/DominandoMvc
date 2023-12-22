﻿using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using AppSemTemplate.Data;
using AppSemTemplate.Models;
using Microsoft.AspNetCore.Authorization;
using AppSemTemplate.Extensions;

namespace AppSemTemplate.Controllers
{

    //[Authorize(Roles = "Admin")]
    [Authorize]
    [Route("meus-produtos")]
    public class ProdutosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProdutosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Produtos
        //[Authorize(Policy = "VerProdutos")]
        [AllowAnonymous]
       // [ClaimsAuthorize("Produtos", "VI")]
        public async Task<IActionResult> Index()
        {
            var user = HttpContext.User.Identity;

            return _context.Produto != null ?
                        View(await _context.Produto.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Produto'  is null.");
        }

        // GET: Produtos/Details/5
        [ClaimsAuthorize("Produtos", "VI")]
        [Route("detalhes/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Produto == null)
            {
                return NotFound();
            }

            var produto = await _context.Produto
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        // GET: Produtos/Create
        [ClaimsAuthorize("Produtos", "ADD")]//customizando autenticação
        [Route("criar-novo")]
        public IActionResult CriarNovoProduto()
        {
            return View("Create");
        }

        // POST: Produtos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[ValidateAntiForgeryToken]
        [ClaimsAuthorize("Produtos", "AD")]
        [HttpPost("criar-novo")]
        public async Task<IActionResult> CriarNovoProduto([Bind("Id,Nome,Imagem,Valor")] Produto produto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(produto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View("Create", produto);
        }

        // GET: Produtos/Edit/5
        [ClaimsAuthorize("Produtos", "ED")]
        [Route("editar-produto/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Produto == null)
            {
                return NotFound();
            }

            var produto = await _context.Produto.FindAsync(id);
            if (produto == null)
            {
                return NotFound();
            }
            return View(produto);
        }

        // POST: Produtos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [ClaimsAuthorize("Produtos", "ED")]
        [HttpPost("editar-produto/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Imagem,Valor")] Produto produto)
        {
            if (id != produto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(produto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProdutoExists(produto.Id))
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
            return View(produto);
        }

        // GET: Produtos/Delete/5
        //[Authorize(Policy = "PodeExcluirPErmanentemente")]
        [Route("excluir/{id}")]
        [ClaimsAuthorize("Produtos", "EX")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Produto == null)
            {
                return NotFound();
            }

            var produto = await _context.Produto
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        // POST: Produtos/Delete/5
        // [Authorize(Policy = "PodeExcluirPErmanentemente")]
        [HttpPost("excluir/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize("Produtos", "EX")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Produto == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Produto'  is null.");
            }
            var produto = await _context.Produto.FindAsync(id);
            if (produto != null)
            {
                _context.Produto.Remove(produto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProdutoExists(int id)
        {
            return (_context.Produto?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
