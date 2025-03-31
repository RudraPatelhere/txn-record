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
        private TransactionContext _tranactionContext;

        public TransactionController(TransactionContext transactionConatext)
        {
            _tranactionContext = transactionConatext;
        }

        // 👤 Requires login to access Add
        [HttpGet]
        [Authorize]
        public IActionResult Add()
        {
            ViewBag.Action = "Add";
            ViewBag.TransactionTypes = _tranactionContext.TransactionType.OrderBy(t => t.Name).ToList();
            return View("Edit", new Transaction());
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                _tranactionContext.Transactions.Add(transaction);
                _tranactionContext.SaveChanges();
                return RedirectToAction("Index", "Transactions");
            }
            else
            {
                ViewBag.Action = "Add";
                return View("Edit", transaction);
            }
        }

        // 👤 Requires login to access Edit
        [HttpGet]
        [Authorize]
        public IActionResult Edit(int id)
        {
            ViewBag.Action = "Edit";
            var transaction = _tranactionContext.Transactions.Find(id);
            ViewBag.TransactionTypes = _tranactionContext.TransactionType.OrderBy(t => t.Name).ToList();
            return View(transaction);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                _tranactionContext.Transactions.Update(transaction);
                _tranactionContext.SaveChanges();
                return RedirectToAction("Index", "Transactions");
            }
            else
            {
                ViewBag.Action = "Edit";
                ViewBag.TransactionTypes = _tranactionContext.TransactionType.OrderBy(t => t.Name).ToList();
                return View(transaction);
            }
        }

        // 🔐 Only Admin can access Delete
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var transaction = _tranactionContext.Transactions.Find(id);
            return View(transaction);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(Transaction transaction)
        {
            _tranactionContext.Transactions.Remove(transaction);
            _tranactionContext.SaveChanges();
            return RedirectToAction("Index", "Transactions");
        }
    }
}
