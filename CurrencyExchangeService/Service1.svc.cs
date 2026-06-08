using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;

namespace CurrencyExchangeService
{
    public class Service1 : IService1
    {
        public Service1()
        {
            DatabaseManager.InitializeDatabase();
        }

        public void CreateUser(string username)
        {
            DatabaseManager.CreateUser(username);
        }

        public string GetBalances(int userId)
        {
            return DatabaseManager.GetBalances(userId);
        }

        public decimal GetExchangeRate(string currencyCode)
        {
            if (currencyCode.ToUpper() == "PLN")
                return 1;

            string url =
                $"https://api.nbp.pl/api/exchangerates/rates/A/{currencyCode}/?format=json";

            HttpWebRequest request =
                (HttpWebRequest)WebRequest.Create(url);

            HttpWebResponse response =
                (HttpWebResponse)request.GetResponse();

            using (StreamReader reader =
                new StreamReader(response.GetResponseStream()))
            {
                string json = reader.ReadToEnd();

                NbpRate rate =
                    JsonConvert.DeserializeObject<NbpRate>(json);

                return rate.Rates[0].Mid;
            }
        }

        public decimal ConvertToPln(decimal amount, string currencyCode)
        {
            decimal rate = GetExchangeRate(currencyCode);

            if (rate <= 0)
                return -1;

            return amount * rate;
        }

        public decimal ConvertFromPln(decimal amount, string currencyCode)
        {
            decimal rate = GetExchangeRate(currencyCode);

            if (rate <= 0)
                return -1;

            return amount / rate;
        }

        public decimal ConvertCurrency(
            decimal amount,
            string fromCurrency,
            string toCurrency)
        {
            decimal amountInPln =
                ConvertToPln(amount, fromCurrency);

            decimal result =
                ConvertFromPln(amountInPln, toCurrency);

            DatabaseManager.SaveTransaction(
                fromCurrency,
                toCurrency,
                amount,
                result);

            return result;
        }
    }
}