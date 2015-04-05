using System.Configuration;
using System.Windows.Forms;
using databaseHIA;
using HIA_client_lourd.Class;

namespace HIA_client_lourd.Forms
{
    public partial class PreListe : Form
    {
        private readonly Patient _currentPatient;

        public PreListe(Patient patient)
        {
            this.InitializeComponent();

            _currentPatient = patient;

            this.FillData();
        }

        private void FillData()
        {
            HeavyClientDatabaseObject hdb = new HeavyClientDatabaseObject(ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString);

            dataGridView1.DataSource = hdb.GetPreListe(_currentPatient.NomPatient, _currentPatient.PrenomPatient);
            dataGridView1.Columns[0].HeaderText = @"Nom du visiteur";
            dataGridView1.Columns[1].HeaderText = @"Prénom du visiteur";
            dataGridView1.Columns[2].HeaderText = @"Numéro de téléphone";
            dataGridView1.Columns[3].HeaderText = @"Adresse Email";

        }
    }
}
