using System;

namespace CurrencyClient
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceReference1.Service1Client client =
                new ServiceReference1.Service1Client();

            Console.Write("Enter currency code (USD, EUR, GBP, CHF): ");
            string currencyCode = Console.ReadLine();

            decimal rate =
                client.GetExchangeRateAsync(currencyCode.ToUpper()).Result;

            Console.WriteLine();
            Console.WriteLine($"1 {currencyCode.ToUpper()} = {rate} PLN");

            Console.ReadLine();
        }
    }
}