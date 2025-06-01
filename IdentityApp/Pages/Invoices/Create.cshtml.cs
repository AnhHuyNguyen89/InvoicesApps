using IdentityApp.Authorization;
using IdentityApp.Data;
using IdentityApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityApp.Pages.Invoices
{
    public class CreateModel : DI_BasePageModel
    {
        private readonly IdentityApp.Data.ApplicationDbContext _context;

        public CreateModel(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager)
            : base(context, authorizationService, userManager)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Invoice Invoice { get; set; }

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            Invoice.CreatorId = UserManager.GetUserId(User);

            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                User, Invoice, InvoiceOperations.Create);

            if (isAuthorized.Succeeded == false)
                return Forbid();

            Context.Invoice.Add(Invoice);
            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
