using System.Windows;
namespace HIA_client_lourd.Forms
{
    public partial class PreListe : System.Windows.Forms.Form
    {
        private readonly Class.Patient _currentPatient;

        public PreListe(Class.Patient patient)
        {
            this.InitializeComponent();

            _currentPatient = patient;

            this.FillData();
        }

        private void FillData()
        {
            databaseHIA.HeavyClientDatabaseObject hdb = new databaseHIA.HeavyClientDatabaseObject(System.Configuration.ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString);

            dataGridView1.DataSource = hdb.GetPreListe(_currentPatient.NomPatient, _currentPatient.PrenomPatient, System.Convert.ToInt32(_currentPatient.IdPatient));

            if (dataGridView1.Rows.Count <= 0){
                MessageBox.Show("Il y a eu un problème pour recupérer les données.");
                return;
            }
                
            dataGridView1.Columns[0].HeaderText = @"Nom du visiteur";
            dataGridView1.Columns[1].HeaderText = @"Prénom du visiteur";
            dataGridView1.Columns[2].HeaderText = @"Numéro de téléphone";
            dataGridView1.Columns[3].HeaderText = @"Adresse Email";

        }
    }
}
