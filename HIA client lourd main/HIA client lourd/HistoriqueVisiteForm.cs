using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using databaseHIA;
using System.Configuration;

namespace HIA_client_lourd
{
    public partial class HistoriqueVisite : Form
    {
        private Patient _currentPatient;

        public HistoriqueVisite(Patient patient)
        {
            InitializeComponent();

            this._currentPatient = patient;

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

            dataGridView2.DataSource = hdb.getHistoriqueVisite(status, this._currentPatient._NomPatient);
            dataGridView2.Columns[0].HeaderText = "Nom du visiteur";
            dataGridView2.Columns[1].HeaderText = "Prénom du visiteur";
            dataGridView2.Columns[2].HeaderText = "Date de la visite";
            dataGridView2.Columns[3].HeaderText = "Heure de début";
            dataGridView2.Columns[4].HeaderText = "Heure de fin";
            dataGridView2.Columns[5].HeaderText = "Numéro de visite";

            dataGridView2.Visible = true;

        }
    }
}
