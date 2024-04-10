using Buffet_Galina_WPF.API;
using Buffet_Galina_WPF.DTO;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Buffet_Galina_WPF
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public AdminDTO Admin { get; set; }
        public string Password { get; set; }

        public LoginWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }

        private async void Admin_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                var admin = await Client.Instance.LoginAdmin(Admin, Password);
                AdminWindow adminWindow = new AdminWindow(admin);
                adminWindow.Show();
                //MessageBox.Show("Пиривет Лох Безденежный");
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            //string login = "Admin";
            //int pass = 1;
            //if (Pass == pass.ToString() && Login == login)
            //{
            //    new AdminWindow().Show();
            //    Close();
            //}
            //else
            //{
            //    MessageBox.Show("Неверный логин или пароль. Попробуйте снова.", "Ок");
            //}
        }

        private void keyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Admin_Click(this, null);
        }
    }
}
