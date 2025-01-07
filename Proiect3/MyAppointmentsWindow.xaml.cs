using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Windows;

namespace Proiect3
{
    public partial class MyAppointmentsWindow : Window
    {
        private readonly string connectionString = "Server=G713RS;Database=spitaldb;Trusted_Connection=True;Encrypt=False;";
        private readonly string username;
        private readonly Window previousWindow;

        public MyAppointmentsWindow(string username, Window previousWindow)
        {
            InitializeComponent();
            this.username = username;
            this.previousWindow = previousWindow;

            LoadData();
        }

        /// <summary>
        /// Loads Consultatie records for the user (medic or pacient), depending on their role.
        /// </summary>
        private void LoadData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // 1. Determine the user's role: 'Medic' or 'Pacient' (or something else).
                    string roleQuery = "SELECT Role FROM Users WHERE Username = @Username";
                    using (SqlCommand roleCmd = new SqlCommand(roleQuery, conn))
                    {
                        roleCmd.Parameters.AddWithValue("@Username", username);

                        object roleObj = roleCmd.ExecuteScalar();
                        if (roleObj == null)
                        {
                            MessageBox.Show("Could not determine user role. No data to display.");
                            return;
                        }

                        string userRole = roleObj.ToString();

                        // 2. Build the SQL query based on the user role.
                        string appointmentQuery;

                        if (userRole.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                        {
                            // Show appointments for the medic (filter by Medic.UserID).
                            appointmentQuery = @"
                                SELECT 
                                    c.ConsultID,
                                    CONCAT(m.NumeMedic, ' ', m.PrenumeMedic) AS Medic,
                                    CONCAT(p.NumePacient, ' ', p.PrenumePacient) AS Pacient,
                                    c.Data,
                                    c.Diagnostic,
                                    med.Denumire AS Medicament,
                                    c.DozaMedicament
                                FROM Consultatie c
                                JOIN Medic m ON c.MedicID = m.MedicID
                                JOIN Pacient p ON c.PacientID = p.PacientID
                                JOIN Medicamente med ON c.MedicamentID = med.MedicamentID
                                JOIN Users u ON m.UserID = u.UserID
                                WHERE u.Username = @Username;
                            ";
                        }
                        else if (userRole.Equals("User", StringComparison.OrdinalIgnoreCase))
                        {
                            // Show appointments for the pacient (filter by Pacient.UserID).
                            appointmentQuery = @"
                                SELECT 
                                    c.ConsultID,
                                    CONCAT(m.NumeMedic, ' ', m.PrenumeMedic) AS Medic,
                                    CONCAT(p.NumePacient, ' ', p.PrenumePacient) AS Pacient,
                                    c.Data,
                                    c.Diagnostic,
                                    med.Denumire AS Medicament,
                                    c.DozaMedicament
                                FROM Consultatie c
                                JOIN Medic m ON c.MedicID = m.MedicID
                                JOIN Pacient p ON c.PacientID = p.PacientID
                                JOIN Medicamente med ON c.MedicamentID = med.MedicamentID
                                JOIN Users u ON p.UserID = u.UserID
                                WHERE u.Username = @Username;
                            ";
                        }
                        else
                        {
                            // If needed, handle an 'Admin' or other roles differently
                            MessageBox.Show($"No appointment view available for role: {userRole}");
                            return;
                        }

                        // 3. Execute the appropriate query to load appointments.
                        using (SqlDataAdapter dataAdapter = new SqlDataAdapter(appointmentQuery, conn))
                        {
                            dataAdapter.SelectCommand.Parameters.AddWithValue("@Username", username);

                            DataTable dataTable = new DataTable();
                            dataAdapter.Fill(dataTable);

                            dataGridAppointments.ItemsSource = dataTable.DefaultView;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Refreshes the data grid by reloading from the database.
        /// </summary>
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        /// <summary>
        /// Closes the current window and returns to the previous window.
        /// </summary>
        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            // Show the previous window again
            previousWindow.Show();
            // Then close this one
            this.Close();
        }
    }
}
