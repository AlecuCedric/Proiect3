using System;
using System.Windows;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace Proiect3
{
    public partial class ChangePasswordWindow : Window
    {
        private string currentUsername;

        // Constructor that accepts the username
        public ChangePasswordWindow(string username)
        {
            InitializeComponent();
            currentUsername = username;
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

            // Update the password in the database
            try
            {
                string connectionString = "Server=G713RS;Database=spitaldb;Trusted_Connection=True;Encrypt=False;";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Call the stored procedure to check if the current password is correct
                    using (SqlCommand cmd = new SqlCommand("CheckCurrentPassword", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        // Add parameters to the stored procedure
                        cmd.Parameters.AddWithValue("@Username", currentUsername);
                        cmd.Parameters.AddWithValue("@CurrentPassword", currentPassword);

                        // Execute the stored procedure and get the result
                        object resultObj = cmd.ExecuteScalar();

                        // Check if the result is null and handle it safely
                        if (resultObj != null)
                        {
                            int result = (int)resultObj;

                            // If result is 0, the current password is incorrect
                            if (result == 0)
                            {
                                MessageBox.Show("Current password is incorrect.");
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Error: Stored procedure returned null.");
                            return;
                        }

                        // If the result is 1, the current password is correct
                        // Update the password
                        string updatePasswordQuery = "UPDATE Users SET PasswordHash = HASHBYTES('SHA2_256', @NewPassword) WHERE Username = @Username";
                        using (SqlCommand updateCmd = new SqlCommand(updatePasswordQuery, conn))
                        {
                            updateCmd.Parameters.AddWithValue("@Username", currentUsername);
                            updateCmd.Parameters.AddWithValue("@NewPassword", newPassword);
                            updateCmd.ExecuteNonQuery();
                        }

                        MessageBox.Show("Password updated successfully!");
                        this.Close(); // Close the change password window after success
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
