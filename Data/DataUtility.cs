using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using WebBlog.Models;

namespace WebBlog.Data
{
    public static class DataUtility
    {
        private const string _adminRole = "Administrator";
        private const string _modRole = "Moderator";
        private const string _adminEmail = "admin@Mailinator.com";
        private const string _modEmail = "moderator@Mailinator.com";




        public static string GetConnectionString(IConfiguration configuration)
        {
            string? connectionString = configuration.GetConnectionString("DefaultConnection"); //Local Connection String
            string? databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL"); //Remote Connection String?! Railway / Rail Way?!


            return string.IsNullOrEmpty(databaseUrl) ? connectionString : BuildConnectionString(databaseUrl);
        }

        private static string BuildConnectionString(string databaseUrl)
        {
            var databaseUri = new Uri(databaseUrl);
            var userInfo = databaseUri.UserInfo.Split(':');
            var builder = new NpgsqlConnectionStringBuilder()
            {
                Host = databaseUri.Host,
                Port = databaseUri.Port,
                Username = userInfo[0],
                Password = userInfo[1],
                Database = databaseUri.LocalPath.TrimStart('/'),
                SslMode = SslMode.Require,
                TrustServerCertificate = true
            };

            return builder.ToString();
        }


        public static async Task ManageDataAsync(IServiceProvider svcProvider)
        {
            // Obtaining the necessary services based on the IServiceProvider parameter
            var dbContextSvc = svcProvider.GetRequiredService<ApplicationDbContext>();

            var configurationSvc = svcProvider.GetRequiredService<IConfiguration>();

            var userManagerSvc = svcProvider.GetRequiredService<UserManager<BlogUser>>();

            var roleManagerSvc = svcProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Align the database by checking Migrations
            await dbContextSvc.Database.MigrateAsync();

            //Seed Demo User
           //await SeedDemoUserAsync(userManagerSvc);


            // Seed Default Roles
            await SeedRolesAsync(roleManagerSvc);


            // Seed Default Users
            await SeedUsersAsync(dbContextSvc, configurationSvc, userManagerSvc);

        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync(_adminRole))
            {
                await roleManager.CreateAsync(new IdentityRole(_adminRole));
            }

            if (!await roleManager.RoleExistsAsync(_modRole))
            {
                await roleManager.CreateAsync(new IdentityRole(_modRole));
            }

        }

        private static async Task SeedUsersAsync(ApplicationDbContext context, IConfiguration configuration, UserManager<BlogUser> userManager)
        {

            try
            {
                // Seed Amin
                if (!context.Users.Any(u => u.Email == _adminEmail))
                {
                    BlogUser adminUser = new()
                    {
                        Email = _adminEmail,
                        UserName = _adminEmail,
                        FirstName = "Tushar",
                        LastName = "Saini",
                        EmailConfirmed = true
                    };

                    await userManager.CreateAsync(adminUser, configuration["AdminPassword"] ?? Environment.GetEnvironmentVariable("AdminPassword"));
                    await userManager.AddToRoleAsync(adminUser, _adminRole);

                }

                // Seed Moderator
                if (!context.Users.Any(u => u.Email == _modEmail))
                {
                    BlogUser modUser = new()
                    {
                        Email = _modEmail,
                        UserName = _modEmail,
                        FirstName = "Mod",
                        LastName = "Man",
                        EmailConfirmed = true
                    };

                    await userManager.CreateAsync(modUser, configuration["ModeratorPassword"] ?? Environment.GetEnvironmentVariable("ModeratorPassword"));
                    await userManager.AddToRoleAsync(modUser, _modRole);

                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
