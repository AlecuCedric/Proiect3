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

namespace Proiect3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MedicForm_Click(object sender, RoutedEventArgs e)
        {
            Medic medicWindow = new Medic();
            medicWindow.Show();  // Open the Medic window
            this.Close();         // Optionally, close the main window
        }

        private void PacientForm_Click(object sender, RoutedEventArgs e)
        {
            Pacient pacientWindow = new Pacient();
            pacientWindow.Show();
            this.Close();
        }

        private void Medicamente_Click(object sender, RoutedEventArgs e)
        {
            Medicamente medicamenteWindow = new Medicamente();
            medicamenteWindow.Show();
            this.Close();
        }

        private void Consultatie_Click(object sender, RoutedEventArgs e)
        {
            Consultatie consultatieWindow = new Consultatie();
            consultatieWindow.Show();
            this.Close();
        }
    }
}