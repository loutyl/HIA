using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using OnBarcode.Barcode;
using System.Net.Mail;
using System.Configuration;


namespace HIA_client_lourd
{
    public partial class DemandeVisitePatient : Form
    {
        //liste des demandes de visites du patient
        List<demandeDeVisite> listVisite = new List<demandeDeVisite>();
        //Le patient concerné
        private Patient currentPatient;
        //Index correspondant à la demande de visite dans la liste
        private int indexDemandeVisite = 0;
        //Le bon de visite crée
        private bonDeVisite currentBonDeVisite;

        //Constructeur de la form DemandeVisitePatient
        public DemandeVisitePatient(Patient _patient)
        {
            //Initialisation des composants du Form
            InitializeComponent();
            //Affectation de la variable passée en paramêtre a la variable globale currentPatient
            currentPatient = _patient;
            //Récupération des demandes de visites
            GetData();
        }
        
        //Génération d'un QRcode grâce à la fonction  GenerateQrcode
        private void GenerateQrcode(string data, string path, string nomCode)
        {
            
            //Instanciation d'un nouveau QRcode
            QRCode qrcode = new QRCode();
            //Affectation des données au QRcode
            qrcode.Data = data;
            //Changement des différentes paramêtres du QRcode comme sa taille, sa résolution, etc...
            qrcode.DataMode = QRCodeDataMode.Byte;
            qrcode.UOM = UnitOfMeasure.PIXEL;
            qrcode.X = 3;
            qrcode.LeftMargin = 0;
            qrcode.RightMargin = 0;
            qrcode.TopMargin = 0;
            qrcode.BottomMargin = 0;
            qrcode.Resolution = 72;
            qrcode.Rotate = Rotate.Rotate0;
            //Format du QRcode
            qrcode.ImageFormat = ImageFormat.Jpeg;
            
            //essaie de création du QRcode avec le chemin path + le nom du QRcode
            try
            {
                qrcode.drawBarcode(path + nomCode);
                //Message OK si success
                MessageBox.Show("QRcode OK!");
            }
            catch (Exception ex)
            {   
                //Affiche le message d'erreur 
                MessageBox.Show(ex.Message);
            }
           
        }

        //Fonction de création du QRcode
        private void createQrcode()
        {
            //Création d'une variable user contenant le nom d'utilisateur du PC actuel
            string user = Environment.UserName;
            //Le chemin ou sera stocker le QRcode
            string path = "C:\\Users\\" + user + "\\Desktop\\test\\";
            //Le nom du QRcode
            string nomCode = "QRcode_ visite.jpeg";
            //Les données du QRcode :
            //Les données correspondent aux attributs du bon devisite ainsi que les informations du patient visité
            string data = "Votre demande a été acceptée, le numéro d'identification de se bon de visite est : " + currentBonDeVisite.Num_bon_visite + "\r\n" +
                 "\r\nVoici les informations de la visite ainsi que celle du patient : "+
                 "\r\nDate de la visite : " + currentBonDeVisite.DateBonVisite + 
                 "\r\nHeure de début de la visite : " + currentBonDeVisite.HeureBonDVisite +
                 "\r\nHeure de fin de la visite : " + currentBonDeVisite.HeureBonFVisite + "\r\n" +
                 "\r\nInformation concernant le patient : " +
                 "\r\nNom : " + currentPatient.NomPatient + 
                 "\r\nPrénom : " + currentPatient.PrenomPatient +
                 "\r\nEtage : " + currentPatient.EtagePatient +
                 "\r\nChambre : " + currentPatient.ChambrePatient +
                 "\r\nLit : " + currentPatient.LitPatient ;
            
            //Si le dossier exisite alors on génère le QRcode 
            if(Directory.Exists(path))
            {
                //Génération du QRcode
                GenerateQrcode(data, path, nomCode);
            }
            else if (Directory.Exists(path) == false)
            {
                //Si il n'exisite pas alors on essaie de créer le répertoire
                try
                {
                    //On créé le répertoire
                    Directory.CreateDirectory(path);
                    //Génération du QRcode
                    GenerateQrcode(data, path, nomCode);

                }
                catch (Exception)
                {
                    //Si erreur alors on essaye avec un chemin par défaut qui est le répertoire de la solution
                    string defaultPath = Environment.CurrentDirectory;
                    //Génération du QRcode
                    GenerateQrcode(data, defaultPath, nomCode);
                    
                }
            } 
        }

