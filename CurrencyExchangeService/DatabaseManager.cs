using System;
using System.Data.SQLite;

namespace CurrencyExchangeService
{
    public static class DatabaseManager
    {
        private static string connectionString =
    @"Data Source=C:\Temp\ExchangeOffice.db;Version=3;";
        public static void SaveTransaction(
            string fromCurrency,
            string toCurrency,
            decimal amount,
            decimal resultAmount)
        {
            using (SQLiteConnection connection =
                new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = @"
        INSERT INTO Transactions
        (
            UserId,
            FromCurrency,
            ToCurrency,
            Amount,
            ResultAmount,
            TransactionDate
        )
        VALUES
        (
            1,
            @from,
            @to,
            @amount,
            @result,
            @date
        )";

                SQLiteCommand command =
                    new SQLiteCommand(query, connection);

                command.Parameters.AddWithValue("@from", fromCurrency);
                command.Parameters.AddWithValue("@to", toCurrency);
                command.Parameters.AddWithValue("@amount", amount);
                command.Parameters.AddWithValue("@result", resultAmount);
                command.Parameters.AddWithValue("@date", DateTime.Now.ToString());

                command.ExecuteNonQuery();
            }
        }
        public static void CreateInitialBalances(int userId)
        {
            using (SQLiteConnection connection =
                new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = @"
        INSERT INTO Balances
        (UserId, CurrencyCode, Amount)
        VALUES
        (@userId, @currency, @amount)";

                string[] currencies =
                {
            "PLN",
            "EUR",
            "USD",
            "GBP"
        };

                foreach (string currency in currencies)
                {
                    SQLiteCommand command =
                        new SQLiteCommand(query, connection);

                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.AddWithValue("@currency", currency);

                    if (currency == "PLN")
                        command.Parameters.AddWithValue("@amount", 1000);
                    else
                        command.Parameters.AddWithValue("@amount", 0);

                    command.ExecuteNonQuery();
                }
            }
        }
        public static string GetBalances(int userId)
        {
            using (SQLiteConnection connection =
                new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query =
                    "SELECT CurrencyCode, Amount FROM Balances WHERE UserId = @userId";

                SQLiteCommand command =
                    new SQLiteCommand(query, connection);

                command.Parameters.AddWithValue("@userId", userId);

                SQLiteDataReader reader =
                    command.ExecuteReader();

                string result = "";

                while (reader.Read())
                {
                    result +=
                        reader["CurrencyCode"] + ": " +
                        reader["Amount"] + Environment.NewLine;
                }

                return result;
            }
        }
        public static void CreateUser(string username)
        {
            using (SQLiteConnection connection =
                new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query =
                    "INSERT INTO Users (Username) VALUES (@username)";

                SQLiteCommand command =
                    new SQLiteCommand(query, connection);

                command.Parameters.AddWithValue(
                    "@username",
                    username);

                command.ExecuteNonQuery();

                long userId = connection.LastInsertRowId;

                CreateInitialBalances((int)userId);
            }
        }
        public static void InitializeDatabase()
        {
            using (SQLiteConnection connection =
                new SQLiteConnection(connectionString))
            {
                connection.Open();

                string usersTable = @"
                CREATE TABLE IF NOT EXISTS Users (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT NOT NULL
                );";

                string balancesTable = @"
                CREATE TABLE IF NOT EXISTS Balances (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    UserId INTEGER,
                    CurrencyCode TEXT,
                    Amount REAL
                );";

                string transactionsTable = @"
                CREATE TABLE IF NOT EXISTS Transactions (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    UserId INTEGER,
                    FromCurrency TEXT,
                    ToCurrency TEXT,
                    Amount REAL,
                    ResultAmount REAL,
                    TransactionDate TEXT
                );";

                new SQLiteCommand(usersTable, connection).ExecuteNonQuery();
                new SQLiteCommand(balancesTable, connection).ExecuteNonQuery();
                new SQLiteCommand(transactionsTable, connection).ExecuteNonQuery();
            }
        }
    }
}