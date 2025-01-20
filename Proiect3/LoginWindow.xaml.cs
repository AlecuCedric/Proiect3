using Microsoft.Data.SqlClient;
using System.Windows;

namespace Proiect3
{
    public partial class LoginWindow : Window
    {
        private string connectionString = "Server=G713RS;Database=spitaldb;Trusted_Connection=True;Encrypt=False;";
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT Role FROM Users WHERE Username = @Username AND PasswordHash = HASHBYTES('SHA2_256', @Password)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);

                try
                {
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        string role = result.ToString();
                        if (role == "Admin")
                        {
                            MainWindow mainWindow = new MainWindow(username);
                            mainWindow.Show();
                        }
                        else if (role == "User")
                        {
                            RegularUserMainWindow RegularUserMainWindow = new RegularUserMainWindow(username);
                            RegularUserMainWindow.Show();
                        }
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Invalid username or password.",
                            "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Database Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}