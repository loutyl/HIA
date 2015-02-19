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
    public partial class HistoriqueVisite : Form
    {
        //Le patient actuellement recherché
        private Patient currentPatient;

        public HistoriqueVisite(Patient _patient)
        {
            //Initialisation des composants du FORM
            InitializeComponent();
            //Affection du paramêtre à currentPatient
            currentPatient = _patient;
            
            
        }
        //Evenement lors du changement de text dans le comboBox
        private void cmBStatus_TextChanged(object sender, EventArgs e)
        {
            //Variables correspondant aux status
            int status;
            string pStatus;
            //Récupération du status dans la comboBox
            pStatus = cmBStatus.Text;
            //Swtich 
            switch (pStatus)
            {
                //Cas Fini status prend 0
                case "Fini":
                    status = 0;
                    break;
                //Accepter status = 1
                case "Accepter":
                    status = 1;
                    break;
                //Refuser status = 2
                case "Refuser":
                    status = 2;
                    break;
                //Par défault le status = 0
                default:
                    status = 0;
                    break;
            }
            //On remplis le dataGrid avec la fonction FillData
            FillData(status);

        }
        //Fonction permettant de remplir le dataGrid avec les données en fonction du status
        private void FillData(int status)
        {

            //Ouverture d'une connection à la base de donnée
            SqlConnection connection = new SqlConnection(Properties.Settings.Default.dbConnectionString);
            //Essaie de remplissage des données
            try
            {
                //Ouverture de la connection
                connection.Open();
                //Création d'un adapter
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT DISTINCT PL_nom_visiteur, PL_prenom_visiteur, date_visite, heure_deb_visite" +
                ", heure_fin_visite, num_bon_visite FROM VisitePatient WHERE status_demande = @status AND nom_patient = @pRecherche", connection);
                //Passage par paramêtre des filtres WHERE
                adapter.SelectCommand.Parameters.AddWithValue("@status", status);
                adapter.SelectCommand.Parameters.AddWithValue("@pRecherche", currentPatient.NomPatient);
                //Création d'une datatable
                DataTable table = new DataTable();
                //Remplissage de la datatable 
                adapter.Fill(table);
                //Affection de la source de donnée au dataGrid
                dataGridView2.DataSource = table;
                //Changement des noms des colonnes du dataGrid
                dataGridView2.Columns[0].HeaderText = "Nom du visiteur";
                dataGridView2.Columns[1].HeaderText = "Prénom du visiteur";
                dataGridView2.Columns[2].HeaderText = "Date de la visite";
                dataGridView2.Columns[3].HeaderText = "Heure de début";
                dataGridView2.Columns[4].HeaderText = "Heure de fin";
                dataGridView2.Columns[5].HeaderText = "Numéro de visite";
                //Affichage du dataGrid
                dataGridView2.Visible = true;

            }
            catch (Exception ex)
            {   
                //Affichage du message d'erreur
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
