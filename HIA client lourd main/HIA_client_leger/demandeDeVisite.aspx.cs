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
                this.InitPageControl();
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
            if (Master != null)
            {
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
                UtilitiesTool.TimeManipulation timeTool = new UtilitiesTool.TimeManipulation();
                LightClientDatabaseObject lDb = new LightClientDatabaseObject(_databaseConnectionString);

                bool bPatientMatch = lDb.RecherchePatient(txtBoxNomPatient.Text, txtBoxPrenPatient.Text, txtBoxCodePatient.Text, 1);
                if (bPatientMatch)
                {
                    if (lDb.IsVisiteurInPreList((int)Session["idVisiteur"], txtBoxNomPatient.Text))
                    {
                        Session["idPatient"] = lDb.GetPatientId(txtBoxNomPatient.Text, txtBoxPrenPatient.Text, String.Empty, 2);
                        int statusPatient = lDb.GetStatusPatient((int)Session["idPatient"]);
                        if (statusPatient != 0)
                        {
                            panelEtape2.Visible = false;
                            //string sClass = divBarEtape1.Attributes["class"].Replace("activestep", "");
                            //this.divBarEtape2.Attributes["class"] = sClass;

                            //this.divBarEtape3.Attributes["class"] += " activestep";

                            TimeSpan[,] plageHoraire = lDb.GetSchedule(txtBoxNomPatient.Text, txtBoxPrenPatient.Text);

                            this.DisplayLabel(timeTool.GetDispo(plageHoraire, _horaireDebutVisite, _horaireFinVisite), plageHoraire);
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
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "notification", "notificationError(" + (int)ErrorType.VisiteurNotInPreList + ");", true);
                    }

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "notification", "notificationError(" + (int)ErrorType.PatientNotFound + ");", true);
                }
            }
        }

        protected void btnConfirmerInfoVisiteur_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txtBoxEmailVisiteur.Text))
            {
                UtilitiesTool.StringUtilities stringTool = new UtilitiesTool.StringUtilities();

                if (stringTool.IsValidEmail(txtBoxEmailVisiteur.Text))
                {
                    LightClientDatabaseObject lDb = new LightClientDatabaseObject(_databaseConnectionString);

                    bool bVisiteurMatch = lDb.RechercheVisiteur(txtBoxNomVisiteur.Text, txtBoxPrenVisiteur.Text, txtBoxEmailVisiteur.Text);
                    if (bVisiteurMatch)
                    {
                        Session["idVisiteur"] = lDb.GetVisiteurId(txtBoxNomVisiteur.Text, txtBoxEmailVisiteur.Text);

                        panelEtape1.Visible = false;

                        //string sClass = divBarEtape2.Attributes["class"].Replace("activestep", "");
                        //this.divBarEtape1.Attributes["class"] = sClass;

                        //this.divBarEtape2.Attributes["class"] += " activestep";

                        panelEtape2.Visible = true;

                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "notification", "notificationError(" + (int)ErrorType.VisiteurAuthError + ");", true);
                    }
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "notification", "notificationError(" + (int)ErrorType.TextBoxAuthEmpty + ");", true);
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
            if (this.CheckTextBoxAuthValues())
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

                LightClientDatabaseObject lDb = new LightClientDatabaseObject(_databaseConnectionString);

                bool patientMatch = lDb.RecherchePatient(txtBoxValues[3], txtBoxValues[4], txtBoxValues[5], 2);

                if (patientMatch)
                {
                    UtilitiesTool.StringUtilities stringTool = new UtilitiesTool.StringUtilities();

                    if (stringTool.IsValidEmail(txtBoxValues[2]))
                    {
                        EmailSenderObject emailSender = new EmailSenderObject();

                        int patientId = lDb.GetPatientId(txtBoxNomPatientAuth.Text, txtBoxPrenPatientAuth.Text, txtBoxChambrePatientAuth.Text, 1);
                        if (lDb.AddToPrelist(patientId, txtBoxValues))
                        {
                            if (emailSender.SendNotification(txtBoxEmailVisiteurAuth.Text, EmailSenderObject.Notification.PrelisteAccepted, txtBoxNomPatientAuth.Text))
                            {
                                panelEtape1.Visible = false;
                                panelEtapeNotificationEnvoiAutorisation.Visible = true;
                            }

                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "notification", "notificationError(" + (int)ErrorType.VisiteurEmailNotValid + ");", true);
                    }

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "notification", "notificationError(" + (int)ErrorType.PatientNotFound + ");", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "notification", "notificationError(" + (int)ErrorType.TextBoxAuthEmpty + ");", true);
            }

        }

        protected void btnConfirmerPlageHoraire_Click(object sender, EventArgs e)
        {
            int indexPanel = 0;
            foreach (Panel control in divEtapeHoraire.Controls.OfType<Panel>())
            {
                foreach (RadioButton rb in control.Controls.OfType<RadioButton>().Select(c => c as RadioButton).Where(rb => rb.Checked))
                {
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
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "openTimeMin", "openTimeMin();", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "openTimeMax", "openTimeMax();", true);
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

            UtilitiesTool.TimeManipulation timeTool = new UtilitiesTool.TimeManipulation();
            UtilitiesTool.StringUtilities stringTool = new UtilitiesTool.StringUtilities();
            string guidBonVisite = stringTool.GenerateGuid();

            QrGenerator generator = new QrGenerator();

            EmailSenderObject emailSender = new EmailSenderObject();

            LightClientDatabaseObject lDb = new LightClientDatabaseObject(_databaseConnectionString);

            if (timeTool.CompareTimeSpan(TimeSpan.Parse(heureDebutVisite), plageDebutVisite) && timeTool.CompareTimeSpan(plageFinVisite, TimeSpan.Parse(heureFinVisite)))
            {
                if (lDb.GetStatusPatient((int)Session["idPatient"]) == 1)
                {
                    if (lDb.SendDemandeDeVisite(TimeSpan.Parse(heureDebutVisite), TimeSpan.Parse(heureFinVisite), (int)Session["idPatient"], (int)Session["idVisiteur"], guidBonVisite, 1))
                    {
                        generator.GenerateQrCode(guidBonVisite, DateTime.Today.ToString(CultureInfo.CurrentCulture), heureDebutVisite, heureFinVisite, txtBoxNomPatient.Text, txtBoxPrenPatient.Text, "2", "150");

                        //emailSender.SendNotification("t.maalem@aforp.eu", emailSenderObject.Notification.Accepted, Generator.QRCodeCompletePath);

                        panelEtape3.Visible = false;
                        panelInfoDemandeDeVisiteFinal.Visible = true;
                    }
                }
                else if (lDb.GetStatusPatient((int)Session["idPatient"]) == 3)
                {
                    if (lDb.SendDemandeDeVisite(TimeSpan.Parse(heureDebutVisite), TimeSpan.Parse(heureFinVisite), (int)Session["idVisiteur"], (int)Session["idPatient"], guidBonVisite, 3))
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

            UtilitiesTool.StringUtilities stringTool = new UtilitiesTool.StringUtilities();

            if (Session["panelList"] == null)
            {
                for (int i = 0; i < horaire.GetLength(0); i++)
                {
                    Panel myPanel = new Panel { ID = "panelPlageHoraire" + i, CssClass = "form-group" };

                    myPanel.Style.Value = "margin-left:50px";

                    Label myLabel1 = new Label
                    {
                        ID = "labelPlageHoraire" + i,
                        Text =
                            stringTool.SplitString(horaire[i, 0].ToString()) + @" - " +
                            stringTool.SplitString(horaire[i, 1].ToString()),
                        CssClass = "col-md-3 control-label"
                    };


                    Label myLabel2 = new Label();

                    string etat = String.Empty;

                    myLabel2.ID = "labelAffluence" + i;

                    LightClientDatabaseObject lDb = new LightClientDatabaseObject(_databaseConnectionString);

                    int affluence = lDb.GetAffluence(horaire[i, 0], horaire[i, 1], 1, (int)Session["idPatient"]);
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
                        Panel myPanel = new Panel { ID = "panelPlageHoraireIndispo" + i, CssClass = "form-group" };
                        myPanel.Style.Value = "margin-left:50px";

                        Label myLabel1 = new Label
                        {
                            ID = "labelPlageHoraireIndispo" + i,
                            Text =
                                stringTool.SplitString(horaireIndispo[i, 0].ToString()) + @" - " +
                                stringTool.SplitString(horaireIndispo[i, 1].ToString()),
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