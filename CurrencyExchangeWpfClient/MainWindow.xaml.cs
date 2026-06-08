using System;
using System.Windows;

namespace CurrencyExchangeWpfClient
{
    public partial class MainWindow : Window
    {
        private void CreateUserButton_Click(
    object sender,
    RoutedEventArgs e)
        {
            try
            {
                ServiceReference1.Service1Client client =
                    new ServiceReference1.Service1Client();

                client.CreateUserAsync(
                    UsernameTextBox.Text).Wait();

                ResultTextBlock.Text =
                    "User created successfully";
            }
            catch (Exception ex)
            {
                ResultTextBlock.Text =
                    ex.Message;
            }
        }
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ConvertButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ServiceReference1.Service1Client client =
                    new ServiceReference1.Service1Client();

                decimal amount =
                    decimal.Parse(AmountTextBox.Text);

                string fromCurrency =
                    FromCurrencyTextBox.Text.ToUpper();

                string toCurrency =
                    ToCurrencyTextBox.Text.ToUpper();

                decimal result =
                    client.ConvertCurrencyAsync(
                        amount,
                        fromCurrency,
                        toCurrency).Result;

                ResultTextBlock.Text =
                    $"{amount} {fromCurrency} = {result:F2} {toCurrency}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}