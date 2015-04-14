using System.Data.Entity;

namespace Hovis.Web.Base
{
    /// <summary>
    /// Seeds Database with data
    /// </summary>
    public class ApplicationDbInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            //seed identity
            //IdentityConfig.SeedRoles();
            //IdentityConfig.SeedUsers();

            //could seed other data here if required by application

            base.Seed(context);
        }
    }
}