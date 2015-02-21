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
using databaseHIA;
using emailSender;
using Utilities;


namespace HIA_client_lourd
{
    public partial class DemandeVisitePatient : Form
    {
        private List<demandeDeVisite> listVisite = new List<demandeDeVisite>();
        private Patient _currentPatient;
        private int _indexDemandeVisite = 0;
        private bonDeVisite _currentBonDeVisite;
        private static string _databaseConnectionString = ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString;
        private emailSenderObject _emailSender = new emailSenderObject();
        private UtilitiesTool.stringUtilities _stringTool = new UtilitiesTool.stringUtilities();

        private void getDemandeDeVisite()
        {
            int status = 3;
            List<List<string>> listDemandeDeVisite = new List<List<string>>();

            heavyClientDatabaseObject hdb = new heavyClientDatabaseObject(DemandeVisitePatient._databaseConnectionString);

            listDemandeDeVisite = hdb.getDemandeDeVisite(status, this._currentPatient._NomPatient);

            foreach (List<string> list in listDemandeDeVisite)
            {
                Visiteur visiteur = new Visiteur(list[0], list[1], list[2], list[3], list[4]);

                demandeDeVisite demande = new demandeDeVisite(visiteur, list[6], list[7], list[8]);

                this.listVisite.Add(demande);
            }

            if (listVisite.Count == 0)
            {
                MessageBox.Show("Ce patient n'a aucune demande de visite en attente de décision.");
            }
            else
            {
                displayInfoDemandeVisite(this._indexDemandeVisite);
            }
        }

        public DemandeVisitePatient(Patient patient)
        {
            InitializeComponent();

            this._currentPatient = patient;

            this.getDemandeDeVisite();
        }       

