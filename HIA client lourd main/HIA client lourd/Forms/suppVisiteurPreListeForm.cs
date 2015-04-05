using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows.Forms;
using databaseHIA;
using HIA_client_lourd.Class;

namespace HIA_client_lourd.Forms
{
    public partial class SuppVisiteurPreListeForm : Form
    {
        private readonly Patient _patientRecherche;
        private static readonly string DatabaseConnectionString = ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString;
        private List<string> _infoVisiteur;

        public SuppVisiteurPreListeForm(Patient currentPatient)
        {
            _patientRecherche = currentPatient;

            this.InitializeComponent();
        }
        private void suppVisiteurPreListeForm_Load(object sender, EventArgs e)
        {
            Height = 107;
        }

        private void btnChercherVisiteurSupp_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtBoxNomVisiteurSupp.Text) || !String.IsNullOrEmpty(txtBoxEmailVisiteurSupp.Text))
            {
                _infoVisiteur = new List<string>();

                HeavyClientDatabaseObject hdb = new HeavyClientDatabaseObject(DatabaseConnectionString);

                _infoVisiteur = hdb.SearchVisiteurinPreList(txtBoxNomVisiteurSupp.Text, txtBoxEmailVisiteurSupp.Text, Convert.ToInt32(_patientRecherche.IdPatient));

                if (_infoVisiteur.Count > 0)
                {
                    label2.Text = _infoVisiteur[1];
                    label4.Text = _infoVisiteur[2];
                    label6.Text = _infoVisiteur[3];
                    label8.Text = String.IsNullOrEmpty(_infoVisiteur[4]) ? @"Non renseigné." : _infoVisiteur[4];

                    foreach (Control control in panelInfoVisiteurSupp.Controls)
                    {
                        control.Visible = control.Name != lblMsgConfirmationSupp.Name;

                    }

                    panelInfoVisiteurSupp.Visible = true;

                    Height = 260;
                }
                else
                {
                    MessageBox.Show(@"Aucun visiteur n'a été trouvé.");
                }
            }
            else
            {
                MessageBox.Show(@"Veuillez entrer le nom et l'adresse e-mail du visiteur que vous souhaitez supprimer.");
            }
        }

        private void btnSuppVisiteur_Click(object sender, EventArgs e)
        {
            btnSuppVisiteur = sender as Button;

            HeavyClientDatabaseObject hdb = new HeavyClientDatabaseObject(DatabaseConnectionString);

            if (hdb.DeleteFromPreList(_infoVisiteur[1], _infoVisiteur[3], Convert.ToInt32(_infoVisiteur[0])))
            {
                lblMsgConfirmationSupp.Text = @"Le visiteur a correctement été supprimer.";
                lblMsgConfirmationSupp.Visible = true;
            }
            else
            {
                MessageBox.Show(@"Il y a eu un problème lors de la suppression du visiteur, veuillez réessayer ultérieurement.");
            }
        }
    }
}
