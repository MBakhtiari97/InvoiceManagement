using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvoiceManagement.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using InvoiceManagement.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace InvoiceManagement.Pages.Invoices
{
    public class CreateModel : DI_BasePageModel
    {
        public CreateModel(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager)
        : base(context, authorizationService, userManager)
        {
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Invoice Invoice { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.Invoices == null || Invoice == null)
            {
                return Page();
            }

            _context.Invoices.Add(Invoice);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
