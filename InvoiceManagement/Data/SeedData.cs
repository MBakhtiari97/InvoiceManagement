using InvoiceManagement.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design.Internal;

namespace InvoiceManagement.Data
{
    public class SeedData
    {
        public static async Task Initialize(
            IServiceProvider serviceProvider,
            string password = "Test@123")
        {
            //cannot use DI here so using service provider to get context
            using (var context = new ApplicationDbContext(
                       serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                //Manager
                // Ensuring the user and role are existed 
                var managerUid = await EnsureUser(serviceProvider, "manager@demo.com", password);
                await EnsureRole(serviceProvider, managerUid, Constants.InvoiceManagersRole);

                //Admin
                var adminUid = await EnsureUser(serviceProvider, "admin@demo.com", password);
                await EnsureRole(serviceProvider, adminUid, Constants.InvoiceAdminRole);
            }
        }

        //Want to ensure the user is existed
        private static async Task<string> EnsureUser(
            IServiceProvider serviceProvider,
            string userName, string initPw)
        {
            //first of all getting the user manager from service provider , get service of type user manager of type identity user
            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            //getting the user from user manager
            var user = await userManager.FindByNameAsync(userName);

            //if the user does not existed 
            if (user == null)
            {
                //create new user
                user = new IdentityUser
                {
                    UserName = userName,
                    Email = userName,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, initPw);
            }

            //if still user does not existed , then it certainly has problem with password policy
            if (user == null)
            {
                throw new Exception("Password Policy Problem , Creating User Aborted !");
            }

            return user.Id;
        }

        private static async Task<IdentityResult> EnsureRole(
            IServiceProvider serviceProvider,
            string uId, string role)
        {
            //getting the role manager from the service provider
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            //create a new variable of type identity result
            IdentityResult ir;

            //if the role does not existed
            if (!await roleManager.RoleExistsAsync(role))
            {
                //creating the role
                ir = await roleManager.CreateAsync(new IdentityRole(role));
            }

            //getting the userId
            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            //getting the user
            var user = await userManager.FindByIdAsync(uId);

            //Checking if the user was null then throw an exception
            if (user == null)
                throw new Exception("User does not exists");

            //adding the user to specific role
            ir = await userManager.AddToRoleAsync(user, role);

            //returning the identity result
            return ir;
        }
    }
}
