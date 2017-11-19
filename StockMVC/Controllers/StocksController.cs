using System;
using Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Core.Entities;

namespace StockMVC.Controllers
{
    public class StocksController : Controller
    {
        private readonly IStocksRepository _stocksRepository;
        private readonly IProductsRepository _productsRepository;

        public StocksController(
            IStocksRepository stocksRepository,
            IProductsRepository productsRepository)
        {
            _stocksRepository = stocksRepository;
            _productsRepository = productsRepository;
        }

        // GET: Stocks
        public IActionResult Index()
        {
            return View(_stocksRepository.Get());
        }

        // GET: Stocks/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stock = _stocksRepository.Get(id.Value);
            if (stock == null)
            {
                return NotFound();
            }

            return View(stock);
        }

        // GET: Stocks/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_productsRepository.Get(), "Id", "Name", null);
            return View(new Stock());
        }

        // POST: Stocks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,ProductId,Quantity,Price,IsCredit,OperationTime")] Stock stock)
        {
            if (ModelState.IsValid)
            {
                stock.Id = Guid.NewGuid();
                _stocksRepository.Create(stock);
                return RedirectToAction(nameof(Index));
            }
            return View(stock);
        }

        // GET: Stocks/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stock = _stocksRepository.Get(id.Value);
            if (stock == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_productsRepository.Get(), "Id", "Name", stock.ProductId);
            return View(stock);
        }

        // POST: Stocks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Id,ProductId,Quantity,Price,IsCredit,OperationTime")] Stock stock)
        {
            if (id != stock.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _stocksRepository.Update(stock);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_stocksRepository.Get(stock.Id) == null)
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
            ViewData["ProductId"] = new SelectList(_productsRepository.Get(), "Id", "Name", stock.ProductId);
            return View(stock);
        }

        // GET: Stocks/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stock = _stocksRepository.Get(id.Value);
            if (stock == null)
            {
                return NotFound();
            }

            return View(stock);
        }

        // POST: Stocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            _stocksRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
