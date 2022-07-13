using InvoiceManagement.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using IFormFile = Microsoft.AspNetCore.Http.IFormFile;

namespace InvoiceManagement.Authorization
{
    public class InvoiceCreatorAuthorizationHandler :
        AuthorizationHandler<OperationAuthorizationRequirement, Invoice>
    {
        UserManager<IdentityUser> _userManager;
        public InvoiceCreatorAuthorizationHandler(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            OperationAuthorizationRequirement requirement,
            Invoice invoice)
        {
            //if the user or invoice was null task completed and operation aborted
            if (context.User == null || invoice == null)
                return Task.CompletedTask;

            //if the operation name wasnt one of the CRUD operations names then task is completed and operation is aborted
            if (requirement.Name != Constants.CreateOperationName &&
                requirement.Name != Constants.ReadOperationName &&
                requirement.Name != Constants.UpdateOperationName &&
                requirement.Name != Constants.DeleteOperationName)
            {
                return Task.CompletedTask;
            }

            //if the invoice user id was equal to current logged in user id then succeed the operation , the only valid condition
            if (invoice.CreatorId == _userManager.GetUserId(context.User))
            {
                context.Succeed(requirement);
            }

            //anything else happening then the operation is aborted and task completed  
            return Task.CompletedTask;

        }
    }
}
