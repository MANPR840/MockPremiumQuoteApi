using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MockPremiumQuoteApi
{
    public class MockPremium
    {
        [Key]
        public string? CompanyCode { get; set; }
        public float Premium { get; set; }
    }

}