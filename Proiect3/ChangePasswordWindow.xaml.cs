using System;
using System.Windows;
using Microsoft.Data.SqlClient;

namespace Proiect3
{
    public partial class ChangePasswordWindow : Window
    {
        private string currentUsername;
        private Window previousWindow; // Reference to the previous window
        private string connectionString = "Server=G713RS;Database=spitaldb;Trusted_Connection=True;Encrypt=False;";

        // Constructor that accepts the username and the previous window
        public ChangePasswordWindow(string username, Window previousWindow)
        {
            InitializeComponent();
            this.currentUsername = username;
            this.previousWindow = previousWindow; // Save the reference to reopen later
            txtUsername.Text = currentUsername;  // Display the username in the TextBox
        }

        private void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            string currentPassword = txtCurrentPassword.Password;
            string newPassword = txtNewPassword.Password;
            string confirmPassword = txtConfirmPassword.Password;

            // Check if new password and confirm password match
            if (newPassword != confirmPassword)
            {
                MessageBox.Show("New password and confirm password do not match.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Query to check if the current username and password hash exist in the database
                    string checkQuery = "SELECT COUNT(*) FROM Users WHERE Username = @Username AND PasswordHash = HASHBYTES('SHA2_256', @CurrentPassword)";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@Username", currentUsername);
                        checkCmd.Parameters.AddWithValue("@CurrentPassword", currentPassword);

                        int count = (int)checkCmd.ExecuteScalar();
                        if (count == 0)
                        {
                            MessageBox.Show("Current password is incorrect.");
                            return;
                        }
                    }

                    // If valid, update the password
                    string updateQuery = "UPDATE Users SET PasswordHash = HASHBYTES('SHA2_256', @NewPassword) WHERE Username = @Username";
                    using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
                    {
                        updateCmd.Parameters.AddWithValue("@Username", currentUsername);
                        updateCmd.Parameters.AddWithValue("@NewPassword", newPassword);

                        int rowsAffected = updateCmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Password updated successfully!");

                            // Open the LoginWindow
                            LoginWindow loginWindow = new LoginWindow();
                            loginWindow.Show();

                            // Close the current ChangePasswordWindow
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Failed to update the password. Please try again.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Show the previous window and close the ChangePasswordWindow
            previousWindow.Show();
            this.Close();
        }
    }
}
