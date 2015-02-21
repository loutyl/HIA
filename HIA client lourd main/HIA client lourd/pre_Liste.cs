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
    public partial class pre_Liste : Form
    {
        private Patient _currentPatient;

        public pre_Liste(Patient patient)
        {
            InitializeComponent();

            this._currentPatient = patient;

            FillData();
        }

        private void FillData()
        {
            heavyClientDatabaseObject hdb = new heavyClientDatabaseObject(ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString);

            dataGridView1.DataSource = hdb.getPreListe(this._currentPatient._NomPatient, this._currentPatient._PrenomPatient);
            dataGridView1.Columns[0].HeaderText = "Nom du visiteur";
            dataGridView1.Columns[1].HeaderText = "Prénom du visiteur";
            dataGridView1.Columns[2].HeaderText = "Numéro de téléphone";
            dataGridView1.Columns[3].HeaderText = "Adresse Email";

        }
    }
}
