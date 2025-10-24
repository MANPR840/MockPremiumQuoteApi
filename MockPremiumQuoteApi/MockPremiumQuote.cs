using System.ComponentModel.DataAnnotations;

namespace MockPremiumQuoteApi
{
    public class MockPremiumQuote
    {
        [Key]
        public Guid QuoteId { get; private set; }

        public int PolicyTerm { get; set; }

        public float CoverageAmount { get; set; }

        public List<MockPremium>? Premiums { get; set; }

    }
}
