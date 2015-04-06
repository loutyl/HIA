using System.Windows;
namespace HIA_client_lourd.Forms
{
    public partial class HistoriqueVisite : System.Windows.Forms.Form
    {
        private readonly Class.Patient _currentPatient;

        public HistoriqueVisite(Class.Patient patient)
        {
            this.InitializeComponent();

            _currentPatient = patient;

        }

        private void cmBStatus_TextChanged(object sender, System.EventArgs e)
        {
            int status;

            switch (cmBStatus.Text)
            {
                case "Fini":
                    status = 0;
                    break;
                case "Accepter":
                    status = 1;
                    break;
                case "Refuser":
                    status = 2;
                    break;
                default:
                    status = 0;
                    break;
            }

            this.FillData(status);
        }

        private void FillData(int status)
        {
            databaseHIA.HeavyClientDatabaseObject hdb = new databaseHIA.HeavyClientDatabaseObject(System.Configuration.ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString);

            dataGridView2.DataSource = hdb.GetHistoriqueVisite(status, _currentPatient.NomPatient, System.Convert.ToInt32(_currentPatient.IdPatient));

            if (dataGridView2.Rows.Count <= 0)
            {
                MessageBox.Show("Il y a eu un problème pour recupérer les données.");
                return;
            }

            dataGridView2.Columns[0].HeaderText = @"Nom du visiteur";
            dataGridView2.Columns[1].HeaderText = @"Prénom du visiteur";
            dataGridView2.Columns[2].HeaderText = @"Date de la visite";
            dataGridView2.Columns[3].HeaderText = @"Heure de début";
            dataGridView2.Columns[4].HeaderText = @"Heure de fin";
            dataGridView2.Columns[5].HeaderText = @"Numéro de visite";

            dataGridView2.Visible = true;

        }
    }
}
