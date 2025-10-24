using Microsoft.EntityFrameworkCore;

namespace MockPremiumQuoteApi
{
    class MockPremiumQuoteDb : DbContext
    {
        public MockPremiumQuoteDb(DbContextOptions<MockPremiumQuoteDb> options)
            : base(options) { }

        public DbSet<MockPremiumQuote> PremiumQuotes => Set<MockPremiumQuote>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MockPremiumQuote>().OwnsMany(p => p.Premiums, a =>
            {
                a.WithOwner().HasForeignKey("OwnerId");
                a.Property<Guid>("Id");
                a.HasKey("Id");
            });
        }
    }
}