using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Configuration;
using databaseHIA;
using Utilities;
using QRcodeGenerator;
using emailSender;

namespace HIA_client_leger
{
    public partial class demandeDeVisite : System.Web.UI.Page
    {
        protected string inputValueDebutVisite { get; set; }
        protected string inputValueFinVisite { get; set; }
        private string databaseConnectionString = WebConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString;
        private TimeSpan horaireDebutVisite = TimeSpan.FromHours(8);
        private TimeSpan horaireFinVisite = TimeSpan.FromHours(22);

        public enum errorType
        {
            VisiteurAuthError = 1,
            VisiteurEmailNotValid = 2,
            VisiteurNotInPreList = 3,

            PatientNotFound = 4,

            textBoxAuthEmpty = 5
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!this.IsPostBack)
            {
                initPageControl();
            }
            if (Session["panelList"] != null)
            {
                foreach (Control control in (List<Panel>)Session["panelList"])
                {
                    if (control is Panel)
                    {
                        this.divEtapeHoraire.Controls.Add(control);
                    }
                }
                if (Session["panelListIndispo"] != null)
                {
                    foreach (Control control in (List<Panel>)Session["panelListIndispo"])
                    {
                        if (control is Panel)
                        {
                            this.divEtapeHoraireIndisponible.Controls.Add(control);
                        }
                    }
                }
            }

        }

        private void initPageControl()
        {
            //Ajout de la class "active" à la li de la master page            
            HtmlGenericControl liItem = (HtmlGenericControl)Master.FindControl("demandeDeVisite");
            liItem.Attributes.Add("class", "active");

            #region init textBox etape1 placeholder
            this.txtBoxNomVisiteur.Attributes.Add("placeholder", "Votre nom");
            this.txtBoxPrenVisiteur.Attributes.Add("placeholder", "Votre prénom");
            this.txtBoxEmailVisiteur.Attributes.Add("placeholder", "Votre adresse email");
            #endregion

            #region init textBox etape2 placeholder
            this.txtBoxNomPatient.Attributes.Add("placeholder", "Nom du patient");
            this.txtBoxPrenPatient.Attributes.Add("placeholder", "Prénom du patient");
            this.txtBoxCodePatient.Attributes.Add("placeholder", "Exemple : A12B45");
            #endregion

            #region init textBox etape1 Auth.
            this.txtBoxNomVisiteurAuth.Attributes.Add("placeholder", "Votre nom");
            this.txtBoxPrenVisiteurAuth.Attributes.Add("placeholder", "Votre prénom");
            this.txtBoxEmailVisiteurAuth.Attributes.Add("placeholder", "Votre adresse email");
            this.txtBoxNomPatientAuth.Attributes.Add("placeholder", "Nom du patient");
            this.txtBoxPrenPatientAuth.Attributes.Add("placeholder", "Prénom du patient");
            this.txtBoxChambrePatientAuth.Attributes.Add("placeholder", "Numéro de chambre du patient");
            #endregion

        }

