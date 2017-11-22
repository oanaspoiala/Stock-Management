using System;
using System.Linq;
using Core.Entities;
using ManagementStocks.Core.Entities;
using ManagementStocks.Core.Interfaces;
using ManagementStocks.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StockManagement.Utils.QueryUtils;

namespace ManagementStocks.MVC.Controllers
{
    public class StocksController : Controller
    {
        private readonly IQueryRepository<Product> _productsQueryRepository;
        private readonly ICommandRepository<Stock> _stockCommandRepository;
        private readonly IStocksQueryRepository _stocksQueryRepository;

        public StocksController(
            IQueryRepository<Product> productsQueryRepository,
            ICommandRepository<Stock> stockCommandRepository,
            IStocksQueryRepository stocksQueryRepository)
        {
            _productsQueryRepository = productsQueryRepository;
            _stockCommandRepository = stockCommandRepository;
            _stocksQueryRepository = stocksQueryRepository;
        }

        // GET: Stocks
        public IActionResult Index(string sortOrder, string searchString, int? page, int? pageSize)
        {
            ViewData["OperationTimeSortParm"] = sortOrder == "operationTime" ? "operationTime_desc" : "operationTime";
            ViewData["ProductNameSortParm"] = string.IsNullOrEmpty(sortOrder) || sortOrder == "name_desc" ? "name" : "name_desc";
            ViewData["PriceSortParm"] = string.IsNullOrEmpty(sortOrder) || sortOrder == "price_desc" ? "price" : "price_desc";
            ViewData["QttySortParm"] = sortOrder == "qtty" ? "qtty_desc" : "qtty";
            ViewData["CurrentFilter"] = searchString;

            var queryParameters = new QueryParameters
            {
                PageNumber = page ?? 1,
                PageSize = pageSize ?? 10
            };
            var sortItem = new QuerySortItem();
            switch (sortOrder)
            {
                case "operationTime_desc":
                    sortItem.FieldName = "OperationTime";
                    sortItem.Descending = true;
                    break;
                case "operationTime":
                    sortItem.FieldName = "OperationTime";
                    sortItem.Descending = false;
                    break;
                case "name_desc":
                    sortItem.FieldName = "Name";
                    sortItem.Descending = true;
                    break;
                case "name":
                    sortItem.FieldName = "Name";
                    sortItem.Descending = false;
                    break;
                case "price_desc":
                    sortItem.FieldName = "Price";
                    sortItem.Descending = true;
                    break;
                case "qtty":
                    sortItem.FieldName = "Qtty";
                    sortItem.Descending = false;
                    break;
                case "qtty_desc":
                    sortItem.FieldName = "Qtty";
                    sortItem.Descending = true;
                    break;
                default:
                    sortItem.FieldName = "Price";
                    sortItem.Descending = false;
                    break;
            }
            queryParameters.SortItems.Add(sortItem);

            if (!string.IsNullOrEmpty(searchString))
            {
                queryParameters.Filters.Add(new QueryFilterItem
                {
                    FieldName = "Name",
                    FilterValue = searchString,
                    FilterOperator = "LIKE",
                    ParameterName = "Name"
                });
            }

            var stocks = _stocksQueryRepository.Get(queryParameters)
                .Select(x => new StockViewModel
                {
                    Id = x.Id,
                    Price = x.Price,
                    Quantity = x.Quantity,
                }).ToList();
            return View(stocks);
        }

        // GET: Stocks/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stock = _stocksQueryRepository.Get(id.Value);
            if (stock == null)
            {
                return NotFound();
            }

            return View(stock);
        }

        // GET: Stocks/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_productsQueryRepository.Get(), "Id", "Name", null);
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
                _stockCommandRepository.Create(stock);
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

            var stock = _stocksQueryRepository.Get(id.Value);
            if (stock == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_productsQueryRepository.Get(), "Id", "Name", stock.ProductId);
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
                    _stockCommandRepository.Update(stock);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_stocksQueryRepository.Get(stock.Id) == null)
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
            ViewData["ProductId"] = new SelectList(_productsQueryRepository.Get(), "Id", "Name", stock.ProductId);
            return View(stock);
        }

        // GET: Stocks/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stock = _stocksQueryRepository.Get(id.Value);
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
            if (Math.Abs(_stocksQueryRepository.GetProductQtty(id)) > double.Epsilon)
            {
                return RedirectToAction(nameof(Index));
            }
            _stockCommandRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
