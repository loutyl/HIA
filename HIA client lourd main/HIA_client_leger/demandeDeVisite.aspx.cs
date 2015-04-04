using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using databaseHIA;
using emailSender;
using QRcodeGenerator;
using Utilities;

namespace HIA_client_leger
{
    public partial class DemandeDeVisite : Page
    {
        protected string InputValueDebutVisite { get; set; }
        protected string InputValueFinVisite { get; set; }
        private readonly string _databaseConnectionString = WebConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString;
        private readonly TimeSpan _horaireDebutVisite = TimeSpan.FromHours(8);
        private readonly TimeSpan _horaireFinVisite = TimeSpan.FromHours(22);

        public enum ErrorType
        {
            VisiteurAuthError = 1,
            VisiteurEmailNotValid = 2,
            VisiteurNotInPreList = 3,

            PatientNotFound = 4,

            TextBoxAuthEmpty = 5
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                InitPageControl();
            }
            if (Session["panelList"] != null)
            {
                foreach (Panel control in (List<Panel>)Session["panelList"])
                {
                    if (control != null)
                    {
                        divEtapeHoraire.Controls.Add(control);
                    }
                }
                if (Session["panelListIndispo"] != null)
                {
                    foreach (Panel control in (List<Panel>)Session["panelListIndispo"])
                    {
                        if (control != null)
                        {
                            divEtapeHoraireIndisponible.Controls.Add(control);
                        }
                    }
                }
            }

        }

        private void InitPageControl()
        {
            //Ajout de la class "active" à la li de la master page            
            if (Master != null){
                HtmlGenericControl liItem = (HtmlGenericControl)Master.FindControl("demandeDeVisite");
                liItem.Attributes.Add("class", "active");
            }

            #region init textBox etape1 placeholder
            txtBoxNomVisiteur.Attributes.Add("placeholder", "Votre nom");
            txtBoxPrenVisiteur.Attributes.Add("placeholder", "Votre prénom");
            txtBoxEmailVisiteur.Attributes.Add("placeholder", "Votre adresse email");
            #endregion

            #region init textBox etape2 placeholder
            txtBoxNomPatient.Attributes.Add("placeholder", "Nom du patient");
            txtBoxPrenPatient.Attributes.Add("placeholder", "Prénom du patient");
            txtBoxCodePatient.Attributes.Add("placeholder", "Exemple : A12B45");
            #endregion

            #region init textBox etape1 Auth.
            txtBoxNomVisiteurAuth.Attributes.Add("placeholder", "Votre nom");
            txtBoxPrenVisiteurAuth.Attributes.Add("placeholder", "Votre prénom");
            txtBoxEmailVisiteurAuth.Attributes.Add("placeholder", "Votre adresse email");
            txtBoxNomPatientAuth.Attributes.Add("placeholder", "Nom du patient");
            txtBoxPrenPatientAuth.Attributes.Add("placeholder", "Prénom du patient");
            txtBoxChambrePatientAuth.Attributes.Add("placeholder", "Numéro de chambre du patient");
            #endregion

        }

