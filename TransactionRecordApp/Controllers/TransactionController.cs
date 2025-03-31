using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TransactionRecordApp.Models;

namespace TransactionRecordApp.Controllers
{
    /// <summary>
    /// Controls the Add, Edit and Delete functions
    /// </summary>
    public class TransactionController : Controller
    {
        private readonly TransactionContext _transactionContext;

        public TransactionController(TransactionContext transactionContext)
        {
            _transactionContext = transactionContext;
        }

        // 👤 Requires login to access Add
        [HttpGet]
        [Authorize]
        public IActionResult Add()
        {
            ViewBag.Action = "Add";
            ViewBag.TransactionTypes = _transactionContext.TransactionType.OrderBy(t => t.Name).ToList();
            return View("Edit", new Transaction());
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                _transactionContext.Transactions.Add(transaction);
                _transactionContext.SaveChanges();
                return RedirectToAction("Index", "Transactions");
            }
            else
            {
                ViewBag.Action = "Add";
                ViewBag.TransactionTypes = _transactionContext.TransactionType.OrderBy(t => t.Name).ToList();
                return View("Edit", transaction);
            }
        }

        // 👤 Requires login to access Edit
        [HttpGet]
        [Authorize]
        public IActionResult Edit(int id)
        {
            ViewBag.Action = "Edit";
            var transaction = _transactionContext.Transactions.Find(id);
            ViewBag.TransactionTypes = _transactionContext.TransactionType.OrderBy(t => t.Name).ToList();
            return View(transaction);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                _transactionContext.Transactions.Update(transaction);
                _transactionContext.SaveChanges();
                return RedirectToAction("Index", "Transactions");
            }
            else
            {
                ViewBag.Action = "Edit";
                ViewBag.TransactionTypes = _transactionContext.TransactionType.OrderBy(t => t.Name).ToList();
                return View(transaction);
            }
        }

        // 🔐 Only Admin can access Delete
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var transaction = _transactionContext.Transactions.Find(id);
            return View(transaction);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(Transaction transaction)
        {
            _transactionContext.Transactions.Remove(transaction);
            _transactionContext.SaveChanges();
            return RedirectToAction("Index", "Transactions");
        }
    }
}
