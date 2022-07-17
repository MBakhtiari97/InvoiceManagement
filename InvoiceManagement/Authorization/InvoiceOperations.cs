using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.ComponentModel;

namespace InvoiceManagement.Authorization
{
    public class InvoiceOperations
    {
        public static OperationAuthorizationRequirement Create =
            new OperationAuthorizationRequirement
            {
                Name = Constants.CreateOperationName
            };

        public static OperationAuthorizationRequirement Read =
            new OperationAuthorizationRequirement
            {
                Name = Constants.ReadOperationName
            };

        public static OperationAuthorizationRequirement Update =
            new OperationAuthorizationRequirement
            {
                Name = Constants.UpdateOperationName
            };

        public static OperationAuthorizationRequirement Delete =
            new OperationAuthorizationRequirement
            {
                Name = Constants.DeleteOperationName
            };

        public static OperationAuthorizationRequirement Approve =
            new OperationAuthorizationRequirement
            {
                Name = Constants.ApprovedOperationName
            };

        public static OperationAuthorizationRequirement Reject =
            new OperationAuthorizationRequirement
            {
                Name = Constants.RejectedOperationName
            };
    }

    public class Constants
    {
        //CRUD Operations
        public static readonly string CreateOperationName = "Create";
        public static readonly string ReadOperationName = "Read";
        public static readonly string UpdateOperationName = "Update";
        public static readonly string DeleteOperationName = "Delete";

        //Manager Operations
        public static readonly string ApprovedOperationName = "Approve";
        public static readonly string RejectedOperationName = "Reject";

        //Roles
        public static readonly string InvoiceManagersRole = "InvoiceManager";
        public static readonly string InvoiceAdminRole = "InvoiceAdmin";

    }
}
