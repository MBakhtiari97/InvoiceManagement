using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvoiceManagement.Authorization;
using InvoiceManagement.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InvoiceManagement.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace InvoiceManagement.Pages.Invoices
{
    public class DeleteModel : DI_BasePageModel
    {

        public DeleteModel(ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager)
            : base(context, authorizationService, userManager)
        {
        }

        [BindProperty]
      public Invoice Invoice { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || Context.Invoices == null)
            {
                return NotFound();
            }

            var invoice = await Context.Invoices.FirstOrDefaultAsync(m => m.InvoiceId == id);

            if (invoice == null)
            {
                return NotFound();
            }
            else 
            {
                Invoice = invoice;
            }

            //Authorization
            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                User, invoice, InvoiceOperations.Delete);

            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || Context.Invoices == null)
            {
                return NotFound();
            }
            var invoice = await Context.Invoices.FindAsync(id);

            //Authorization
            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                User, invoice, InvoiceOperations.Delete);

            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            if (invoice != null)
            {
                Invoice = invoice;
                Context.Invoices.Remove(Invoice);
                await Context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
