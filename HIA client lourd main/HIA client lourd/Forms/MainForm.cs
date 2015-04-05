namespace HIA_client_lourd.Forms
{
    public partial class MainForm : System.Windows.Forms.Form
    {
        private Class.Patient _patientRecherche;
        private static readonly string DatabaseConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString;
        private readonly Utilities.UtilitiesTool.StringUtilities _stringTool = new Utilities.UtilitiesTool.StringUtilities();
        private readonly emailSender.EmailSenderObject _emailSender = new emailSender.EmailSenderObject();

        public MainForm()
        {
            this.InitializeComponent();

        }
        private void MainForm_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void btnPreListeAfficher_Click(object sender, System.EventArgs e)
        {
            btnPreListeAfficher = sender as System.Windows.Forms.Button;

            PreListe preListe = new PreListe(_patientRecherche);

            preListe.Show();
        }

        private void btnPreListeAjouter_Click(object sender, System.EventArgs e)
        {
            btnPreListeAjouter = sender as System.Windows.Forms.Button;

            if (System.String.IsNullOrEmpty(txtBoxPreListeNomVisiteur.Text) || System.String.IsNullOrEmpty(txtBoxPreListePrenVisiteur.Text))
            {
                System.Windows.Forms.MessageBox.Show(@"Veuillez saisir le nom et prénom du visiteur à ajouter dans la liste.", @"Erreur",

                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            if (!System.String.IsNullOrEmpty(txtBoxPreListeEmailVisiteur.Text))
            {
                if (_stringTool.IsValidEmail(txtBoxPreListeEmailVisiteur.Text))
                {
                    if (_emailSender.SendNotification("t.maalem@aforp.eu", emailSender.EmailSenderObject.Notification.PrelisteAccepted, _patientRecherche.NomPatient))
                    {
                        System.Collections.Generic.List<string> infoPl = new System.Collections.Generic.List<string>
                        {
                            txtBoxPreListeNomVisiteur.Text,
                            txtBoxPreListePrenVisiteur.Text,
                            txtBoxPreListeTelVisiteur.Text,
                            txtBoxPreListeEmailVisiteur.Text,
                            _patientRecherche.IdPatient
                        };


                        databaseHIA.HeavyClientDatabaseObject hdb = new databaseHIA.HeavyClientDatabaseObject(DatabaseConnectionString);

                        System.Windows.Forms.MessageBox.Show(hdb.AjoutPrelist(infoPl)
                            ? @"Le visiteur a bien été enregistré dans la pré-liste."
                            : @"Un problème est survenu le visiteur n'a pas pu être enregistré.");
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show(@"Veuillez renseigner une adresse email correcte.", @"Erreur",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(@"Veuillez renseigner l'adresse e-mail du visiteur à ajouter.");
            }
        }

        private void DisplayInfoPatient(int status)
        {
            lblNomRecherchePatient.Text = _patientRecherche.NomPatient;
            lblPrenRecherchePatient.Text = _patientRecherche.PrenomPatient;
            label3.Text = _patientRecherche.AgePatient;
            label4.Text = _patientRecherche.EtagePatient;
            label5.Text = _patientRecherche.ChambrePatient;
            label6.Text = _patientRecherche.LitPatient;

            if (status == 2)
            {

                lblStatusVisite.Text = @"Toutes les visites de se patient sont actuellement bloquées.";
                lblStatusVisite.Visible = true;
            }

            lblNomRecherchePatient.Visible = true;
            lblPrenRecherchePatient.Visible = true;
            label3.Visible = true;
            label4.Visible = true;
            label5.Visible = true;
            label6.Visible = true;

        }

        private void btnRecherchePatient_Click(object sender, System.EventArgs e)
        {
            btnRecherchePatient = sender as System.Windows.Forms.Button;

            if (!System.String.IsNullOrEmpty(txtBoxRecherchePatient.Text))
            {
                try
                {
                    databaseHIA.HeavyClientDatabaseObject hdb = new databaseHIA.HeavyClientDatabaseObject(DatabaseConnectionString);

                    _patientRecherche = new Class.Patient(hdb.RecherchePatient(txtBoxRecherchePatient.Text));

                    this.DisplayInfoPatient(_patientRecherche.StatusVisite);
                }
                catch (System.Exception)
                {
                    System.Windows.Forms.MessageBox.Show(@"Patient inconnu");
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(@"Veuillez entrer le nom ou prénom d'un patient", @"Erreur",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        private void BtnVoirDemandeVisite_Click(object sender, System.EventArgs e)
        {
            BtnVoirDemandeVisite = sender as System.Windows.Forms.Button;

            if (!System.String.IsNullOrEmpty(txtBoxRecherchePatient.Text))
            {
                var listVisite = _patientRecherche.GetDemandeDeVisite();

                if (listVisite.Count == 0)
                {
                    System.Windows.Forms.MessageBox.Show(@"Ce patient n'a aucune demande de visite en attente de décision.");
                }
                else
                {
                    DemandeVisitePatient demandeVisiteWindow = new DemandeVisitePatient(_patientRecherche, listVisite);

                    demandeVisiteWindow.Show();
                }
            }
        }

        private void btnHistoriqueVisite_Click(object sender, System.EventArgs e)
        {
            btnHistoriqueVisite = sender as System.Windows.Forms.Button;

            if (!System.String.IsNullOrEmpty(txtBoxRecherchePatient.Text))
            {
                HistoriqueVisite historiqueWindow = new HistoriqueVisite(_patientRecherche);

                historiqueWindow.Show();
            }

        }
        private void btnDbloquerVisite_Click(object sender, System.EventArgs e)
        {
            databaseHIA.HeavyClientDatabaseObject hdb = new databaseHIA.HeavyClientDatabaseObject(DatabaseConnectionString);

            if (hdb.DebloquerVisite(_patientRecherche.NomPatient))
            {
                if (_emailSender.SendNotification("t.maalem@aforp.eu", emailSender.EmailSenderObject.Notification.Unblocked, _patientRecherche.NomPatient))
                {
                    System.Windows.Forms.MessageBox.Show(@"Les visites du patient ont bien été débloquées.");
                }

            }
            else
            {
                System.Windows.Forms.MessageBox.Show(@"Un problème est survenue, les visites n'ont pas pu être débloquées.");
            }
        }

        private void btnSupprimerPreListe_Click(object sender, System.EventArgs e)
        {
            btnSupprimerPreListe = sender as System.Windows.Forms.Button;

            SuppVisiteurPreListeForm suppVisiteurWindow = new SuppVisiteurPreListeForm(_patientRecherche);
            suppVisiteurWindow.Show();

        }
    }
}
