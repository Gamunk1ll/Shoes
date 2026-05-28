using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Shoes
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var DB = new ShoesContext();
                var login = UserLoginTextBox.Text;
                var password = UserPasswordPasswordBox.Password;
                var result = DB.Users.Where(x=>x.Login ==  login && x.Password==password).FirstOrDefault();
                if (result != null)
                {
                    MessageBox.Show("Вы успешно авторизировались!");
                    UserSingleton.GetUser = result;
                    new ListProductWindow().Show();
                    Close();
                }
                else 
                {
                    MessageBox.Show("Неверный логин или пароль!", "Ошибка!", MessageBoxButton.OK,MessageBoxImage.Error);
                }
            }
            catch 
            {
                MessageBox.Show("Произошла ошибка");
            }
        }

        private void GuestButton_Click(object sender, RoutedEventArgs e)
        {
            new ListProductWindow().Show();
            Close();
        }
    }
}