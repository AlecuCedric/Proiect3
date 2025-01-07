using System.Windows;

namespace Proiect3
{
    public partial class RegularUserMainWindow : Window
    {
        private string username;

        public RegularUserMainWindow(string username)
        {
            InitializeComponent();
            this.username = username;
            lblWelcome.Content = $"Welcome, {username}!";
        }

        /// <summary>
        /// Opens the window showing the user's appointments.
        /// </summary>
        private void ProgramarileMeleButton_Click(object sender, RoutedEventArgs e)
        {
            MyAppointmentsWindow appointmentsWin = new MyAppointmentsWindow(username, this);
            // Optionally hide or close the current window:
            this.Hide();
            appointmentsWin.Show();
        }

        /// <summary>
        /// Opens a window to allow the user to change their password.
        /// Passes 'this' as the previous window.
        /// </summary>
        private void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePasswordWindow changePasswordWindow = new ChangePasswordWindow(username, this);
            // Hide or close the current window
            this.Hide();
            changePasswordWindow.Show();
        }

        /// <summary>
        /// Logs the user out and returns to the login window.
        /// </summary>
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            // Open the LoginWindow or do other logout logic
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}
