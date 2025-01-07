using System.Windows;

namespace Proiect3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string currentUsername;  // Replace this with actual username

        public MainWindow(string username)
        {
            InitializeComponent();
            currentUsername = username;
        }

        // Redirect to Medic Form (Programarile Mele)
        private void ProgramarileMeleButton_Click(object sender, RoutedEventArgs e)
        {
            MyAppointmentsWindow appointmentsWin = new MyAppointmentsWindow(currentUsername, this);
            // Optionally hide or close the current window:
            this.Hide();
            appointmentsWin.Show();
        }

        // Redirect to Change Password window
        private void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            // From MainWindow or another window
            ChangePasswordWindow changePasswordWindow = new ChangePasswordWindow(currentUsername, this);
            this.Hide(); // Hide the current window
            changePasswordWindow.Show();
        }

        // Log out and return to Login page
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();  // Open the Login page
            this.Close();        // Close the current window
        }

        // Admin Panel Button Clicks - These remain unchanged
        private void MedicForm_Click(object sender, RoutedEventArgs e)
        {
            Medic medicWindow = new Medic();
            medicWindow.Show();  // Open the Medic window
            this.Close();         // Optionally, close the main window
        }

        private void PacientForm_Click(object sender, RoutedEventArgs e)
        {
            Pacient pacientWindow = new Pacient();
            pacientWindow.Show();  // Open the Pacient window
            this.Close();          // Optionally, close the main window
        }

        private void Medicamente_Click(object sender, RoutedEventArgs e)
        {
            Medicamente medicamenteWindow = new Medicamente();
            medicamenteWindow.Show();  // Open the Medicamente window
            this.Close();              // Optionally, close the main window
        }

        private void Consultatie_Click(object sender, RoutedEventArgs e)
        {
            Consultatie consultatieWindow = new Consultatie();
            consultatieWindow.Show();  // Open the Consultatie window
            this.Close();              // Optionally, close the main window
        }
    }
}
