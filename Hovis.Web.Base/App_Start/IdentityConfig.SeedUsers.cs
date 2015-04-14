using Hovis.Web.Base.Identity;
using Hovis.Web.Base.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Web;

namespace Hovis.Web.Base
{
    public static partial class IdentityConfig
    {
        // array with users that will be created for this application on startup
        private static readonly UserToCreate[] UsersToCreate =
        {
            new UserToCreate("Andy", "Taylor", "andy.taylor@hovis.co.uk", "Admin"),

            //other users to create can be created in this array like this:
            //new UserToCreate("John", "Smith", "john.smith@hovis.co.uk", "Admin", "AnotherRole", "AndAnotherRole"),
        };

        public static void SeedUsers()
        {
            var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();

            foreach (var userToCreate in UsersToCreate)
            {
                //check to see if the user already exists
                var user = userManager.FindByEmail(userToCreate.EmailAddress);

                //if user isn't found, create it
                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = userToCreate.EmailAddress,
                        Email = userToCreate.EmailAddress,
                        EmailConfirmed = true, //set confirmed to true by default
                        FirstName = userToCreate.FirstName,
                        LastName = userToCreate.LastName
                    };

                    userManager.Create(user);

                    //don't let this user be locked out
                    userManager.SetLockoutEnabled(user.Id, false);
                }

                // Add user to specified roles
                foreach (var roleToAddUserTo in userToCreate.Roles)
                {
                    var rolesForUser = userManager.GetRoles(user.Id);

                    if (!rolesForUser.Contains(roleToAddUserTo))
                        userManager.AddToRole(user.Id, roleToAddUserTo);
                }
            }
        }

        //private class only used within seeding functionatly to hold info on user
        //makes it easy to use array in here
        private class UserToCreate
        {
            public UserToCreate(string firstName, string lastName, string emailAddress, params string[] roles)
            {
                FirstName = firstName;
                LastName = lastName;
                EmailAddress = emailAddress;
                Roles = roles;
            }

            public string FirstName { get; set; }

            public string LastName { get; set; }

            public string EmailAddress { get; set; }

            public string[] Roles { get; set; }
        }
    }
}