        //Fonction de création du numéro de visite
        private string createNumVisite()
        {
            //Instanciation d'un nouveau GUID
            Guid guid = Guid.NewGuid();

            //Retourne le GUID sous la forme d'une chaine de caractère "splité" après le 1er '-'
            return guid.ToString().Split('-').First();

        }     
  
        //Création d'un bon de visite
        private void createBonVisite(string _num_bon_visite)
        {
            //Instanciation du bon de visite avec les attributs de la demande de visite
            bonDeVisite aBonDeViste = new bonDeVisite(listVisite[indexDemandeVisite].DateVisite, listVisite[indexDemandeVisite].HeureDVisite,
               listVisite[indexDemandeVisite].HeureFVisite, _num_bon_visite);
            //Affection du bon de visite à la variable globale currentBonDeVisite
            currentBonDeVisite = aBonDeViste;
        }

        //Evenement lors d'un click sur le bouton AccepterDemande
        private void btnAccepterDemande_Click(object sender, EventArgs e)
        {
            //Variable Status correspondant au status de la demande après être acceptée
            int newStatus = 1;
            //Variable permettant de savoir si la visite a été acceptée ou non
            bool accepterRefuser = true;
            //création d'un numéro de bon de visite
            string _num_bon_visite = createNumVisite();

            //Création du bon de visite
            createBonVisite(_num_bon_visite);
            //Mise a jour du status de la demande de visite
            updateData(newStatus, accepterRefuser);
            //Création du QRcode
            createQrcode();
            //Création des variable pour l'envoi du mail de confirmation
            string toAddress = "t.maalem@aforp.eu";
            string emailSubject = "Confirmation de votre demande de visite";
            string emailBody = "Votre demande de visite à été acceptée, vous treverez ci-joint le QRcode à présenter à l'accueil de l'hôpital le jour de votre visite.";
            //Envoi du mail de confirmation
            sendEmail(toAddress, emailSubject, emailBody);

            //Message de confirmation 
            MessageBox.Show("La demande de visite à été acceptée");
            
        }

        private void btnRefuserDemande_Click(object sender, EventArgs e)
        {
            //Pareil que pour Accepter demande
            int newStatus = 2; 
            bool accepterRefuser = false;

            //Mise à jour du status de la demande de visite
            updateData(newStatus, accepterRefuser);

            //Variables correspondantes aux paramêtre de l'envoi du mail de notification de refus 
            string toAddress = "t.maalem@aforp.eu";
            string emailSubject = "Refus de votre demande de visite";
            string emailBody = "Votre demande de visite à été refusé";
            //Envoi du mail
            sendEmail(toAddress, emailSubject, emailBody);
            //Message de confirmation
            MessageBox.Show("La demande de visite à été refusé");
        }

        //Fonction d'envoi de mail
        public void sendEmail(string address, string subject, string body)
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

