using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Text.RegularExpressions;
using System.Globalization;
using databaseHIA;
using Utilities;

namespace HIA_client_leger
{
    public partial class demandeDeVisite : System.Web.UI.Page
    {
        protected string inputValueDebutVisite { get; set; }
        protected string inputValueFinVisite { get; set; }

        private string databaseConnectionString = WebConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString;

        private TimeSpan horaireDebutVisite = TimeSpan.FromHours(8);
        private TimeSpan horaireFinVisite = TimeSpan.FromHours(22);

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                initPageControl();
            }
            if (Session["panelList"] != null)
            {
                foreach (Control control in (List<Panel>)Session["panelList"])
                {
                    if (control is Panel)
                    {
                        divEtapeHoraire.Controls.Add(control);
                    }
                }
                foreach (Control control in (List<Panel>)Session["panelListIndispo"])
                {
                    if (control is Panel)
                    {
                        divEtapeHoraireIndisponible.Controls.Add(control);
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
                lightClientDatabaseObject lDB = new lightClientDatabaseObject(databaseConnectionString);

                bool bPatientMatch = lDB.recherchePatient(txtBoxNomPatient.Text, txtBoxPrenPatient.Text, txtBoxCodePatient.Text, 1);
                if (bPatientMatch)
                {
                    Session["idPatient"] = lDB.getPatientId(txtBoxNomPatient.Text, txtBoxPrenPatient.Text, "", 2);

                    panelEtape2.Visible = false;
                    string sClass = divBarEtape1.Attributes["class"].Replace("activestep", "");
                    divBarEtape2.Attributes["class"] = sClass;

                    divBarEtape3.Attributes["class"] += " activestep";

                    TimeSpan[,] plageHoraire = lDB.getSchedule(txtBoxNomPatient.Text, txtBoxPrenPatient.Text);

                    displayLabel(timeTool.getDispo(plageHoraire, this.horaireDebutVisite, this.horaireFinVisite), plageHoraire);
                    panelEtape3.Visible = true;

                }
                else
                {
                    panelEtape2.Visible = false;
                    panelEtapeInfoPatientError.Visible = true;
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
                    lightClientDatabaseObject lDB = new lightClientDatabaseObject(databaseConnectionString);

                    bool bVisiteurMatch = lDB.rechercheVisiteur(txtBoxNomVisiteur.Text, txtBoxPrenVisiteur.Text, txtBoxEmailVisiteur.Text);
                    if (bVisiteurMatch)
                    {
                        Session["idVisiteur"] = lDB.getVisiteurId(txtBoxNomVisiteur.Text, txtBoxEmailVisiteur.Text);

                        panelEtape1.Visible = false;
                        string sClass = divBarEtape2.Attributes["class"].Replace("activestep", "");
                        divBarEtape1.Attributes["class"] = sClass;

                        divBarEtape2.Attributes["class"] += " activestep";

                        panelEtape2.Visible = true;

                    }
                    else
                    {
                        panelEtape1.Visible = false;
                        panelEtapeInfoVisiteurError.Visible = true;
                    }
                }
            }
        }

        protected void btnConfirmerInfoVisiteurAuth_Click(object sender, EventArgs e)
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

            lightClientDatabaseObject lDB = new lightClientDatabaseObject(databaseConnectionString);

            bool patientMatch = lDB.recherchePatient(txtBoxValues[3], txtBoxValues[4], txtBoxValues[5], 2);

            if (patientMatch)
            {
                UtilitiesTool.stringUtilities stringTool = new UtilitiesTool.stringUtilities();
                if (stringTool.isValidEmail(txtBoxValues[2]))
                {
                    int patientID = lDB.getPatientId(txtBoxNomPatientAuth.Text, txtBoxPrenPatientAuth.Text, txtBoxChambrePatientAuth.Text, 1);
                    bool addSuccess = lDB.addToPrelist(patientID, txtBoxValues);
                    if (addSuccess)
                    {
                        string patientNumViste = lDB.getPatientNumVisite(patientID);

                        panelEtape1.Visible = false;
                        panelEtapeNotificationEnvoiAutorisation.Visible = true;
                    }
                }
                else
                {
                    panelEtape1.Visible = false;
                    panelEtapeInfoPatientError.Visible = true;
                }

            }
            else
            {
                panelEtape1.Visible = false;
                panelEtapeInfoPatientError.Visible = true;
            }
        }

        protected void btnConfirmerPlageHoraire_Click(object sender, EventArgs e)
        {
            int indexPanel = 0;
            foreach (Control control in divEtapeHoraire.Controls)
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

                                        myModalSubTitle.Text = label.Text;
                                        myModalSubTitle.Style.Value = "margin-right:15px;";

                                        labelHeureDebVisite.Style.Value = "margin-bottom:10px;";
                                        labelHeureFinVisite.Style.Value = "margin-bottom:10px;";

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

            lightClientDatabaseObject lDB = new lightClientDatabaseObject(databaseConnectionString);

            if (timeTool.compareTimeSpan(TimeSpan.Parse(heureDebutVisite), plageDebutVisite) && timeTool.compareTimeSpan(plageFinVisite, TimeSpan.Parse(heureFinVisite)))
            {
                if (lDB.getStatusPatient((int)Session["idPatient"]) == 1)
                {
                    if (lDB.sendDemandeDeVisite(TimeSpan.Parse(heureDebutVisite), TimeSpan.Parse(heureFinVisite), (int)Session["idVisiteur"], (int)Session["idPatient"], stringTool.generateGUID(), 1))
                    {
                        panelEtape3.Visible = false;
                        panelInfoDemandeDeVisiteFinal.Visible = true;
                    }
                }
                else if (lDB.getStatusPatient((int)Session["idPatient"]) == 3)
                {
                    if (lDB.sendDemandeDeVisite(TimeSpan.Parse(heureDebutVisite), TimeSpan.Parse(heureFinVisite), (int)Session["idVisiteur"], (int)Session["idPatient"], stringTool.generateGUID(), 3))
                    {
                        panelEtape3.Visible = false;
                        panelInfoDemandeDeVisiteBesoinConfirmation.Visible = true;
                    }
                }
            }
            else
            {
                //divErreurHoraire.Visible = true;
            }
        }       

        private void displayLabel(TimeSpan[,] horaire, TimeSpan[,] horaireIndispo)
        {
            List<Panel> panelList = new List<Panel>();
            List<Panel> panelListIndispo = new List<Panel>();

            labelInfoAffluence.Style.Value = "margin-left:5px;";
            labelInfoChoixHoraire.Style.Value = "margin-left:25px;";

            UtilitiesTool.stringUtilities stringTool = new UtilitiesTool.stringUtilities();

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

                lightClientDatabaseObject lDB = new lightClientDatabaseObject(databaseConnectionString);

                int affluence = lDB.getAffluence(horaire[i, 0], horaire[i, 1]);
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
                divEtapeHoraire.Controls.Add(myPanel);

                Session["panelList"] = panelList;

            }

            if (horaireIndispo[0, 0] != TimeSpan.Zero)
            {
                plageHoraireIndispoTitre.Visible = true;

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
                    divEtapeHoraireIndisponible.Controls.Add(myPanel);

                    Session["panelListIndispo"] = panelListIndispo;

                }
            }
        }
    }
}