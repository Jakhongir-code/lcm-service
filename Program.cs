using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
bool IsNaturalNumber(string value)
{
    if (string.IsNullOrWhiteSpace(value))
        return false;
    
    if (!long.TryParse(value.Trim(), out long number))
        return false;
    
    return number > 0 && value.Trim() == number.ToString();
}

long GCD(long a, long b)
{
    while (b != 0)
    {
        long temp = b;
        b = a % b;
        a = temp;
    }
    return a;
}

long LCM(long x, long y)
{
    return Math.Abs(x * y) / GCD(x, y);
}

app.MapGet("/{*path}", async (HttpContext context) =>
{
    string xParam = context.Request.Query["x"].ToString();
    string yParam = context.Request.Query["y"].ToString();
    
    if (!IsNaturalNumber(xParam) || !IsNaturalNumber(yParam))
    {
        context.Response.ContentType = "text/plain";
        await context.Response.WriteAsync("NaN");
        return;
    }
    
    long x = long.Parse(xParam);
    long y = long.Parse(yParam);
    
    long result = LCM(x, y);
    
    context.Response.ContentType = "text/plain";
    await context.Response.WriteAsync(result.ToString());
});

app.Run();
