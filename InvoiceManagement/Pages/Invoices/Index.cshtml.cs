using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InvoiceManagement.Data.Models;

namespace InvoiceManagement.Pages.Invoices
{
    public class IndexModel : PageModel
    {
        private readonly InvoiceManagement.Data.ApplicationDbContext _context;

        public IndexModel(InvoiceManagement.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Invoice> Invoice { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Invoices != null)
            {
                Invoice = await _context.Invoices.ToListAsync();
            }
        }
    }
}
