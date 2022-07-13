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
    public class DetailsModel : PageModel
    {
        private readonly InvoiceManagement.Data.ApplicationDbContext _context;

        public DetailsModel(InvoiceManagement.Data.ApplicationDbContext context)
        {
            _context = context;
        }

      public Invoice Invoice { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Invoices == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices.FirstOrDefaultAsync(m => m.InvoiceId == id);
            if (invoice == null)
            {
                return NotFound();
            }
            else 
            {
                Invoice = invoice;
            }
            return Page();
        }
    }
}
