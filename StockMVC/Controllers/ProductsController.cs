﻿using System;
using System.Linq;
using Core.Entities;
using ManagementStocks.Core.Interfaces;
using ManagementStocks.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ManagementStocks.MVC.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ICommandRepository<Product> _productsCommandRepository;
        private readonly IQueryRepository<Product> _productsQueryRepository;
        private readonly IStocksQueryRepository _stocksQueryRepository;

        public ProductsController(
            ICommandRepository<Product> productsCommandRepository,
            IQueryRepository<Product> productsQueryRepository,
            IStocksQueryRepository stocksQueryRepository)
        {
            _productsCommandRepository = productsCommandRepository;
            _productsQueryRepository = productsQueryRepository;
            _stocksQueryRepository = stocksQueryRepository;
        }

        // GET: Products
        public IActionResult Index(string sortOrder, string searchString, int? page, int? pageSize)
        {
            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) || sortOrder == "name_desc" ? "name" : "name_desc";
            ViewData["QttySortParm"] = sortOrder == "qtty" ? "qtty_desc" : "qtty";
            ViewData["CurrentFilter"] = searchString;

            var products = _productsQueryRepository.Get()
                .Select(x => new ProductViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Qtty = _stocksQueryRepository.GetProductQtty(x.Id)
                }).ToList();

            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.Name.ToUpper().Contains(searchString.ToUpper())
                                               || s.Description.ToUpper().Contains(searchString.ToUpper())).ToList();
            }

            switch (sortOrder)
            {
                case "name_desc":
                    products = products.OrderByDescending(s => s.Name).ToList();
                    break;
                case "qtty":
                    products = products.OrderBy(s => s.Qtty).ToList();
                    break;
                case "qtty_desc":
                    products = products.OrderByDescending(s => s.Qtty).ToList();
                    break;
                default:
                    products = products.OrderBy(s => s.Name).ToList();
                    break;
            }
            return View(products);
        }

        // GET: Products/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _productsQueryRepository.Get(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name,Description")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.Id = Guid.NewGuid();
                _productsCommandRepository.Create(product);
                return RedirectToAction(nameof(Index));

            }
            return View(product);
        }

        // GET: Products/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _productsQueryRepository.Get(id.Value);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Id,Name,Description")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _productsCommandRepository.Update(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_productsQueryRepository.Get(product.Id) == null)
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
            return View(product);
        }

        // GET: Products/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _productsQueryRepository.Get(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            if (Math.Abs(_stocksQueryRepository.GetProductQtty(id.Value)) > double.Epsilon)
            {
                return RedirectToAction(nameof(Details), new { id });
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            if (Math.Abs(_stocksQueryRepository.GetProductQtty(id)) > double.Epsilon)
            {
                return RedirectToAction(nameof(Index));
            }
            _productsCommandRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
