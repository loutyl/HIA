namespace HIA_client_lourd.Forms
{
    public partial class SuppVisiteurPreListeForm : System.Windows.Forms.Form
    {
        private readonly Class.Patient _patientRecherche;
        private static readonly string DatabaseConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString;
        private System.Collections.Generic.List<string> _infoVisiteur;

        public SuppVisiteurPreListeForm(Class.Patient currentPatient)
        {
            _patientRecherche = currentPatient;

            this.InitializeComponent();
        }
        private void suppVisiteurPreListeForm_Load(object sender, System.EventArgs e)
        {
            Height = 107;
        }

        private void btnChercherVisiteurSupp_Click(object sender, System.EventArgs e)
        {
            if (!System.String.IsNullOrEmpty(txtBoxNomVisiteurSupp.Text) || !System.String.IsNullOrEmpty(txtBoxEmailVisiteurSupp.Text))
            {
                _infoVisiteur = new System.Collections.Generic.List<string>();

                databaseHIA.HeavyClientDatabaseObject hdb = new databaseHIA.HeavyClientDatabaseObject(DatabaseConnectionString);

                _infoVisiteur = hdb.SearchVisiteurinPreList(txtBoxNomVisiteurSupp.Text, txtBoxEmailVisiteurSupp.Text, System.Convert.ToInt32(_patientRecherche.IdPatient));

                if (_infoVisiteur.Count > 0)
                {
                    label2.Text = _infoVisiteur[1];
                    label4.Text = _infoVisiteur[2];
                    label6.Text = _infoVisiteur[3];
                    label8.Text = System.String.IsNullOrEmpty(_infoVisiteur[4]) ? @"Non renseigné." : _infoVisiteur[4];

                    foreach (System.Windows.Forms.Control control in panelInfoVisiteurSupp.Controls)
                    {
                        control.Visible = control.Name != lblMsgConfirmationSupp.Name;

                    }

                    panelInfoVisiteurSupp.Visible = true;

                    Height = 260;
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show(@"Aucun visiteur n'a été trouvé.");
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(@"Veuillez entrer le nom et l'adresse e-mail du visiteur que vous souhaitez supprimer.");
            }
        }

        private void btnSuppVisiteur_Click(object sender, System.EventArgs e)
        {
            btnSuppVisiteur = sender as System.Windows.Forms.Button;

            databaseHIA.HeavyClientDatabaseObject hdb = new databaseHIA.HeavyClientDatabaseObject(DatabaseConnectionString);

            if (hdb.DeleteFromPreList(_infoVisiteur[1], _infoVisiteur[3], System.Convert.ToInt32(_infoVisiteur[0])))
            {
                lblMsgConfirmationSupp.Text = @"Le visiteur a correctement été supprimer.";
                lblMsgConfirmationSupp.Visible = true;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(@"Il y a eu un problème lors de la suppression du visiteur, veuillez réessayer ultérieurement.");
            }
        }
    }
}
