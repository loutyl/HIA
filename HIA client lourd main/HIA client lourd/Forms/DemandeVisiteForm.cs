﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;
using databaseHIA;
using emailSender;
using HIA_client_lourd.Class;
using QRcodeGenerator;
using Utilities;

namespace HIA_client_lourd.Forms
{
    public partial class DemandeVisitePatient : Form
    {
        private readonly List<DemandeDeVisite> _listVisite;
        private readonly Patient _currentPatient;
        private int _indexDemandeVisite;
        private BonDeVisite _currentBonDeVisite;
        private static readonly string DatabaseConnectionString = ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString;
        private readonly EmailSenderObject _emailSender = new EmailSenderObject();
        private readonly UtilitiesTool.StringUtilities _stringTool = new UtilitiesTool.StringUtilities();

        public DemandeVisitePatient(Patient patient, List<DemandeDeVisite> listDemandeVisite)
        {
            this.InitializeComponent();

            _currentPatient = patient;
            _listVisite = listDemandeVisite;

            this.DisplayInfoDemandeVisite(_indexDemandeVisite);
        }

        private void CreateBonVisite(string numBonVisite)
        {
            _currentBonDeVisite = new BonDeVisite(_listVisite[_indexDemandeVisite].DateVisite, _listVisite[_indexDemandeVisite].HeureDVisite,
              _listVisite[_indexDemandeVisite].HeureFVisite, numBonVisite);
        }

        private void btnAccepterDemande_Click(object sender, EventArgs e)
        {
            const int newStatus = 1;

            this.CreateBonVisite(_stringTool.GenerateGuid());

            HeavyClientDatabaseObject hdb = new HeavyClientDatabaseObject(DatabaseConnectionString);

            hdb.UpdateData(newStatus, _listVisite[_indexDemandeVisite].DateVisite, _listVisite[_indexDemandeVisite].HeureDVisite, _listVisite[_indexDemandeVisite].HeureFVisite, true, _currentBonDeVisite.NumBonVisite);

            QrGenerator generator = new QrGenerator();
            generator.GenerateQrCode(_currentBonDeVisite.NumBonVisite, _currentBonDeVisite.DateBonVisite, _currentBonDeVisite.HeureBonDVisite, _currentBonDeVisite.HeureBonFVisite,
                                     _currentPatient.NomPatient, _currentPatient.PrenomPatient,
                                     _currentPatient.EtagePatient, _currentPatient.ChambrePatient);


            if (_emailSender.SendNotification("t.maalem@aforp.eu", EmailSenderObject.Notification.Accepted, generator.QrCodeCompletePath))
            {
                MessageBox.Show(@"La demande de visite à été acceptée");
            }
        }

        private void btnRefuserDemande_Click(object sender, EventArgs e)
        {
            const int newStatus = 2;

            HeavyClientDatabaseObject hdb = new HeavyClientDatabaseObject(DatabaseConnectionString);

            hdb.UpdateData(newStatus, _listVisite[_indexDemandeVisite].DateVisite, _listVisite[_indexDemandeVisite].HeureDVisite, _listVisite[_indexDemandeVisite].HeureFVisite, false, String.Empty);

            if (_emailSender.SendNotification("t.maalem@aforp.eu", EmailSenderObject.Notification.Refused))
            {
                MessageBox.Show(@"La demande de visite à été refusé");
            }

        }

        private void DisplayInfoDemandeVisite(int index)
        {
            label1.Text = _listVisite[index].VisiteurOrigine.NomVisiteur;
            label2.Text = _listVisite[index].VisiteurOrigine.PrenVisiteur;
            label3.Text = _listVisite[index].DateVisite;
            label4.Text = _listVisite[index].HeureDVisite;
            label5.Text = _listVisite[index].HeureFVisite;

            HeavyClientDatabaseObject hdb = new HeavyClientDatabaseObject(DatabaseConnectionString);

            int affluence = hdb.GetAffluence(TimeSpan.Parse(_listVisite[_indexDemandeVisite].HeureDVisite), TimeSpan.Parse(_listVisite[_indexDemandeVisite].HeureFVisite), 3, Convert.ToInt32(_currentPatient.IdPatient));

            if (affluence >= 0 && affluence < 2)
            {
                lblDemandeDeVisiteNivAfflu.ForeColor = Color.Green;
                lblDemandeDeVisiteNivAfflu.Text = @"Faible";
            }
            else if (affluence >= 2 && affluence < 3)
            {
                lblDemandeDeVisiteNivAfflu.ForeColor = Color.DarkOrange;
                lblDemandeDeVisiteNivAfflu.Text = @"Moyenne";
            }
            else if (affluence >= 3)
            {
                lblDemandeDeVisiteNivAfflu.ForeColor = Color.Red;
                lblDemandeDeVisiteNivAfflu.Text = @"Forte";
            }

            lblNbVisite.Text = (index + 1) + @"/" + _listVisite.Count;

            label1.Visible = true;
            label2.Visible = true;
            label3.Visible = true;
            label4.Visible = true;
            label5.Visible = true;
            lblNbVisite.Visible = true;

        }

        private void btnDemandeVisiteSuivant_Click(object sender, EventArgs e)
        {
            _indexDemandeVisite++;

            if (_indexDemandeVisite < _listVisite.Count)
            {
                this.DisplayInfoDemandeVisite(_indexDemandeVisite);
            }
        }

        private void btnBloquerVisite_Click(object sender, EventArgs e)
        {
            HeavyClientDatabaseObject hdb = new HeavyClientDatabaseObject(DatabaseConnectionString);

            try
            {
                hdb.BloquerVisite(_currentPatient.NomPatient, 3);

                List<string> listEmailVisiteurDoublon = new List<string>();
                List<string> listEmailVisiteurSansDoublon = new List<string>();

                int i = 0;

                while (i < _listVisite.Count)
                {

                    hdb.UpdateData(2, _listVisite[i].DateVisite, _listVisite[i].HeureDVisite, _listVisite[i].HeureFVisite, false);

                    listEmailVisiteurDoublon.Add(_listVisite[i].VisiteurOrigine.EmailVisiteur);

                    i++;
                }

                listEmailVisiteurSansDoublon = _stringTool.RemoveDoublonFromList(listEmailVisiteurDoublon);

                if (_emailSender.SendNotification("t.maalem@aforp.eu", EmailSenderObject.Notification.Blocked, _currentPatient.NomPatient))
                {
                    MessageBox.Show(@"Toutes les visites du patient ont été bloquées");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
