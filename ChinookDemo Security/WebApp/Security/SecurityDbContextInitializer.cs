using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

#region Additional namespces
using System.Data.Entity;
using WebApp.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Configuration;
using Microsoft.AspNet.Identity;
#endregion
namespace WebApp.Security
{
    public class SecurityDbContextInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    {

        protected override void Seed(ApplicationDbContext context)
        {
            #region Seed the roles
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var startupRoles = ConfigurationManager.AppSettings["startupRoles"].Split(';');
            foreach (var role in startupRoles)
                roleManager.Create(new IdentityRole { Name = role });
            #endregion

            #region Seed the users
            string adminUser = ConfigurationManager.AppSettings["adminUserName"];
            string adminRole = ConfigurationManager.AppSettings["adminRole"];
            string adminEmail = ConfigurationManager.AppSettings["adminEmail"];
            string adminPassword = ConfigurationManager.AppSettings["adminPassword"];
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            var result = userManager.Create(new ApplicationUser
            {
                UserName = adminUser,
                Email = adminEmail,
                CustomerId = null,
                EmployeeId = null
            }, adminPassword);
            if (result.Succeeded)
                userManager.AddToRole(userManager.FindByName(adminUser).Id, adminRole);

            //in your program, you will need to seed users
            //example of customer
            string customerUser = "HansenB";
            string customerRole = "Customers";
            string customerEmail = "bjon.hansen@yahoo.no";
            string customerPassword = ConfigurationManager.AppSettings["newUserPassword"];
            int customerid = 4;
            result = userManager.Create(new ApplicationUser
            {
                UserName = customerUser,
                Email = customerEmail,
                CustomerId=customerid,
                EmployeeId=null,
            }, adminPassword);
            if (result.Succeeded)
                userManager.AddToRole(userManager.FindByName(customerUser).Id, customerRole);

            //example for an employee
            string employeeUser = "JPeacock";
            string employeeRole = "Employees";
            string employeeEmail = "PeacockJ@Chinook.no";
            string employeePassword = ConfigurationManager.AppSettings["newUserPassword"];
            int employeeid = 20;
            result = userManager.Create(new ApplicationUser
            {
                UserName = employeeUser,
                Email = employeeEmail,
                CustomerId = null,
                EmployeeId = employeeid,
            }, employeePassword);
            if (result.Succeeded)
                userManager.AddToRole(userManager.FindByName(employeeUser).Id, employeeRole);

            #endregion

            // ... etc. ...

            base.Seed(context);
        }
    }
}