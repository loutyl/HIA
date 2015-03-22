using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using OnBarcode.Barcode;
using System.Configuration;
using databaseHIA;
using emailSender;
using Utilities;
using QRcodeGenerator;


namespace HIA_client_lourd
{
    public partial class DemandeVisitePatient : Form
    {
        private List<demandeDeVisite> _listVisite = new List<demandeDeVisite>();
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

                this._listVisite.Add(demande);
            }

            if (this._listVisite.Count == 0)
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

        private void createBonVisite(string numBonVisite)
        {
            this._currentBonDeVisite = new bonDeVisite(this._listVisite[this._indexDemandeVisite].DateVisite, this._listVisite[this._indexDemandeVisite].HeureDVisite,
              this._listVisite[this._indexDemandeVisite].HeureFVisite, numBonVisite);;
        }

        private void btnAccepterDemande_Click(object sender, EventArgs e)
        {
            int newStatus = 1;

            createBonVisite(this._stringTool.generateGUID());            

            heavyClientDatabaseObject hdb = new heavyClientDatabaseObject(DemandeVisitePatient._databaseConnectionString);

            hdb.updateData(newStatus, this._currentBonDeVisite.Num_bon_visite, this._listVisite[this._indexDemandeVisite].DateVisite, this._listVisite[this._indexDemandeVisite].HeureDVisite, this._listVisite[this._indexDemandeVisite].HeureFVisite, true);            

            QRGenerator generator = new QRGenerator();
            generator.generateQRCode(this._currentBonDeVisite.Num_bon_visite, this._currentBonDeVisite.DateBonVisite,                                                                             this._currentBonDeVisite.HeureBonDVisite, this._currentBonDeVisite.HeureBonFVisite, 
                                     this._currentPatient._NomPatient, this._currentPatient._PrenomPatient, 
                                     this._currentPatient._EtagePatient, this._currentPatient._ChambrePatient);


            if (this._emailSender.sendNotification("t.maalem@aforp.eu", emailSenderObject.NOTIFICATION.Accepted, generator.QRCodeCompletePath))
            {
                MessageBox.Show("La demande de visite à été acceptée");
            }      
        }

        private void btnRefuserDemande_Click(object sender, EventArgs e)
        {
            int newStatus = 2;

            heavyClientDatabaseObject hdb = new heavyClientDatabaseObject(DemandeVisitePatient._databaseConnectionString);

            hdb.updateData(newStatus, String.Empty, this._listVisite[this._indexDemandeVisite].DateVisite, this._listVisite[this._indexDemandeVisite].HeureDVisite, this._listVisite[this._indexDemandeVisite].HeureFVisite, false);           

            if (this._emailSender.sendNotification("t.maalem@aforp.eu", emailSenderObject.NOTIFICATION.Refused))
            {
                MessageBox.Show("La demande de visite à été refusé");                
            }
                        
        }    

        private void displayInfoDemandeVisite(int index)
        {
            label1.Text = this._listVisite[index].VisiteurOrigine._NomVisiteur;
            label2.Text = this._listVisite[index].VisiteurOrigine._PrenVisiteur;
            label3.Text = this._listVisite[index].DateVisite;
            label4.Text = this._listVisite[index].HeureDVisite;
            label5.Text = this._listVisite[index].HeureFVisite;

            heavyClientDatabaseObject hdb = new heavyClientDatabaseObject(DemandeVisitePatient._databaseConnectionString);

            int affluence = hdb.getAffluence(TimeSpan.Parse(this._listVisite[this._indexDemandeVisite].HeureDVisite), TimeSpan.Parse(this._listVisite[this._indexDemandeVisite].HeureFVisite), 3, Convert.ToInt32(this._currentPatient._IdPatient));

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

            lblNbVisite.Text = (index + 1).ToString() + "/" + this._listVisite.Count.ToString();

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

            if (this._indexDemandeVisite < this._listVisite.Count)
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

                while (i < this._listVisite.Count)
                {

                    hdb.updateData(2, this._currentBonDeVisite.Num_bon_visite, this._listVisite[i].DateVisite, this._listVisite[i].HeureDVisite, this._listVisite[i].HeureFVisite, false);

                    listEmailVisiteurDoublon.Add(this._listVisite[i].VisiteurOrigine._EmailVisiteur);

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
