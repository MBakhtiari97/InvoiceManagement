using InvoiceManagement.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace InvoiceManagement.Authorization
{
    public class InvoiceManagerAuthorizationHandler
        : AuthorizationHandler<OperationAuthorizationRequirement, Invoice>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            OperationAuthorizationRequirement requirement,
            Invoice invoice)
        {
            if (context.User == null || invoice == null)
                return Task.CompletedTask;

            //if the operation name wasnt one of the manager operations names then task is completed and operation is aborted
            if (requirement.Name != Constants.ApprovedOperationName &&
                requirement.Name != Constants.RejectedOperationName)
            {
                return Task.CompletedTask;
            }

            if (context.User.IsInRole(Constants.InvoiceManagersRole))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
