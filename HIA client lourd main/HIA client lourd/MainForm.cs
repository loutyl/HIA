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
using System.Net.Mail;
using System.Configuration;
using System.Text.RegularExpressions;

namespace HIA_client_lourd
{
    public partial class MainForm : Form
    {
        //Variables globales patient recherché
        private static Patient patientRecherche;
        //La recherche (le nom ou prénom du patient)
        private string recherche;
       
        public MainForm()
        {
            //Initialisation des composants du Form
            InitializeComponent();
 
        }
        //Evenement lors d'un click sur le boutton affichage de la pré-liste
        private void btnPreListeAfficher_Click(object sender, EventArgs e)
        {
            btnPreListeAfficher = sender as Button;
            
            //Instanciation du Form preListe
            pre_Liste preListe = new pre_Liste(patientRecherche);
            //Affichage du form preListe
            preListe.Show();
        }
        //Fonction d'envoi de mail
        public bool sendEmail(string address, string subject, string body)
        {
            //Variable d'identification du compte 
            string utilisateur, password, serveur;
            utilisateur = ConfigurationManager.AppSettings["SmtpUtilisateur"];
            password = ConfigurationManager.AppSettings["SmtpPassword"];
            serveur = ConfigurationManager.AppSettings["SmtpServeur"];
            //Instanciation d'un client smtp
            SmtpClient client = new SmtpClient();
            //Affection du port 
            client.Port = 587;
            //Le serveur SMTP
            client.Host = serveur;
            //Les différents paramêtre du client
            client.EnableSsl = true;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            //Les identifiants
            client.Credentials = new System.Net.NetworkCredential(utilisateur, password);
            //Instanciation d'un nouvel email 
            MailMessage email = new MailMessage("newcomp92@hotmail.fr", address);
            //Création de l'email avec subjet et body
            email.Subject = subject;
            email.Body = body;
            //booleen de retour
            bool bRet = false;
            //essaie de l'envoi du mail
            try
            {
                //Envoi du mail
                client.Send(email);
                //Notification de succes de l'envoi du mail
                MessageBox.Show("L'email a bien été envoyé.");
                bRet = true;

            }
            catch (Exception ex)
            {
                //Affichage du message d'erreur
                MessageBox.Show(ex.Message);

            }
            finally
            {
                //Destruction des objets email et client
                email.Dispose();
                client.Dispose();
            }

            return bRet;

        }
        //Evenement lors d'un click sur le boutton d'envoi du code de visite
        private void btnEnvoiCodeVisite_Click(object sender, EventArgs e)
        {
            btnEnvoiCodeVisite = sender as Button;

            string emailSubject = "Code de visite";
            string emailBody = "Voici votre code de visite pour remplir votre formulaire : " + lblNbCodeVisite.Text;
            string addressEmailVisiteur = "t.maalem@aforp.eu";

            sendEmail(addressEmailVisiteur, emailSubject, emailBody);
        }
        //Evenement lors d'un click sur le boutton de génération d'un code de visite
        private void btnGenerationCodeVisite_Click(object sender, EventArgs e)
        {
            btnGenerationCodeVisite = sender as Button;
            
            //Instanciation d'un nouveau GUID
            Guid guid = Guid.NewGuid();
            //Affichage du code de visite
            lblNbCodeVisite.Text = guid.ToString().Split('-').First();

        }      

