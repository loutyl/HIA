﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Configuration;
using emailSender;
using databaseHIA;
using Utilities;

namespace HIA_client_lourd
{
    public partial class MainForm : Form
    {
        private Patient _patientRecherche;
        private static string _databaseConnectionString = ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString;
        private UtilitiesTool.stringUtilities _stringTool = new UtilitiesTool.stringUtilities();
        private emailSenderObject _emailSender = new emailSenderObject();

        public MainForm()
        {
            InitializeComponent();

        }
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btnPreListeAfficher_Click(object sender, EventArgs e)
        {
            btnPreListeAfficher = sender as Button;

            pre_Liste preListe = new pre_Liste(_patientRecherche);

            preListe.Show();
        }

        private void btnEnvoiCodeVisite_Click(object sender, EventArgs e)
        {
            btnEnvoiCodeVisite = sender as Button;

            string addressEmailVisiteur = "t.maalem@aforp.eu";

            if (this._emailSender.sendVisitCode(addressEmailVisiteur, lblNbCodeVisite.Text))
            {
                MessageBox.Show("L'email a bien été envoyé.");
            }
            else
            {
                MessageBox.Show("L'email n'a pu être envoyé correctement.");
            }
        }

        private void btnGenerationCodeVisite_Click(object sender, EventArgs e)
        {
            btnGenerationCodeVisite = sender as Button;

            lblNbCodeVisite.Text = this._stringTool.generateGUID();

        }

        private void btnPreListeAjouter_Click(object sender, EventArgs e)
        {
            btnPreListeAjouter = sender as Button;

            if (String.IsNullOrEmpty(txtBoxPreListeNomVisiteur.Text) || String.IsNullOrEmpty(txtBoxPreListePrenVisiteur.Text))
            {
                MessageBox.Show("Veuillez saisir le nom et prénom du visiteur à ajouter dans la liste.", "Erreur",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (!String.IsNullOrEmpty(txtBoxPreListeEmailVisiteur.Text))
            {
                if (this._stringTool.isValidEmail(txtBoxPreListeEmailVisiteur.Text))
                {
                    if (this._emailSender.sendNotification("t.maalem@aforp.eu", emailSenderObject.NOTIFICATION.PrelisteAccepted, this._patientRecherche._NomPatient))
                    {
                        List<string> infoPL = new List<string>();

                        infoPL.Add(txtBoxPreListeNomVisiteur.Text);
                        infoPL.Add(txtBoxPreListePrenVisiteur.Text);
                        infoPL.Add(txtBoxPreListeTelVisiteur.Text);
                        infoPL.Add(txtBoxPreListeEmailVisiteur.Text);
                        infoPL.Add(this._patientRecherche._IdPatient);

                        heavyClientDatabaseObject hdb = new heavyClientDatabaseObject(MainForm._databaseConnectionString);

                        if (hdb.ajoutPrelist(infoPL))
                        {
                            MessageBox.Show("Le visiteur a bien été enregistré dans la pré-liste.");
                        }
                        else
                        {
                            MessageBox.Show("Un problème est survenu le visiteur n'a pas pu être enregistré");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Veuillez renseigner une adresse email correcte.", "Erreur",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Erreur");
            }
        }

        private void displayInfoPatient(int status)
        {
            lblNomRecherchePatient.Text = this._patientRecherche._NomPatient;
            lblPrenRecherchePatient.Text = this._patientRecherche._PrenomPatient;
            label3.Text = this._patientRecherche.AgePatient;
            label4.Text = this._patientRecherche._EtagePatient;
            label5.Text = this._patientRecherche._ChambrePatient;
            label6.Text = this._patientRecherche._LitPatient;

            if (status == 2)
            {

                lblStatusVisite.Text = "Toutes les visites de se patient sont actuellement bloquées.";
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

            List<string> infoPatientRecherche = new List<string>();

            if (!String.IsNullOrEmpty(txtBoxRecherchePatient.Text))
            {
                try
                {
                    heavyClientDatabaseObject hdb = new heavyClientDatabaseObject(MainForm._databaseConnectionString);

                    this._patientRecherche = new Patient(hdb.recherchePatient(txtBoxRecherchePatient.Text));

                    displayInfoPatient(this._patientRecherche._StatusVisite);
                }
                catch (Exception)
                {
                    MessageBox.Show("Patient inconnu");
                }
            }
            else
            {
                MessageBox.Show("Veuillez entrer le nom ou prénom d'un patient", "Erreur",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnVoirDemandeVisite_Click(object sender, EventArgs e)
        {
            BtnVoirDemandeVisite = sender as Button;

            if (!String.IsNullOrEmpty(txtBoxRecherchePatient.Text))
            {
                DemandeVisitePatient demandeVisiteWindow = new DemandeVisitePatient(_patientRecherche);

                demandeVisiteWindow.Show();
            }
        }

        private void btnHistoriqueVisite_Click(object sender, EventArgs e)
        {
            btnHistoriqueVisite = sender as Button;

            if (!String.IsNullOrEmpty(txtBoxRecherchePatient.Text))
            {
                HistoriqueVisite HistoriqueWindow = new HistoriqueVisite(_patientRecherche);

                HistoriqueWindow.Show();
            }

        }
        private void btnDbloquerVisite_Click(object sender, EventArgs e)
        {
            Button DbloquerVisite = sender as Button;

            heavyClientDatabaseObject hdb = new heavyClientDatabaseObject(MainForm._databaseConnectionString);

            if (hdb.debloquerVisite(this._patientRecherche._NomPatient))
            {
                if (this._emailSender.sendNotification("t.maalem@aforp.eu", emailSenderObject.NOTIFICATION.Unblocked, this._patientRecherche._NomPatient))
                {
                    MessageBox.Show("Les visites du patient ont bien été débloquées.");
                }
                
            }
            else
            {
                MessageBox.Show("Un problème est survenue, les visites n'ont pas pu être débloquées.");
            }
        }
    }
}