        protected void btnConfirmerInfoPatient_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(this.txtBoxNomPatient.Text) && !String.IsNullOrWhiteSpace(this.txtBoxPrenPatient.Text) &&
            !String.IsNullOrWhiteSpace(this.txtBoxCodePatient.Text))
            {
                UtilitiesTool.timeManipulation timeTool = new UtilitiesTool.timeManipulation();
                lightClientDatabaseObject lDB = new lightClientDatabaseObject(this.databaseConnectionString);

                bool bPatientMatch = lDB.recherchePatient(this.txtBoxNomPatient.Text, this.txtBoxPrenPatient.Text, this.txtBoxCodePatient.Text, 1);
                if (bPatientMatch)
                {
                    if (lDB.isVisiteurInPreList((int)Session["idVisiteur"], this.txtBoxNomPatient.Text))
                    {
                        Session["idPatient"] = lDB.getPatientId(this.txtBoxNomPatient.Text, this.txtBoxPrenPatient.Text, String.Empty, 2);
                        int statusPatient = lDB.getStatusPatient((int)Session["idPatient"]);
                        if (statusPatient != 0)
                        {
                            this.panelEtape2.Visible = false;
                            //string sClass = divBarEtape1.Attributes["class"].Replace("activestep", "");
                            //this.divBarEtape2.Attributes["class"] = sClass;

                            //this.divBarEtape3.Attributes["class"] += " activestep";

                            TimeSpan[,] plageHoraire = lDB.getSchedule(this.txtBoxNomPatient.Text, this.txtBoxPrenPatient.Text);

                            displayLabel(timeTool.getDispo(plageHoraire, this.horaireDebutVisite, this.horaireFinVisite), plageHoraire);
                            this.panelEtape3.Visible = true;
                        }
                        else
                        {
                            this.panelEtape2.Visible = false;
                            this.panelPatientStatusBloquer.Visible = true;
                        }
                        
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "notification", "notificationError(" + (int)errorType.VisiteurNotInPreList + ");", true);
                    }

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "notification", "notificationError(" + (int)errorType.PatientNotFound + ");", true);
                }
            }
        }

        protected void btnConfirmerInfoVisiteur_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(this.txtBoxEmailVisiteur.Text))
            {
                UtilitiesTool.stringUtilities stringTool = new UtilitiesTool.stringUtilities();

                if (stringTool.isValidEmail(this.txtBoxEmailVisiteur.Text))
                {
                    lightClientDatabaseObject lDB = new lightClientDatabaseObject(this.databaseConnectionString);

                    bool bVisiteurMatch = lDB.rechercheVisiteur(this.txtBoxNomVisiteur.Text, this.txtBoxPrenVisiteur.Text, this.txtBoxEmailVisiteur.Text);
                    if (bVisiteurMatch)
                    {
                        Session["idVisiteur"] = lDB.getVisiteurId(this.txtBoxNomVisiteur.Text, this.txtBoxEmailVisiteur.Text);

                        this.panelEtape1.Visible = false;

                        //string sClass = divBarEtape2.Attributes["class"].Replace("activestep", "");
                        //this.divBarEtape1.Attributes["class"] = sClass;

                        //this.divBarEtape2.Attributes["class"] += " activestep";

                        this.panelEtape2.Visible = true;

                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "notification", "notificationError(" + (int)errorType.VisiteurAuthError + ");", true);
                    }
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "notification", "notificationError(" + (int)errorType.textBoxAuthEmpty + ");", true);
            }
        }

        bool checkTextBoxAuthValues()
        {
            foreach (Control control in divEtape2DemandeAutorisation.Controls)
            {
                if (control is TextBox)
                {
                    TextBox tb = control as TextBox;
                    if (!String.IsNullOrWhiteSpace(tb.Text))
                    {
                        return true;
                    }
                }
            }
            return false;

        }

        protected void btnConfirmerInfoVisiteurAuth_Click(object sender, EventArgs e)
        {
            if (this.checkTextBoxAuthValues())
            {
                List<String> txtBoxValues = new List<string>();

                foreach (Control control in this.divEtape2DemandeAutorisation.Controls)
                {
                    if (control is TextBox)
                    {
                        TextBox tb = control as TextBox;
                        if (!String.IsNullOrWhiteSpace(tb.Text))
                        {
                            txtBoxValues.Add(tb.Text);
                        }
                    }
                }

                lightClientDatabaseObject lDB = new lightClientDatabaseObject(this.databaseConnectionString);

                bool patientMatch = lDB.recherchePatient(txtBoxValues[3], txtBoxValues[4], txtBoxValues[5], 2);

                if (patientMatch)
                {
                    UtilitiesTool.stringUtilities stringTool = new UtilitiesTool.stringUtilities();

                    if (stringTool.isValidEmail(txtBoxValues[2]))
                    {
                        emailSenderObject emailSender = new emailSenderObject();

                        int patientID = lDB.getPatientId(this.txtBoxNomPatientAuth.Text, this.txtBoxPrenPatientAuth.Text, this.txtBoxChambrePatientAuth.Text, 1);
                        if (lDB.addToPrelist(patientID, txtBoxValues))
                        {
                            string patientNumViste = lDB.getPatientNumVisite(patientID);
                            if (emailSender.sendNotification(this.txtBoxEmailVisiteurAuth.Text, emailSenderObject.NOTIFICATION.PrelisteAccepted, this.txtBoxNomPatientAuth.Text))
                            {
                                this.panelEtape1.Visible = false;
                                this.panelEtapeNotificationEnvoiAutorisation.Visible = true;
                            }

                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "notification", "notificationError(" + (int)errorType.VisiteurEmailNotValid + ");", true);
                    }

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "notification", "notificationError(" + (int)errorType.PatientNotFound + ");", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "notification", "notificationError(" + (int)errorType.textBoxAuthEmpty + ");", true);
            }
            
        }

        protected void btnConfirmerPlageHoraire_Click(object sender, EventArgs e)
        {
            int indexPanel = 0;
            foreach (Control control in this.divEtapeHoraire.Controls)
            {
                if (control is Panel)
                {
                    foreach (Control c in control.Controls)
                    {
                        if (c is RadioButton)
                        {
                            RadioButton rb = c as RadioButton;
                            if (rb.Checked)
                            {
                                foreach (Control cont in rb.Parent.Controls)
                                {
                                    if (cont is Label && cont.ID == "labelPlageHoraire" + indexPanel)
                                    {
                                        Label label = cont as Label;

                                        this.myModalSubTitle.Text = label.Text;
                                        this.myModalSubTitle.Style.Value = "margin-right:15px;";

                                        this.labelHeureDebVisite.Style.Value = "margin-bottom:10px;";
                                        this.labelHeureFinVisite.Style.Value = "margin-bottom:10px;";

                                        this.inputValueDebutVisite = label.Text.Split(' ').First();
                                        this.inputValueFinVisite = label.Text.Split(' ').Last();

                                    }
                                }
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "openTimeMin", "openTimeMin();", true);
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "openTimeMax", "openTimeMax();", true);
                            }
                        }
                    }
                    indexPanel++;
                }
            }
        }

        protected void btnConfirmerHeureModal_Click(object sender, EventArgs e)
        {
            string heureDebutVisite = Request.Form["heureDebutVisite"];
            string heureFinVisite = Request.Form["heureFinVisite"];

            TimeSpan plageDebutVisite = TimeSpan.Parse(myModalSubTitle.Text.Split(' ').First());
            TimeSpan plageFinVisite = TimeSpan.Parse(myModalSubTitle.Text.Split(' ').Last());

            UtilitiesTool.timeManipulation timeTool = new UtilitiesTool.timeManipulation();
            UtilitiesTool.stringUtilities stringTool = new UtilitiesTool.stringUtilities();
            string guidBonVisite = stringTool.generateGUID();

            QRGenerator generator = new QRGenerator();

            emailSenderObject emailSender = new emailSenderObject();

            lightClientDatabaseObject lDB = new lightClientDatabaseObject(this.databaseConnectionString);

            if (timeTool.compareTimeSpan(TimeSpan.Parse(heureDebutVisite), plageDebutVisite) && timeTool.compareTimeSpan(plageFinVisite, TimeSpan.Parse(heureFinVisite)))
            {
                if (lDB.getStatusPatient((int)Session["idPatient"]) == 1)
                {
                    if (lDB.sendDemandeDeVisite(TimeSpan.Parse(heureDebutVisite), TimeSpan.Parse(heureFinVisite), (int)Session["idPatient"], (int)Session["idVisiteur"], guidBonVisite, 1))
                    {
                        generator.generateQRCode(guidBonVisite, DateTime.Today.ToString(), heureDebutVisite, heureFinVisite, this.txtBoxNomPatient.Text, this.txtBoxPrenPatient.Text, "2", "150");

                        //emailSender.sendNotification("t.maalem@aforp.eu", emailSenderObject.NOTIFICATION.Accepted, generator.QRCodeCompletePath);

                        this.panelEtape3.Visible = false;
                        this.panelInfoDemandeDeVisiteFinal.Visible = true;
                    }
                }
                else if (lDB.getStatusPatient((int)Session["idPatient"]) == 3)
                {
                    if (lDB.sendDemandeDeVisite(TimeSpan.Parse(heureDebutVisite), TimeSpan.Parse(heureFinVisite), (int)Session["idVisiteur"], (int)Session["idPatient"], guidBonVisite, 3))
                    {
                        this.panelEtape3.Visible = false;
                        this.panelInfoDemandeDeVisiteBesoinConfirmation.Visible = true;
                    }
                }
            }
            else
            {
                //this.divErreurHoraire.Visible = true;
            }
        }

        private void displayLabel(TimeSpan[,] horaire, TimeSpan[,] horaireIndispo)
        {
            /*if (Session["panelList"] != null)
            {
                Session["panelList"] = null;
                if (this.divEtapeHoraireIndisponible.HasControls())
                {
                    foreach (Control control in this.divEtapeHoraireIndisponible.Controls)
                    {
                        if (control is Panel)
                        {
                            control.Controls.Clear();
                        }
                        control.Dispose();
                    }
                }

            }*/

            List<Panel> panelList = new List<Panel>();
            List<Panel> panelListIndispo = new List<Panel>();

            this.labelInfoAffluence.Style.Value = "margin-left:5px;";
            this.labelInfoChoixHoraire.Style.Value = "margin-left:25px;";

            UtilitiesTool.stringUtilities stringTool = new UtilitiesTool.stringUtilities();

            if (Session["panelList"] == null)
            {
                for (int i = 0; i < horaire.GetLength(0); i++)
                {
                    Panel myPanel = new Panel();

                    myPanel.ID = "panelPlageHoraire" + i;
                    myPanel.CssClass = "form-group";
                    myPanel.Style.Value = "margin-left:50px";

                    Label myLabel1 = new Label();
                    myLabel1.ID = "labelPlageHoraire" + i;

                    myLabel1.Text = stringTool.splitString(horaire[i, 0].ToString()) + " - " + stringTool.splitString(horaire[i, 1].ToString());
                    myLabel1.CssClass = "col-md-3 control-label";

                    Label myLabel2 = new Label();

                    string etat = String.Empty;

                    myLabel2.ID = "labelAffluence" + i;

                    lightClientDatabaseObject lDB = new lightClientDatabaseObject(this.databaseConnectionString);

                    int affluence = lDB.getAffluence(horaire[i, 0], horaire[i, 1], 1, (int)Session["idPatient"]);
                    if (affluence >= 0 && affluence < 2)
                    {
                        etat = "Faible";
                        myLabel2.Style.Value = "color:green";
                    }
                    else if (affluence >= 2 && affluence < 3)
                    {
                        etat = "Moyenne";
                        myLabel2.Style.Value = "color:orange";
                    }
                    else if (affluence >= 3)
                    {
                        etat = "Forte";
                        myLabel2.Style.Value = "color:red";
                    }

                    myLabel2.CssClass = "col-md-3 control-label";
                    myLabel2.Text = etat;

                    RadioButton myRadiobutton = new RadioButton();

                    myRadiobutton.ID = "radioBtnHoraire" + i.ToString();
                    myRadiobutton.CssClass = "col-md-3 control-label";

                    myRadiobutton.GroupName = "rBtnGroup";

                    myPanel.Controls.Add(myLabel1);
                    myPanel.Controls.Add(myLabel2);
                    myPanel.Controls.Add(myRadiobutton);

                    panelList.Add(myPanel);
                    this.divEtapeHoraire.Controls.Add(myPanel);

                    Session["panelList"] = panelList;

                }

                if (horaireIndispo[0, 0] != TimeSpan.Zero)
                {
                    this.plageHoraireIndispoTitre.Visible = true;

                    for (int i = 0; i < horaireIndispo.GetLength(0); i++)
                    {
                        Panel myPanel = new Panel();
                        myPanel.ID = "panelPlageHoraireIndispo" + i;
                        myPanel.CssClass = "form-group";
                        myPanel.Style.Value = "margin-left:50px";

                        Label myLabel1 = new Label();
                        myLabel1.ID = "labelPlageHoraireIndispo" + i;
                        myLabel1.Text = stringTool.splitString(horaireIndispo[i, 0].ToString()) + " - " + stringTool.splitString(horaireIndispo[i, 1].ToString());
                        myLabel1.CssClass = "col-md-3 control-label";

                        Label myLabel2 = new Label();
                        myLabel2.ID = "labelAffluenceIndispo" + i;
                        myLabel2.Text = "Indisponible";
                        myLabel2.CssClass = "col-md-3 control-label";
                        myLabel2.Style.Value = "margin-left:20px;";

                        RadioButton myRadioButton = new RadioButton();
                        myRadioButton.ID = "radioBtnHoraireIndispo" + i.ToString();
                        myRadioButton.CssClass = "col-md-3 control-label radioButtonIndispo";
                        myRadioButton.GroupName = "rBtnGroup";
                        myRadioButton.Enabled = false;

                        myPanel.Controls.Add(myLabel1);
                        myPanel.Controls.Add(myLabel2);
                        myPanel.Controls.Add(myRadioButton);

                        panelListIndispo.Add(myPanel);
                        this.divEtapeHoraireIndisponible.Controls.Add(myPanel);

                        Session["panelListIndispo"] = panelListIndispo;

                    }
                }
            }
        }
    }
}