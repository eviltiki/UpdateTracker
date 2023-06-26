using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using UpdateTracker.Models;

namespace UpdateTracker
{
    public class ApplicationViewModel
    {
        public ObservableCollection<Product> Products { get; set; }
        private ServiceBroker serviceBroker;
        private string connectionString;
        private string command;
        private DateTime appStartTime;
        public ApplicationViewModel(string connectionString, DateTime AppStartTime)
        {
            this.connectionString = connectionString;
            this.appStartTime = AppStartTime;
            this.command = $"SELECT Id FROM dbo.Product";

            Products = new ObservableCollection<Product>();
            serviceBroker = new ServiceBroker(connectionString, command);

            serviceBroker.OnMessageSent += new ServiceBroker.MessageHandler(LoadDataIntoList);

            serviceBroker.StartListen();

        }

        public void LoadDataIntoList(object sender, string Message)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand($"SELECT TOP 1 * FROM dbo.Product WHERE Date >= '{appStartTime}' ORDER BY Id DESC", connection);

                connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                int id;
                string productName;
                double productPrice;
                DateTimeOffset date;

                while (reader.Read())
                {
                    id = Convert.ToInt32(reader["Id"]);
                    productName = Convert.ToString(reader["Name"]);
                    productPrice = Convert.ToDouble(reader["Price"]);
                    date = Convert.ToDateTime(Convert.ToString(reader["Date"]));

                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        Products.Add(new Product { Id = id, Name = productName, Price = productPrice, Date = date });
                    });

                }
            }
            
            serviceBroker.StartListen();
        }
    }
}