            //essaie de l'envoi du mail
            try
            {   
                //Envoi du mail
                client.Send(email);
                //Notification de succes de l'envoi du mail
                MessageBox.Show("L'email a bien été envoyé à l'adressse e-mail");

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
        
        }
        private void GetData()
        {
            //Stauts des demandes de visite en attente de décision
            int status = 3;
            string sNomPatient = currentPatient.NomPatient;

            databaseHIA db = new databaseHIA();

            listVisite = db.getDemandeDeVisite(status, sNomPatient);
            if (listVisite.Count == 0)
            {
                MessageBox.Show("Ce patient n'a aucune demande de visite en attente de décision.");
                
            }
            else
            {
                displayInfoDemandeVisite(indexDemandeVisite);
            }            
        }
        //Fonction permettant d'afficher les informations de la demande de visite
        private void displayInfoDemandeVisite(int index)
        {
            //Changement du text des labels par les informations de la demande de visite
            label1.Text = listVisite[index].VisiteurOrigine.NomVisiteur;
            label2.Text = listVisite[index].VisiteurOrigine.PrenVisiteur;
            label3.Text = listVisite[index].DateVisite;
            label4.Text = listVisite[index].HeureDVisite;
            label5.Text = listVisite[index].HeureFVisite;
            lblNbVisite.Text = (index + 1).ToString() + "/" + listVisite.Count.ToString();
            
            //Affichage des labels
            label1.Visible = true;
            label2.Visible = true;
            label3.Visible = true;
            label4.Visible = true;
            label5.Visible = true;
            lblNbVisite.Visible = true;            

        }
        //Evenement se produisant lors d'un click sur le bouton Suivant
        private void btnDemandeVisiteSuivant_Click(object sender, EventArgs e)
        {
            //Incrémentation de l'index
            indexDemandeVisite++;

            //Si l'index de la liste de demande de visite est inférieure au nombre de demande de visite dans la liste -1
            if(indexDemandeVisite < listVisite.Count )
            {
                //Affichage des informations de la demande de visite
                displayInfoDemandeVisite(indexDemandeVisite);                

                if (indexDemandeVisite == 2 || indexDemandeVisite == 4 || indexDemandeVisite == 6)
                {
                    lblDemandeDeVisiteNivAfflu.ForeColor = Color.Green;
                    lblDemandeDeVisiteNivAfflu.Text = "Faible";
                    
                }
                else
                {
                    lblDemandeDeVisiteNivAfflu.ForeColor = Color.DarkOrange;
                    lblDemandeDeVisiteNivAfflu.Text = "Moyenne";
                }
            } 
        }
        //Fonction de mise a jour du status de la demande de visite
        private void updateData(int newStatus, bool accepterRefuser)
        {
            //Variable status correspondant au stauts des visites en attente de prise de décision
            int status = 3;            
            //Ouverture d'une connection à la base de donnée
            SqlConnection connection = new SqlConnection(Properties.Settings.Default.dbConnectionString);
            //Création d'une requête 
            SqlCommand cmd = connection.CreateCommand();
            //Si accepter
           if(accepterRefuser == true)
           {    
               //Essaye d'update
               try
               {
                   //Ouverture de la connection
                   connection.Open();
                   //Création de la requête
                   cmd.CommandText = "UPDATE Visiter SET status_demande = @newStatus, num_bon_visite = @bonVisite "+
                       "WHERE status_demande = @status " +
                       "AND date_visite = Convert(DATE,@dateVisite, 103) " +
                       "AND heure_deb_visite = Convert(TIME(1), @heureDVisite) " +
                       "AND heure_fin_visite = Convert(TIME(1),@heureFVisite) ";
                   //Passage par paramêtre des filtres WHERE
                   cmd.Parameters.AddWithValue("@newStatus", newStatus);
                   cmd.Parameters.AddWithValue("@bonVisite", currentBonDeVisite.Num_bon_visite);
                   cmd.Parameters.AddWithValue("@status", status);
                   cmd.Parameters.AddWithValue("@dateVisite", listVisite[indexDemandeVisite].DateVisite);
                   cmd.Parameters.AddWithValue("@heureDVisite", listVisite[indexDemandeVisite].HeureDVisite);
                   cmd.Parameters.AddWithValue("@heureFVisite", listVisite[indexDemandeVisite].HeureFVisite);
                   //Execution de la requête
                   cmd.ExecuteNonQuery();
              
               }
               catch (Exception ex)
               {    
                   //Affichage du message d'erreur
                   MessageBox.Show(ex.Message);

               }
               finally
               {
                   //Desctruction des objets cmd et connection
                   cmd.Dispose();
                   connection.Close();                 
                  
               }               
               
           }
           //Si refuser on fait pareil que Accepter...
           else
           {   
               try
               {
                   connection.Open();

                   cmd.CommandText = "UPDATE Visiter SET status_demande = @newStatus " +
                       "WHERE status_demande = @status " +
                       "AND date_visite = Convert(DATE,@dateVisite, 103) " +
                       "AND heure_deb_visite = Convert(TIME(1), @heureDVisite) " +
                       "AND heure_fin_visite = Convert(TIME(1),@heureFVisite) ";

                   cmd.Parameters.AddWithValue("@newStatus", newStatus);
                   cmd.Parameters.AddWithValue("@status", status);
                   cmd.Parameters.AddWithValue("@dateVisite", listVisite[indexDemandeVisite].DateVisite);
                   cmd.Parameters.AddWithValue("@heureDVisite", listVisite[indexDemandeVisite].HeureDVisite);
                   cmd.Parameters.AddWithValue("@heureFVisite", listVisite[indexDemandeVisite].HeureFVisite);

                   cmd.ExecuteNonQuery();
               }
               catch (Exception ex)
               {
                   MessageBox.Show(ex.Message);

               }
               finally
               {
                   cmd.Dispose();
                   connection.Close();
               }
           }

        }
        //Evenement lors du click sur le bouton BloquerVisite
        private void btnBloquerVisite_Click(object sender, EventArgs e)
        {
            //Ouverture d'une connection à la base de donnée
            SqlConnection connection = new SqlConnection(Properties.Settings.Default.dbConnectionString);
            //Création de requêtes 
            SqlCommand cmd = connection.CreateCommand();

            try
            {
                //Ouverture de la connection
                connection.Open();
                //Création de la requête
                cmd.CommandText = "UPDATE Patient SET status_visite = '0' WHERE nom_patient = @nomPatient AND status_visite = '1'";

                //Ajout des filtres WHERE par paramêtre
                cmd.Parameters.AddWithValue("@nomPatient", currentPatient.NomPatient);
                //Exécution de la requête
                cmd.ExecuteNonQuery();
                //Variable de status des demandes
                int newStatus = 2;
                bool accepterRefuser = false;
                //Tant que l'index de la liste des demande de visite est inférieure au nombre de demande de visite

                //Création d'une liste d'email des visiteurs avec doublon et une sans doublon
                List<string> listEmailVisiteurDoublon = new List<string>();
                List<string> listEmailVisiteurSansDoublon = new List<string>();
                //Index 
                int i = 0;
                //Tant que i est inférieure au nombre d'occurence dans lise
                while(i  < listVisite.Count)
                {
                    //Appel de la fonction de mise a jour des données
                    updateData(newStatus, accepterRefuser);
                    //Ajout de l'email du visiteur à la liste
                    listEmailVisiteurDoublon.Add(listVisite[i].VisiteurOrigine.EmailVisiteur);
                    //Incrémentation de l'index
                    i++;
                }
                //On enlève les doublons dans la liste avec doublon grâce à la méthode .disctinct() qu'on affecte ensuite
                //à la liste sans doublon
                listEmailVisiteurSansDoublon = listEmailVisiteurDoublon.Distinct().ToList();

                //Création des variable pour l'envoi du mail de confirmation
                string toAddress = "t.maalem@aforp.eu";
                string emailSubject = "Information concernant le statut des visites de M." + currentPatient.NomPatient + " " + currentPatient.PrenomPatient;
                string emailBody = "Suite à ..., toutes les visites de M." + currentPatient.NomPatient + " " + currentPatient.PrenomPatient + "ont été bloqué.";
                //Envoi du mail de confirmation
                sendEmail(toAddress, emailSubject, emailBody);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                
            }
            finally
            {
                //Destruction des objets cmd et connection
                cmd.Dispose();
                connection.Close();
            }

            //Affichage d'un message de notification
            MessageBox.Show("Toutes les visites du patient ont été bloquées");  
        }
    }
}
