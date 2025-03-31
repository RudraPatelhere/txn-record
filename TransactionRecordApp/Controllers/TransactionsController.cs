using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TransactionRecordApp.Models;

namespace TransactionRecordApp.Controllers
{
    /// <summary>
    /// Controller for the Transactions Index (main) page
    /// </summary>
    public class TransactionsController : Controller
    {
        private readonly TransactionContext _transactionContext;

        public TransactionsController(TransactionContext transactionContext)
        {
            _transactionContext = transactionContext;
        }

        /// <summary>
        /// Calls the Transactions Index (main) page and passes all the transactions to it
        /// </summary>
        public IActionResult Index()
        {
            // Get all transactions with their type, ordered by company name
            var transactions = _transactionContext.Transactions
                .Include(t => t.TransactionType)
                .OrderBy(t => t.CompanyName)
                .ToList();

            return View(transactions);
        }
    }
}