        protected void btnConfirmerInfoPatient_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txtBoxNomPatient.Text) && !String.IsNullOrWhiteSpace(txtBoxPrenPatient.Text) &&
            !String.IsNullOrWhiteSpace(txtBoxCodePatient.Text))
            {
                UtilitiesTool.timeManipulation timeTool = new UtilitiesTool.timeManipulation();
                lightClientDatabaseObject lDb = new lightClientDatabaseObject(_databaseConnectionString);

                bool bPatientMatch = lDb.recherchePatient(txtBoxNomPatient.Text, txtBoxPrenPatient.Text, txtBoxCodePatient.Text, 1);
                if (bPatientMatch)
                {
                    if (lDb.isVisiteurInPreList((int)Session["idVisiteur"], txtBoxNomPatient.Text))
                    {
                        Session["idPatient"] = lDb.getPatientId(txtBoxNomPatient.Text, txtBoxPrenPatient.Text, String.Empty, 2);
                        int statusPatient = lDb.getStatusPatient((int)Session["idPatient"]);
                        if (statusPatient != 0)
                        {
                            panelEtape2.Visible = false;
                            //string sClass = divBarEtape1.Attributes["class"].Replace("activestep", "");
                            //this.divBarEtape2.Attributes["class"] = sClass;

                            //this.divBarEtape3.Attributes["class"] += " activestep";

                            TimeSpan[,] plageHoraire = lDb.getSchedule(txtBoxNomPatient.Text, txtBoxPrenPatient.Text);

                            DisplayLabel(timeTool.getDispo(plageHoraire, _horaireDebutVisite, _horaireFinVisite), plageHoraire);
                            panelEtape3.Visible = true;
                        }
                        else
                        {
                            panelEtape2.Visible = false;
                            panelPatientStatusBloquer.Visible = true;
                        }
                        
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "notification", "notificationError(" + (int)ErrorType.VisiteurNotInPreList + ");", true);
                    }

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "notification", "notificationError(" + (int)ErrorType.PatientNotFound + ");", true);
                }
            }
        }

        protected void btnConfirmerInfoVisiteur_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txtBoxEmailVisiteur.Text))
            {
                UtilitiesTool.stringUtilities stringTool = new UtilitiesTool.stringUtilities();

                if (stringTool.isValidEmail(txtBoxEmailVisiteur.Text))
                {
                    lightClientDatabaseObject lDb = new lightClientDatabaseObject(_databaseConnectionString);

                    bool bVisiteurMatch = lDb.rechercheVisiteur(txtBoxNomVisiteur.Text, txtBoxPrenVisiteur.Text, txtBoxEmailVisiteur.Text);
                    if (bVisiteurMatch)
                    {
                        Session["idVisiteur"] = lDb.getVisiteurId(txtBoxNomVisiteur.Text, txtBoxEmailVisiteur.Text);

                        panelEtape1.Visible = false;

                        //string sClass = divBarEtape2.Attributes["class"].Replace("activestep", "");
                        //this.divBarEtape1.Attributes["class"] = sClass;

                        //this.divBarEtape2.Attributes["class"] += " activestep";

                        panelEtape2.Visible = true;

                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "notification", "notificationError(" + (int)ErrorType.VisiteurAuthError + ");", true);
                    }
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "notification", "notificationError(" + (int)ErrorType.TextBoxAuthEmpty + ");", true);
            }
        }

        bool CheckTextBoxAuthValues()
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
            if (CheckTextBoxAuthValues())
            {
                List<String> txtBoxValues = new List<string>();

                foreach (Control control in divEtape2DemandeAutorisation.Controls)
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

                lightClientDatabaseObject lDb = new lightClientDatabaseObject(_databaseConnectionString);

                bool patientMatch = lDb.recherchePatient(txtBoxValues[3], txtBoxValues[4], txtBoxValues[5], 2);

                if (patientMatch)
                {
                    UtilitiesTool.stringUtilities stringTool = new UtilitiesTool.stringUtilities();

                    if (stringTool.isValidEmail(txtBoxValues[2]))
                    {
                        emailSenderObject emailSender = new emailSenderObject();

                        int patientId = lDb.getPatientId(txtBoxNomPatientAuth.Text, txtBoxPrenPatientAuth.Text, txtBoxChambrePatientAuth.Text, 1);
                        if (lDb.addToPrelist(patientId, txtBoxValues))
                        {
                            if (emailSender.sendNotification(txtBoxEmailVisiteurAuth.Text, emailSenderObject.NOTIFICATION.PrelisteAccepted, txtBoxNomPatientAuth.Text))
                            {
                                panelEtape1.Visible = false;
                                panelEtapeNotificationEnvoiAutorisation.Visible = true;
                            }

                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "notification", "notificationError(" + (int)ErrorType.VisiteurEmailNotValid + ");", true);
                    }

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "notification", "notificationError(" + (int)ErrorType.PatientNotFound + ");", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "notification", "notificationError(" + (int)ErrorType.TextBoxAuthEmpty + ");", true);
            }
            
        }

        protected void btnConfirmerPlageHoraire_Click(object sender, EventArgs e)
        {
            int indexPanel = 0;
            foreach (Panel control in divEtapeHoraire.Controls.OfType<Panel>()){
                foreach (RadioButton rb in control.Controls.OfType<RadioButton>().Select(c => c as RadioButton).Where(rb => rb.Checked)){
                    foreach (Control cont in rb.Parent.Controls)
                    {
                        if (cont is Label && cont.ID == "labelPlageHoraire" + indexPanel)
                        {
                            Label label = cont as Label;

                            myModalSubTitle.Text = label.Text;
                            myModalSubTitle.Style.Value = "margin-right:15px;";

                            labelHeureDebVisite.Style.Value = "margin-bottom:10px;";
                            labelHeureFinVisite.Style.Value = "margin-bottom:10px;";

                            InputValueDebutVisite = label.Text.Split(' ').First();
                            InputValueFinVisite = label.Text.Split(' ').Last();

                        }
                    }
                    ScriptManager.RegisterStartupScript(this, GetType(), "Pop", "openModal();", true);
                    ScriptManager.RegisterStartupScript(this, GetType(), "openTimeMin", "openTimeMin();", true);
                    ScriptManager.RegisterStartupScript(this, GetType(), "openTimeMax", "openTimeMax();", true);
                }
                indexPanel++;
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

            lightClientDatabaseObject lDb = new lightClientDatabaseObject(_databaseConnectionString);

            if (timeTool.compareTimeSpan(TimeSpan.Parse(heureDebutVisite), plageDebutVisite) && timeTool.compareTimeSpan(plageFinVisite, TimeSpan.Parse(heureFinVisite)))
            {
                if (lDb.getStatusPatient((int)Session["idPatient"]) == 1)
                {
                    if (lDb.sendDemandeDeVisite(TimeSpan.Parse(heureDebutVisite), TimeSpan.Parse(heureFinVisite), (int)Session["idPatient"], (int)Session["idVisiteur"], guidBonVisite, 1))
                    {
                        generator.generateQRCode(guidBonVisite, DateTime.Today.ToString(CultureInfo.CurrentCulture), heureDebutVisite, heureFinVisite, txtBoxNomPatient.Text, txtBoxPrenPatient.Text, "2", "150");

                        //emailSender.sendNotification("t.maalem@aforp.eu", emailSenderObject.NOTIFICATION.Accepted, generator.QRCodeCompletePath);

                        panelEtape3.Visible = false;
                        panelInfoDemandeDeVisiteFinal.Visible = true;
                    }
                }
                else if (lDb.getStatusPatient((int)Session["idPatient"]) == 3)
                {
                    if (lDb.sendDemandeDeVisite(TimeSpan.Parse(heureDebutVisite), TimeSpan.Parse(heureFinVisite), (int)Session["idVisiteur"], (int)Session["idPatient"], guidBonVisite, 3))
                    {
                        panelEtape3.Visible = false;
                        panelInfoDemandeDeVisiteBesoinConfirmation.Visible = true;
                    }
                }
            }
        }

        private void DisplayLabel(TimeSpan[,] horaire, TimeSpan[,] horaireIndispo)
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

            labelInfoAffluence.Style.Value = "margin-left:5px;";
            labelInfoChoixHoraire.Style.Value = "margin-left:25px;";

            UtilitiesTool.stringUtilities stringTool = new UtilitiesTool.stringUtilities();

            if (Session["panelList"] == null)
            {
                for (int i = 0; i < horaire.GetLength(0); i++)
                {
                    Panel myPanel = new Panel {ID = "panelPlageHoraire" + i, CssClass = "form-group"};

                    myPanel.Style.Value = "margin-left:50px";

                    Label myLabel1 = new Label
                    {
                        ID = "labelPlageHoraire" + i,
                        Text =
                            stringTool.splitString(horaire[i, 0].ToString()) + @" - " +
                            stringTool.splitString(horaire[i, 1].ToString()),
                        CssClass = "col-md-3 control-label"
                    };


                    Label myLabel2 = new Label();

                    string etat = String.Empty;

                    myLabel2.ID = "labelAffluence" + i;

                    lightClientDatabaseObject lDb = new lightClientDatabaseObject(_databaseConnectionString);

                    int affluence = lDb.getAffluence(horaire[i, 0], horaire[i, 1], 1, (int)Session["idPatient"]);
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

                    RadioButton myRadiobutton = new RadioButton
                    {
                        ID = "radioBtnHoraire" + i,
                        CssClass = "col-md-3 control-label",
                        GroupName = "rBtnGroup"
                    };

                    myPanel.Controls.Add(myLabel1);
                    myPanel.Controls.Add(myLabel2);
                    myPanel.Controls.Add(myRadiobutton);

                    panelList.Add(myPanel);
                    divEtapeHoraire.Controls.Add(myPanel);

                    Session["panelList"] = panelList;

                }

                if (horaireIndispo[0, 0] != TimeSpan.Zero)
                {
                    plageHoraireIndispoTitre.Visible = true;

                    for (int i = 0; i < horaireIndispo.GetLength(0); i++)
                    {
                        Panel myPanel = new Panel {ID = "panelPlageHoraireIndispo" + i, CssClass = "form-group"};
                        myPanel.Style.Value = "margin-left:50px";

                        Label myLabel1 = new Label
                        {
                            ID = "labelPlageHoraireIndispo" + i,
                            Text =
                                stringTool.splitString(horaireIndispo[i, 0].ToString()) + @" - " +
                                stringTool.splitString(horaireIndispo[i, 1].ToString()),
                            CssClass = "col-md-3 control-label"
                        };

                        Label myLabel2 = new Label
                        {
                            ID = "labelAffluenceIndispo" + i,
                            Text = @"Indisponible",
                            CssClass = "col-md-3 control-label"
                        };
                        myLabel2.Style.Value = "margin-left:20px;";

                        RadioButton myRadioButton = new RadioButton
                        {
                            ID = "radioBtnHoraireIndispo" + i,
                            CssClass = "col-md-3 control-label radioButtonIndispo",
                            GroupName = "rBtnGroup",
                            Enabled = false
                        };

                        myPanel.Controls.Add(myLabel1);
                        myPanel.Controls.Add(myLabel2);
                        myPanel.Controls.Add(myRadioButton);

                        panelListIndispo.Add(myPanel);
                        divEtapeHoraireIndisponible.Controls.Add(myPanel);

                        Session["panelListIndispo"] = panelListIndispo;

                    }
                }
            }
        }
    }
}