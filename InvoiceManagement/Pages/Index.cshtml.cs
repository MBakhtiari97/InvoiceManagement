using InvoiceManagement.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InvoiceManagement.Pages
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {

        public Dictionary<string, int> revenue;
        private readonly ILogger<IndexModel> _logger;
        private readonly ApplicationDbContext _context;

        public IndexModel( ILogger<IndexModel> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public void OnGet()
        {
            revenue = new Dictionary<string, int>()
            {
                { "January", 0 },
                { "February", 0 },
                { "March", 0 },
                { "April", 0 },
                { "May", 0 },
                { "June", 0 },
                { "July", 0 },
                { "August", 0 },
                { "September", 0 },
                { "October", 0 },
                { "November", 0 },
                { "December", 0 }
            };

            var invoices = _context.Invoices.ToList();
            foreach (var invoice in invoices)
            {
                revenue[invoice.InvoiceMonth] += (int)invoice.InvoiceAmount;
            }
        }
    }
}