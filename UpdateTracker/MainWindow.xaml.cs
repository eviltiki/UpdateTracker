using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UpdateTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string connectionString = @"Data Source=ARTHUR-PC\ARTHURSQL;
                                                            Initial Catalog=ShopDB;Integrated Security=True;MultipleActiveResultSets=true";

        private DateTimeOffset AppStartTime;


        public MainWindow()
        {
            InitializeComponent();

            AppStartTime = DateTime.Now;

            this.DataContext = new ApplicationViewModel(connectionString, AppStartTime.DateTime);

        }

        void DataWindow_Closing(object sender, CancelEventArgs e)
        {
            SqlDependency.Stop(connectionString);
        }
    }
}
