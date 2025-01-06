using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace Proiect3
{
    /// <summary>
    /// Interaction logic for Consultatie.xaml
    /// </summary>
    public partial class Consultatie : Window
    {
        private string connectionString = "Server=G713RS;Database=spitaldb;Trusted_Connection=True;Encrypt=False;";
        public Consultatie()
        {
            InitializeComponent();
            LoadData();
            LoadComboBoxes();
        }

        private void LoadData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    string query = @"
                SELECT 
                    Consultatie.ConsultID,
                    CONCAT(Medic.NumeMedic, ' ', Medic.PrenumeMedic) AS Medic,
                    CONCAT(Pacient.NumePacient, ' ', Pacient.PrenumePacient) AS Pacient,
                    Consultatie.Data,
                    Consultatie.Diagnostic,
                    Medicamente.Denumire AS Medicament,
                    Consultatie.DozaMedicament
                FROM Consultatie
                JOIN Medic ON Consultatie.MedicID = Medic.MedicID
                JOIN Pacient ON Consultatie.PacientID = Pacient.PacientID
                JOIN Medicamente ON Consultatie.MedicamentID = Medicamente.MedicamentID";

                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, conn);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    dataGridConsultatie.ItemsSource = dataTable.DefaultView;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (datePickerData.SelectedDate == null ||
                MedicComboBox.SelectedValue == null ||
                PacientComboBox.SelectedValue == null ||
                MedicamenteComboBox.SelectedValue == null)
            {
                MessageBox.Show("Completați toate câmpurile!");
                return;
            }

            DateTime dataConsultatie = datePickerData.SelectedDate.Value;
            long medicID = (long)MedicComboBox.SelectedValue;
            long pacientID = (long)PacientComboBox.SelectedValue;
            long medicamentID = (long)MedicamenteComboBox.SelectedValue;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Consultatie (Data, Diagnostic, DozaMedicament, MedicID, PacientID, MedicamentID) " +
                               "VALUES (@Data, @Diagnostic, @DozaMedicament, @MedicID, @PacientID, @MedicamentID)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Data", dataConsultatie);
                cmd.Parameters.AddWithValue("@Diagnostic", txtDiagnostic.Text);
                cmd.Parameters.AddWithValue("@DozaMedicament", txtDozaMedicament.Text);
                cmd.Parameters.AddWithValue("@MedicID", medicID);
                cmd.Parameters.AddWithValue("@PacientID", pacientID);
                cmd.Parameters.AddWithValue("@MedicamentID", medicamentID);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Consultatie adăugată cu succes!");
                LoadData();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridConsultatie.SelectedItem != null)
            {
                DataRowView row = (DataRowView)dataGridConsultatie.SelectedItem;
                int consultID = (int)row["ConsultID"];

                if (datePickerData.SelectedDate == null ||
                    MedicComboBox.SelectedValue == null ||
                    PacientComboBox.SelectedValue == null ||
                    MedicamenteComboBox.SelectedValue == null)
                {
                    MessageBox.Show("Completați toate câmpurile!");
                    return;
                }

                DateTime dataConsultatie = datePickerData.SelectedDate.Value;
                int medicID = (int)MedicComboBox.SelectedValue;
                int pacientID = (int)PacientComboBox.SelectedValue;
                int medicamentID = (int)MedicamenteComboBox.SelectedValue;

                string query = "UPDATE Consultatie SET Data = @Data, Diagnostic = @Diagnostic, DozaMedicament = @DozaMedicament, " +
                               "MedicID = @MedicID, PacientID = @PacientID, MedicamentID = @MedicamentID WHERE ConsultID = @ConsultID";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Data", dataConsultatie);
                    cmd.Parameters.AddWithValue("@Diagnostic", txtDiagnostic.Text);
                    cmd.Parameters.AddWithValue("@DozaMedicament", txtDozaMedicament.Text);
                    cmd.Parameters.AddWithValue("@MedicID", medicID);
                    cmd.Parameters.AddWithValue("@PacientID", pacientID);
                    cmd.Parameters.AddWithValue("@MedicamentID", medicamentID);
                    cmd.Parameters.AddWithValue("@ConsultID", consultID);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    MessageBox.Show("Consultatie actualizată cu succes!");
                    LoadData();
                }
            }
            else
            {
                MessageBox.Show("Selectați un rând pentru editare.");
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Check if a row is selected in the DataGrid
            if (dataGridConsultatie.SelectedItem != null)
            {
                // Cast the selected row into a DataRowView (or the type of your data model)
                DataRowView row = (DataRowView)dataGridConsultatie.SelectedItem;

                // Get the MedicID (primary key) of the selected record
                long ConsultID = (long)row["ConsultID"];  // Adjust if the name is different

                // Ask for confirmation before deleting the record
                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this record?", "Delete Confirmation", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    // Create SQL query for deleting the Medic record
                    string query = "DELETE FROM Consultatie WHERE ConsultID = @ConsultID";

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        SqlCommand cmd = new SqlCommand(query, conn);

                        // Add the MedicID parameter to the query
                        cmd.Parameters.AddWithValue("@ConsultID", ConsultID);

                        try
                        {
                            // Open the connection
                            conn.Open();

                            // Execute the delete query
                            int rowsAffected = cmd.ExecuteNonQuery();

                            // Check if any rows were deleted
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Consultatie deleted successfully.");
                                LoadData();  // Refresh the DataGrid to reflect the changes
                            }
                            else
                            {
                                MessageBox.Show("Consultatie deletion failed. Please try again.");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error: " + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a row to delete.");
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            string username = "current_username";
            MainWindow MainWindow = new MainWindow(username);
            MainWindow.Show();
            this.Close();
        }

        private void LoadComboBoxes()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Load Medici
                SqlDataAdapter medicAdapter = new SqlDataAdapter("SELECT MedicID, CONCAT(NumeMedic, ' ', PrenumeMedic) AS NumeComplet FROM Medic", conn);
                DataTable medicTable = new DataTable();
                medicAdapter.Fill(medicTable);
                MedicComboBox.ItemsSource = medicTable.DefaultView;
                MedicComboBox.DisplayMemberPath = "NumeComplet";
                MedicComboBox.SelectedValuePath = "MedicID";

                // Load Pacienti
                SqlDataAdapter pacientAdapter = new SqlDataAdapter("SELECT PacientID, CONCAT(NumePacient, ' ', PrenumePacient) AS NumeComplet FROM Pacient", conn);
                DataTable pacientTable = new DataTable();
                pacientAdapter.Fill(pacientTable);
                PacientComboBox.ItemsSource = pacientTable.DefaultView;
                PacientComboBox.DisplayMemberPath = "NumeComplet";
                PacientComboBox.SelectedValuePath = "PacientID";

                // Load Medicamente
                SqlDataAdapter medicamenteAdapter = new SqlDataAdapter("SELECT MedicamentID, Denumire FROM Medicamente", conn);
                DataTable medicamenteTable = new DataTable();
                medicamenteAdapter.Fill(medicamenteTable);
                MedicamenteComboBox.ItemsSource = medicamenteTable.DefaultView;
                MedicamenteComboBox.DisplayMemberPath = "Denumire";
                MedicamenteComboBox.SelectedValuePath = "MedicamentID";

                conn.Close();
            }
        }
    }
}
