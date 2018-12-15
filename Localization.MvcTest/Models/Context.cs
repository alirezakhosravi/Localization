using Localization.EntityFramework.Extentions;
using Microsoft.EntityFrameworkCore;

namespace Localization.MvcTest.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyLocalizationRecordConfiguration();
        }
    }
}
