using System.ServiceModel;

namespace CurrencyExchangeService
{
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        decimal GetExchangeRate(string currencyCode);

        [OperationContract]
        decimal ConvertToPln(decimal amount, string currencyCode);

        [OperationContract]
        decimal ConvertFromPln(decimal amount, string currencyCode);

        [OperationContract]
        decimal ConvertCurrency(
            decimal amount,
            string fromCurrency,
            string toCurrency);

        [OperationContract]
        void CreateUser(string username);

        [OperationContract]
        string GetBalances(int userId);

       
    }
}