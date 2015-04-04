using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using databaseHIA;

namespace HIA_client_lourd.Forms
{
    public partial class suppVisiteurPreListeForm : Form
    {
        private Patient _patientRecherche;
        private static string _databaseConnectionString = ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString;
        private List<string> _infoVisiteur;

        public suppVisiteurPreListeForm(Patient currentPatient)
        {
            this._patientRecherche = currentPatient;

            InitializeComponent();
        }
        private void suppVisiteurPreListeForm_Load(object sender, EventArgs e)
        {
            this.Height = 107;
        }

        private void btnChercherVisiteurSupp_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(this.txtBoxNomVisiteurSupp.Text) || !String.IsNullOrEmpty(this.txtBoxEmailVisiteurSupp.Text))
            {
                this._infoVisiteur = new List<string>();

                heavyClientDatabaseObject hdb = new heavyClientDatabaseObject(suppVisiteurPreListeForm._databaseConnectionString);

                _infoVisiteur = hdb.searchVisiteurinPreList(this.txtBoxNomVisiteurSupp.Text, this.txtBoxEmailVisiteurSupp.Text, Convert.ToInt32(this._patientRecherche._IdPatient));

                if (_infoVisiteur.Count > 0)
                {
                    this.label2.Text = this._infoVisiteur[1];
                    this.label4.Text = this._infoVisiteur[2];
                    this.label6.Text = this._infoVisiteur[3];
                    if (String.IsNullOrEmpty(this._infoVisiteur[4]))
                    {
                        this.label8.Text = "Non renseigné.";
                    }
                    else
                    {
                        this.label8.Text = this._infoVisiteur[4];
                    }

                    foreach (Control control in this.panelInfoVisiteurSupp.Controls)
                    {
                        if (control.Name == this.lblMsgConfirmationSupp.Name)
                        {
                            control.Visible = false;
                        }
                        else
                        {
                            control.Visible = true;
                        }

                    }

                    panelInfoVisiteurSupp.Visible = true;

                    this.Height = 260;
                }
                else
                {
                    MessageBox.Show("Aucun visiteur n'a été trouvé.");
                }
            }
            else
            {
                MessageBox.Show("Veuillez entrer le nom et l'adresse e-mail du visiteur que vous souhaitez supprimer.");
            }
        }

        private void btnSuppVisiteur_Click(object sender, EventArgs e)
        {
            btnSuppVisiteur = sender as Button;

            heavyClientDatabaseObject hdb = new heavyClientDatabaseObject(suppVisiteurPreListeForm._databaseConnectionString);

            if (hdb.deleteFromPreList(this._infoVisiteur[1], this._infoVisiteur[3], Convert.ToInt32(this._infoVisiteur[0])))
            {
                this.lblMsgConfirmationSupp.Text = "Le visiteur a correctement été supprimer.";
                this.lblMsgConfirmationSupp.Visible = true;
            }
            else
            {
                MessageBox.Show("Il y a eu un problème lors de la suppression du visiteur, veuillez réessayer ultérieurement.");
            }
        }
    }
}
