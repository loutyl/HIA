using System;
using System.Configuration;
using System.Windows.Forms;
using databaseHIA;
using HIA_client_lourd.Class;

namespace HIA_client_lourd.Forms
{
    public partial class HistoriqueVisite : Form
    {
        private readonly Patient _currentPatient;

        public HistoriqueVisite(Patient patient)
        {
            InitializeComponent();

            _currentPatient = patient;

        }

        private void cmBStatus_TextChanged(object sender, EventArgs e)
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

            FillData(status);
        }

        private void FillData(int status)
        {
            heavyClientDatabaseObject hdb = new heavyClientDatabaseObject(ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString);

            dataGridView2.DataSource = hdb.getHistoriqueVisite(status, _currentPatient.NomPatient);
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
