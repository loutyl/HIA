using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows.Forms;
using databaseHIA;
using emailSender;
using HIA_client_lourd.Class;
using Utilities;

namespace HIA_client_lourd.Forms
{
    public partial class MainForm : Form
    {
        private Patient _patientRecherche;
        private static readonly string DatabaseConnectionString = ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString;
        private readonly UtilitiesTool.StringUtilities _stringTool = new UtilitiesTool.StringUtilities();
        private readonly EmailSenderObject _emailSender = new EmailSenderObject();

        public MainForm()
        {
            this.InitializeComponent();

        }
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btnPreListeAfficher_Click(object sender, EventArgs e)
        {
            btnPreListeAfficher = sender as Button;

            PreListe preListe = new PreListe(_patientRecherche);

            preListe.Show();
        }

        private void btnPreListeAjouter_Click(object sender, EventArgs e)
        {
            btnPreListeAjouter = sender as Button;

            if (String.IsNullOrEmpty(txtBoxPreListeNomVisiteur.Text) || String.IsNullOrEmpty(txtBoxPreListePrenVisiteur.Text))
            {
                MessageBox.Show(@"Veuillez saisir le nom et prénom du visiteur à ajouter dans la liste.", @"Erreur",

                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (!String.IsNullOrEmpty(txtBoxPreListeEmailVisiteur.Text))
            {
                if (_stringTool.IsValidEmail(txtBoxPreListeEmailVisiteur.Text))
                {
                    if (_emailSender.SendNotification("t.maalem@aforp.eu", EmailSenderObject.Notification.PrelisteAccepted, _patientRecherche.NomPatient))
                    {
                        List<string> infoPl = new List<string>
                        {
                            txtBoxPreListeNomVisiteur.Text,
                            txtBoxPreListePrenVisiteur.Text,
                            txtBoxPreListeTelVisiteur.Text,
                            txtBoxPreListeEmailVisiteur.Text,
                            _patientRecherche.IdPatient
                        };


                        HeavyClientDatabaseObject hdb = new HeavyClientDatabaseObject(DatabaseConnectionString);

                        MessageBox.Show(hdb.AjoutPrelist(infoPl)
                            ? @"Le visiteur a bien été enregistré dans la pré-liste."
                            : @"Un problème est survenu le visiteur n'a pas pu être enregistré.");
                    }
                }
                else
                {
                    MessageBox.Show(@"Veuillez renseigner une adresse email correcte.", @"Erreur",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show(@"Veuillez renseigner l'adresse e-mail du visiteur à ajouter.");
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

        private void btnRecherchePatient_Click(object sender, EventArgs e)
        {
            btnRecherchePatient = sender as Button;

            if (!String.IsNullOrEmpty(txtBoxRecherchePatient.Text))
            {
                try
                {
                    HeavyClientDatabaseObject hdb = new HeavyClientDatabaseObject(DatabaseConnectionString);

                    _patientRecherche = new Patient(hdb.RecherchePatient(txtBoxRecherchePatient.Text));

                    this.DisplayInfoPatient(_patientRecherche.StatusVisite);
                }
                catch (Exception)
                {
                    MessageBox.Show(@"Patient inconnu");
                }
            }
            else
            {
                MessageBox.Show(@"Veuillez entrer le nom ou prénom d'un patient", @"Erreur",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnVoirDemandeVisite_Click(object sender, EventArgs e)
        {
            BtnVoirDemandeVisite = sender as Button;

            if (!String.IsNullOrEmpty(txtBoxRecherchePatient.Text))
            {
                var listVisite = _patientRecherche.GetDemandeDeVisite();

                if (listVisite.Count == 0)
                {
                    MessageBox.Show(@"Ce patient n'a aucune demande de visite en attente de décision.");
                }
                else
                {
                    DemandeVisitePatient demandeVisiteWindow = new DemandeVisitePatient(_patientRecherche, listVisite);

                    demandeVisiteWindow.Show();
                }
            }
        }

        private void btnHistoriqueVisite_Click(object sender, EventArgs e)
        {
            btnHistoriqueVisite = sender as Button;

            if (!String.IsNullOrEmpty(txtBoxRecherchePatient.Text))
            {
                HistoriqueVisite historiqueWindow = new HistoriqueVisite(_patientRecherche);

                historiqueWindow.Show();
            }

        }
        private void btnDbloquerVisite_Click(object sender, EventArgs e)
        {
            HeavyClientDatabaseObject hdb = new HeavyClientDatabaseObject(DatabaseConnectionString);

            if (hdb.DebloquerVisite(_patientRecherche.NomPatient))
            {
                if (_emailSender.SendNotification("t.maalem@aforp.eu", EmailSenderObject.Notification.Unblocked, _patientRecherche.NomPatient))
                {
                    MessageBox.Show(@"Les visites du patient ont bien été débloquées.");
                }

            }
            else
            {
                MessageBox.Show(@"Un problème est survenue, les visites n'ont pas pu être débloquées.");
            }
        }

        private void btnSupprimerPreListe_Click(object sender, EventArgs e)
        {
            btnSupprimerPreListe = sender as Button;

            SuppVisiteurPreListeForm suppVisiteurWindow = new SuppVisiteurPreListeForm(_patientRecherche);
            suppVisiteurWindow.Show();

        }
    }
}