        //Fonction de test de la validité du format de l'adresse email 
        public bool isValidEmail(string email)
        {
            //Les symbole servant de test de validité de l'email (selon regex)
            string symbole = "^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$";
            //Si l'email match le regex
            if(Regex.IsMatch(email,symbole,RegexOptions.IgnoreCase))
            { 
                //Déclaration des variables pour le titre et le corps de l'email
                string subjectEmail = "Autorisation de visite";
                string bodyEmail = "Vous avez été ajouté à la pré-liste de M." + lblNomRecherchePatient.Text + " " + lblPrenRecherchePatient.Text + ", vous êtes désormais autorisé à effectuer des"+
                    "\r\ndemandes de visite via notre site: ";
                //Fonction d'envoi de l'email
                if(sendEmail(email, subjectEmail, bodyEmail))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            //Si ne match pas
            else
            { 
                return false;
            }
        }
        
        //Evenement lors d'un click sur le boutton d'ajout à la preliste
        private void btnPreListeAjouter_Click(object sender, EventArgs e)
        {
            btnPreListeAjouter = sender as Button;
            
            string sNomPL = txtBoxPreListeNomVisiteur.Text;
            string sPrenomPL = txtBoxPreListePrenVisiteur.Text;
            string sTelPL = txtBoxPreListeTelVisiteur.Text;
            string sEmailPL = txtBoxPreListeEmailVisiteur.Text;
            string sIDPatientPL = patientRecherche.IdPatient;
            
            //Si les textBox sont vide
            if (String.IsNullOrEmpty(sNomPL) || String.IsNullOrEmpty(sPrenomPL))
            {
                //Affichage d'un message d'erreur
                MessageBox.Show("Veuillez saisir le nom et prénom du visiteur à ajouter dans la liste.", "Erreur", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);        
            }
            //Si la textBox renseignant l'email du visiteur n'est pas vide
            if (!String.IsNullOrEmpty(sEmailPL))
            {
                //Si le test de l'email renseigné dans la textBox est valide 
                if (isValidEmail(sEmailPL) == true)
                {
                    List<string> infoPL = new List<string>();                    

                    infoPL.Add(sNomPL);
                    infoPL.Add(sPrenomPL);
                    infoPL.Add(sTelPL);
                    infoPL.Add(sEmailPL);
                    infoPL.Add(sIDPatientPL);

                    databaseHIA db = new databaseHIA();
                    //Si la méthode ajoutPrelist de l'objet db retourne true 
                    if(db.ajoutPrelist(infoPL))
                    {
                        //Notification d'ajout à la préliste
                        MessageBox.Show("Le visiteur a bien été enregistré dans la pré-liste.");
                    }
                    else
                    {   
                        //Notification d'un problème à l'utilisateur
                        MessageBox.Show("Un problème est survenu le visiteur n'a pas pu être enregistré");
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
        //Fonction d'affichage des information du patient
        private void displayInfoPatient(bool status)
        {
            //Changement du text des labels par les informations du patient
            lblNomRecherchePatient.Text = patientRecherche.NomPatient;
            lblPrenRecherchePatient.Text = patientRecherche.PrenomPatient;
            label3.Text = patientRecherche.AgePatient;
            label4.Text = patientRecherche.EtagePatient;
            label5.Text = patientRecherche.ChambrePatient;
            label6.Text = patientRecherche.LitPatient;
            //Si le status de visite du patient est False 
            if(!status)
            {
                //Information pour le chef de service pour connaitre le status du patient
                lblStatusVisite.Text = "Toutes les visites de se patient sont actuellement bloquées.";
                //Affichage du label
                lblStatusVisite.Visible = true;
            }
             
            //Affichage des labels
            lblNomRecherchePatient.Visible = true;
            lblPrenRecherchePatient.Visible = true;
            label3.Visible = true;
            label4.Visible = true;
            label5.Visible = true;
            label6.Visible = true;
            
        }
        //Evenement lors d'un click sur le boutton rechercher
        private void btnRecherchePatient_Click(object sender, EventArgs e)
        {
            btnRecherchePatient = sender as Button;

            //Si la textBox est pas vide
            if (!String.IsNullOrEmpty(txtBoxRecherchePatient.Text))
            {
                //Récupération du texte de la textBox
                recherche = txtBoxRecherchePatient.Text;
                try
                {
                    //Instanciation d'une nouvelle connection à la base de donnée
                    databaseHIA db = new databaseHIA();
                    //Appel de la méthode recherchePatient de l'objet db
                    //Affectation du résultat à la variable patientRecherche
                    patientRecherche = db.recherchePatient(recherche);

                    //Affichage des informations du patient
                    displayInfoPatient(patientRecherche.StatusVisite);
                }
                catch (Exception)
                {
                    //Affichage d'un message d'erreur 
                    MessageBox.Show("Patient inconnu");
                }               
            }
            else
            {
                MessageBox.Show("Veuillez entrer le nom ou prénom d'un patient", "Erreur",
                    MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }
        }
        //Evenement lors d'un click sur le boutton voirDemandeVisite
        private void BtnVoirDemandeVisite_Click(object sender, EventArgs e)
        {
            BtnVoirDemandeVisite = sender as Button;
            
            //Si la textBox recherchePatient n'est pas nul
            if(!String.IsNullOrEmpty(txtBoxRecherchePatient.Text))
            {
                //Instanciation du Form demandeVisiteWindow
                DemandeVisitePatient demandeVisiteWindow = new DemandeVisitePatient(patientRecherche);
                //Affichage du Form
                demandeVisiteWindow.Show();
            }
        }
        //Evenement lors d'un click sur le boutton HistoriqueVisite
        private void btnHistoriqueVisite_Click(object sender, EventArgs e)
        {
            btnHistoriqueVisite = sender as Button; 
            //Si la textBox recherchePatient n'est pas nul
            if (!String.IsNullOrEmpty(txtBoxRecherchePatient.Text))
            {
                //Instanciation du Form HistoriqueWindow
                HistoriqueVisite HistoriqueWindow = new HistoriqueVisite(patientRecherche);
                //Affichage du Form
                HistoriqueWindow.Show();   
            }
            
        }
        //Evenement lorsque le Form est fermé
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Exit de l'application
            Application.Exit();
        }

        private void btnDbloquerVisite_Click(object sender, EventArgs e)
        {
            Button DbloquerVisite = sender as Button;
            databaseHIA db = new databaseHIA();

            if(db.debloquerVisite(patientRecherche.NomPatient))
            {
                //Affichage d'un message de notification
                MessageBox.Show("Les visites du patient ont bien été débloquées.");  
            }
            else
            {
                MessageBox.Show("Un problème est survenue, les visites n'ont pas pu être débloquées.");
            }

            
        }
    }
}
