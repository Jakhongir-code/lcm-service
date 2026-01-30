using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Numerics;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Checks if the string represents a natural number (only digits, > 0)
bool IsNaturalNumber(string value)
{
    if (string.IsNullOrEmpty(value))
        return false;

    foreach (char c in value)
    {
        if (!char.IsDigit(c))
            return false;
    }

    // Exclude "0", "00", etc.
    return value.TrimStart('0').Length > 0;
}

BigInteger GCD(BigInteger a, BigInteger b)
{
    while (b != 0)
    {
        var temp = b;
        b = a % b;
        a = temp;
    }
    return a;
}

BigInteger LCM(BigInteger x, BigInteger y)
{
    // Overflow-safe formula
    return (x / GCD(x, y)) * y;
}

// Catch-all route to allow any path ending with email
app.MapGet("/{*path}", async (HttpContext context) =>
{
    var xParam = context.Request.Query["x"].ToString();
    var yParam = context.Request.Query["y"].ToString();

    context.Response.ContentType = "text/plain";

    if (!IsNaturalNumber(xParam) || !IsNaturalNumber(yParam))
    {
        await context.Response.WriteAsync("NaN");
        return;
    }

    BigInteger x = BigInteger.Parse(xParam);
    BigInteger y = BigInteger.Parse(yParam);

    var result = LCM(x, y);

    await context.Response.WriteAsync(result.ToString());
});

app.Run();
