using ElectionWeb.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ElectionWeb.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<StateRegion> StateRegions { get; set; }
        public DbSet<Constituency> Constituencies { get; set; }
        public DbSet<LGA> lGAs  { get; set; }
        public DbSet<Ward> Wards  { get; set; }
        public DbSet<PollingUnit> PollingUnits  { get; set; }
        public DbSet<Result> Results  { get; set; }
        public DbSet<PartyResult> PartyResults  { get; set; }
    }
}