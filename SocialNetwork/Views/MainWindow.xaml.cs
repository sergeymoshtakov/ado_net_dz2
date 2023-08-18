using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Microsoft.Data.SqlClient;

namespace SocialNetwork
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SqlConnection connection;
        public ObservableCollection<String> columns { get; set; } = new ObservableCollection<String>();
        public MainWindow()
        {
            InitializeComponent();
            connection = null;
            this.DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                connection = new SqlConnection(App.ConnectionString);
                connection.Open();
                loadGroups();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
                Close();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            connection?.Dispose();
        }

        private void CreateUsers_Click(object sender, RoutedEventArgs e)
        {
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = @"
                CREATE TABLE Users (
                    Id          INT NOT NULL PRIMARY KEY,
                    Name        NVARCHAR(50)     NOT NULL,
                    Surname     NVARCHAR(50)     NOT NULL,
                    Gender      BIT              NOT NULL,
                    Status      NVARCHAR(50)     NULL,
                    Birthday    DATE             NOT NULL,
                    Picture     NVARCHAR(50)     NULL
                ) ;";
            try
            {
                command.ExecuteNonQuery();
                MessageBox.Show("Таблица создана");
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Create Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void InsertUsers_Click(object sender, RoutedEventArgs e)
        {
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = @"
                INSERT INTO Users
                    ( Id, Name, Surname, Gender, Status, Birthday, Picture)
                VALUES
                (1, 'Oleg', 'Petrenko', 1, NULL, '2001-01-03', NULL),
                (2, 'Marina', 'Petrova', 0, 'C est la vie', '2002-07-19', 'marina.jpg'),
                (3, 'Petro', 'Kovalchuk', 1, NULL, '1993-05-08', NULL),
                (4, 'Gennady', 'Gorin', 1, NULL, '1954-09-03', 'gena.jpg'),
                (5, 'Igor', 'Hofman', 1, NULL, '1983-01-25' , 'cat.jpg');";
            try
            {
                command.ExecuteNonQuery();
                MessageBox.Show("Таблица заполнена");
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Insert Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CountUsers_Click(object sender, RoutedEventArgs e)
        {
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = @"SELECT COUNT(*) FROM Users";
            try
            {
                int cnt = Convert.ToInt32(command.ExecuteScalar());
                MessageBox.Show($"Всего {cnt} пользователей");
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Selection Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void loadGroups()
        {
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "SELECT * FROM Users";
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    columns.Add($"Id {reader.GetInt32(0)}, Name : {reader.GetString(1)},\n Surname: {reader.GetString(2)},\n Gender: {reader.GetBoolean(3)},\n Birthday: {reader.GetDateTime(5)}");
                }
                reader.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Selection Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
