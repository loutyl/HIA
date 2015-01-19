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

namespace HIA_client_lourd
{
    public partial class pre_Liste : Form
    {
        //Le patient de la recherche
        private Patient currentPatient;
        public pre_Liste(Patient _patient)
        {
            //Initialisation des composants du Form
            InitializeComponent();
            //Affectation du paramêtre à currentPatient
            currentPatient = _patient;
            //Remplissage des données
            FillData();
        }

         //Fonction de remplissage des données
        private void FillData()
        {
            //Ouverture d'une connection à la base de donnée
            SqlConnection connection = new SqlConnection(Properties.Settings.Default.dbConnectionString);

            //Essaie de remplissage des données
            try
            {
                //Ouverture de la connection
                connection.Open();
                //Création d'un adapter avec en paramêtre la requête
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT DISTINCT PL_nom_visiteur, PL_prenom_visiteur, PL_tel_visiteur, PL_email_visiteur"+ 
                " FROM preListeVisiteurPatient WHERE nom_patient = @nom_patient OR prenom_patient = @prenom_patient", connection);
                //Passage en paramêtre des filtres WHERE
                adapter.SelectCommand.Parameters.AddWithValue("@nom_patient", currentPatient.NomPatient);
                adapter.SelectCommand.Parameters.AddWithValue("@prenom_patient", currentPatient.PrenomPatient);
                
                //Création d'une datatable
                DataTable table = new DataTable();
                //Remplissage de la datatable avec l'adapter
                adapter.Fill(table);
                //Affectation de la source donnée du dataGrid par la datatable
                dataGridView1.DataSource = table;
               //Changement du nom des colonnes 
                dataGridView1.Columns[0].HeaderText = "Nom du visiteur";
                dataGridView1.Columns[1].HeaderText = "Prénom du visiteur";
                dataGridView1.Columns[2].HeaderText = "Numéro de téléphone";
                dataGridView1.Columns[3].HeaderText = "Adresse Email";

            }
            catch (Exception ex)
            {
                //Affichage de message d'erreur
                MessageBox.Show(ex.Message);
            
            }
            finally
            {
                //Fermeture de la connection à la base de donnée
                connection.Close();
            }

        }
 
    }
}
