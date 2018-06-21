using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WeatherWebApplication.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        //public DbSet<City> city { get; set; }

        //public DbSet<CityChannelMapping> CityChannelMapping { get; set; }

        //public DbSet<query> channel { get; set; }

        public DbSet<CityWeather> cityWeather { get; set; }

        public DbSet<query> query { get; set; }

        //public DbSet<CityWeatherMapping> cityWeatherMapping { get; set; }

        //public DbSet<Condition> condition { get; set; }

        ////public DbSet<Forecast> forecast { get; set; }

        //public DbSet<Image> image { get; set; }

        //public DbSet<ImageChannelMapping> imageChannelMapping { get; set; }

        //public DbSet<Item> item { get; set; }

        //public DbSet<ItemChannelMapping> itemChannelMapping { get; set; }

        //public DbSet<ItemConditionMapping> itemConditionMapping { get; set; }

        //public DbSet<ItemForecastMapping> itemForecastMapping { get; set; }


    }
}