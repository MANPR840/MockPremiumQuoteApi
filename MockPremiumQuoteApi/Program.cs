using Microsoft.EntityFrameworkCore;
using MockPremiumQuoteApi;


var MyAllowSpecificOrigins = "_allowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MockPremiumQuoteDb>(opt => opt.UseInMemoryDatabase("QuoteList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
string? allowedHosts = builder.Configuration["AllowedHosts"];

builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
                         policy =>
                         {
                             policy.WithOrigins(allowedHosts)
                                                 .AllowAnyHeader()
                                                 .AllowAnyMethod();
                         });
});



var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(MyAllowSpecificOrigins);

// Quote Retreival
app.MapGet("/premiumquotes/{id}", async (Guid id, MockPremiumQuoteDb db) =>
    await db.PremiumQuotes.FindAsync(id)
        is MockPremiumQuote quote
            ? Results.Ok(quote)
            : Results.NotFound());


// Quote Submission
app.MapPost("/premiumquotes", async (MockQuoteRequest request, MockPremiumQuoteDb db) =>
{

    MockPremiumQuote quote = new()
    {
        CoverageAmount = request.CoverageAmount,
        PolicyTerm = request.policyTerm
    };

    float value = request.CoverageAmount;
   
    quote.Premiums = [
        new MockPremium{ CompanyCode = "ABC001", Premium = (float)((value * 2.5)/100) },
        new MockPremium{ CompanyCode = "STQ002", Premium = (float)((value * 1.5)/100) },
        new MockPremium{ CompanyCode = "QRJ003", Premium = (float)((value * 0.5)/100) },
        new MockPremium{ CompanyCode = "JKL004", Premium = (float)((value * 0.2)/100) }
    ];

     
    db.PremiumQuotes.Add(quote);
    await db.SaveChangesAsync();

    MockQuoteResult result = new()
    {
        QuoteId = quote.QuoteId
    };

    return Results.Created($"/premiumquotes/{quote.QuoteId}",result);
});



app.Run();