        private void GenerateQrcode(string data, string path, string nomCode)
        {
            QRCode qrcode = new QRCode();

            qrcode.Data = data;
            qrcode.DataMode = QRCodeDataMode.Byte;
            qrcode.UOM = UnitOfMeasure.PIXEL;
            qrcode.X = 3;
            qrcode.LeftMargin = 0;
            qrcode.RightMargin = 0;
            qrcode.TopMargin = 0;
            qrcode.BottomMargin = 0;
            qrcode.Resolution = 72;
            qrcode.Rotate = Rotate.Rotate0;
            qrcode.ImageFormat = ImageFormat.Jpeg;

            try
            {
                qrcode.drawBarcode(path + nomCode);

                MessageBox.Show("QRcode OK!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void createQrcode()
        {
            string user = Environment.UserName;
            string path = "C:\\Users\\" + user + "\\Desktop\\test\\";
            string nomCode = "QRcode_ visite.jpeg";
            string data = "Votre demande a été acceptée, le numéro d'identification de se bon de visite est : " + _currentBonDeVisite.Num_bon_visite + "\r\n" +
                 "\r\nVoici les informations de la visite ainsi que celle du patient : " +
                 "\r\nDate de la visite : " + _currentBonDeVisite.DateBonVisite +
                 "\r\nHeure de début de la visite : " + _currentBonDeVisite.HeureBonDVisite +
                 "\r\nHeure de fin de la visite : " + _currentBonDeVisite.HeureBonFVisite + "\r\n" +
                 "\r\nInformation concernant le patient : " +
                 "\r\nNom : " + _currentPatient._NomPatient +
                 "\r\nPrénom : " + _currentPatient._PrenomPatient +
                 "\r\nEtage : " + _currentPatient._EtagePatient +
                 "\r\nChambre : " + _currentPatient._ChambrePatient +
                 "\r\nLit : " + _currentPatient._LitPatient;

            if (Directory.Exists(path))
            {
                GenerateQrcode(data, path, nomCode);
            }
            else if (Directory.Exists(path) == false)
            {
                try
                {
                    Directory.CreateDirectory(path);
                    GenerateQrcode(data, path, nomCode);

                }
                catch (Exception)
                {
                    string defaultPath = Environment.CurrentDirectory;
                    GenerateQrcode(data, defaultPath, nomCode);
                }
            }
        }

        private void createBonVisite(string numBonVisite)
        {
            this._currentBonDeVisite = new bonDeVisite(this.listVisite[this._indexDemandeVisite].DateVisite, this.listVisite[this._indexDemandeVisite].HeureDVisite,
              this.listVisite[this._indexDemandeVisite].HeureFVisite, numBonVisite);;
        }

        private void btnAccepterDemande_Click(object sender, EventArgs e)
        {
            int newStatus = 1;

            createBonVisite(this._stringTool.generateGUID());            

            heavyClientDatabaseObject hdb = new heavyClientDatabaseObject(DemandeVisitePatient._databaseConnectionString);

            hdb.updateData(newStatus, this._currentBonDeVisite.Num_bon_visite, this.listVisite[this._indexDemandeVisite].DateVisite, this.listVisite[this._indexDemandeVisite].HeureDVisite, this.listVisite[this._indexDemandeVisite].HeureFVisite, true);

            createQrcode();
            
            if (this._emailSender.sendNotification("t.maalem@aforp.eu", emailSenderObject.NOTIFICATION.Accepted))
            {
                MessageBox.Show("La demande de visite à été acceptée");
            }      
        }

        private void btnRefuserDemande_Click(object sender, EventArgs e)
        {
            int newStatus = 2;

            heavyClientDatabaseObject hdb = new heavyClientDatabaseObject(DemandeVisitePatient._databaseConnectionString);

            hdb.updateData(newStatus, String.Empty, this.listVisite[this._indexDemandeVisite].DateVisite, this.listVisite[this._indexDemandeVisite].HeureDVisite, this.listVisite[this._indexDemandeVisite].HeureFVisite, false);           

            if (this._emailSender.sendNotification("t.maalem@aforp.eu", emailSenderObject.NOTIFICATION.Refused))
            {
                MessageBox.Show("La demande de visite à été refusé");                
            }
                        
        }    

        private void displayInfoDemandeVisite(int index)
        {
            label1.Text = this.listVisite[index].VisiteurOrigine._NomVisiteur;
            label2.Text = this.listVisite[index].VisiteurOrigine._PrenVisiteur;
            label3.Text = this.listVisite[index].DateVisite;
            label4.Text = this.listVisite[index].HeureDVisite;
            label5.Text = this.listVisite[index].HeureFVisite;

            heavyClientDatabaseObject hdb = new heavyClientDatabaseObject(DemandeVisitePatient._databaseConnectionString);

            int affluence = hdb.getAffluence(TimeSpan.Parse(this.listVisite[this._indexDemandeVisite].HeureDVisite), TimeSpan.Parse(this.listVisite[this._indexDemandeVisite].HeureFVisite), 3);

            if (affluence >= 0 && affluence < 2)
            {
                lblDemandeDeVisiteNivAfflu.ForeColor = Color.Green;
                lblDemandeDeVisiteNivAfflu.Text = "Faible";
            }
            else if (affluence >= 2 && affluence < 3)
            {
                lblDemandeDeVisiteNivAfflu.ForeColor = Color.DarkOrange;
                lblDemandeDeVisiteNivAfflu.Text = "Moyenne";
            }
            else if (affluence >= 3)
            {
                lblDemandeDeVisiteNivAfflu.ForeColor = Color.Red;
                lblDemandeDeVisiteNivAfflu.Text = "Forte";
            }

            lblNbVisite.Text = (index + 1).ToString() + "/" + this.listVisite.Count.ToString();

            label1.Visible = true;
            label2.Visible = true;
            label3.Visible = true;
            label4.Visible = true;
            label5.Visible = true;
            lblNbVisite.Visible = true;

        }

        private void btnDemandeVisiteSuivant_Click(object sender, EventArgs e)
        {
            this._indexDemandeVisite++;

            if (this._indexDemandeVisite < this.listVisite.Count)
            {
                displayInfoDemandeVisite(this._indexDemandeVisite);                                
            }
        }       

        private void btnBloquerVisite_Click(object sender, EventArgs e)
        {
            heavyClientDatabaseObject hdb = new heavyClientDatabaseObject(DemandeVisitePatient._databaseConnectionString);

            try
            {
                hdb.bloquerVisite(this._currentPatient._NomPatient, 3);

                List<string> listEmailVisiteurDoublon = new List<string>();
                List<string> listEmailVisiteurSansDoublon = new List<string>();

                int i = 0;

                while (i < this.listVisite.Count)
                {

                    hdb.updateData(2, this._currentBonDeVisite.Num_bon_visite, this.listVisite[i].DateVisite, this.listVisite[i].HeureDVisite, this.listVisite[i].HeureFVisite, false);

                    listEmailVisiteurDoublon.Add(this.listVisite[i].VisiteurOrigine._EmailVisiteur);

                    i++;
                }

                listEmailVisiteurSansDoublon = this._stringTool.removeDoublonFromList(listEmailVisiteurDoublon);

                if (this._emailSender.sendNotification("t.maalem@aforp.eu", emailSenderObject.NOTIFICATION.Blocked, this._currentPatient._NomPatient))
                {
                    MessageBox.Show("Toutes les visites du patient ont été bloquées");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }
    }
}
