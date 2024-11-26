using System.Windows;

namespace WpfZooManager
{
    public partial class LoginWindow : Window
    {
        public string Username { get; private set; }
        public string Password { get; private set; }

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            Username = usernameTextBox.Text;
            Password = passwordBox.Password;

            // Close the login window and return to the main window
            this.DialogResult = true;
            this.Close();
        }
    }
}