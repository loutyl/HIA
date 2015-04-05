using Enumerable = System.Linq.Enumerable;

namespace HIA_client_leger
{
    public partial class DemandeDeVisite : System.Web.UI.Page
    {
        protected string InputValueDebutVisite { get; set; }
        protected string InputValueFinVisite { get; set; }
        private readonly string _databaseConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString;
        private readonly System.TimeSpan _horaireDebutVisite = System.TimeSpan.FromHours(8);
        private readonly System.TimeSpan _horaireFinVisite = System.TimeSpan.FromHours(22);

        public enum ErrorType
        {
            VisiteurAuthError = 1,
            VisiteurEmailNotValid = 2,
            VisiteurNotInPreList = 3,

            PatientNotFound = 4,

            TextBoxAuthEmpty = 5
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {

            if (!IsPostBack)
            {
                this.InitPageControl();
            }
            if (Session["panelList"] != null)
            {
                foreach (System.Web.UI.WebControls.Panel control in (System.Collections.Generic.List<System.Web.UI.WebControls.Panel>)Session["panelList"])
                {
                    if (control != null)
                    {
                        divEtapeHoraire.Controls.Add(control);
                    }
                }
                if (Session["panelListIndispo"] != null)
                {
                    foreach (System.Web.UI.WebControls.Panel control in (System.Collections.Generic.List<System.Web.UI.WebControls.Panel>)Session["panelListIndispo"])
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
                System.Web.UI.HtmlControls.HtmlGenericControl liItem = (System.Web.UI.HtmlControls.HtmlGenericControl)Master.FindControl("demandeDeVisite");
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

        protected void btnConfirmerInfoPatient_Click(object sender, System.EventArgs e)
        {
            if (!System.String.IsNullOrWhiteSpace(txtBoxNomPatient.Text) && !System.String.IsNullOrWhiteSpace(txtBoxPrenPatient.Text) &&
            !System.String.IsNullOrWhiteSpace(txtBoxCodePatient.Text))
            {
                Utilities.UtilitiesTool.TimeManipulation timeTool = new Utilities.UtilitiesTool.TimeManipulation();
                databaseHIA.LightClientDatabaseObject lDb = new databaseHIA.LightClientDatabaseObject(_databaseConnectionString);

                bool bPatientMatch = lDb.RecherchePatient(txtBoxNomPatient.Text, txtBoxPrenPatient.Text, txtBoxCodePatient.Text, 1);
                if (bPatientMatch)
                {
                    if (lDb.IsVisiteurInPreList((int)Session["idVisiteur"], txtBoxNomPatient.Text))
                    {
                        Session["idPatient"] = lDb.GetPatientId(txtBoxNomPatient.Text, txtBoxPrenPatient.Text, System.String.Empty, 2);
                        int statusPatient = lDb.GetStatusPatient((int)Session["idPatient"]);
                        if (statusPatient != 0)
                        {
                            panelEtape2.Visible = false;
                            //string sClass = divBarEtape1.Attributes["class"].Replace("activestep", "");
                            //this.divBarEtape2.Attributes["class"] = sClass;

                            //this.divBarEtape3.Attributes["class"] += " activestep";

                            System.TimeSpan[,] plageHoraire = lDb.GetSchedule(txtBoxNomPatient.Text, txtBoxPrenPatient.Text);

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
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "notification", "notificationError(" + (int)ErrorType.VisiteurNotInPreList + ");", true);
                    }

                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "notification", "notificationError(" + (int)ErrorType.PatientNotFound + ");", true);
                }
            }
        }

        protected void btnConfirmerInfoVisiteur_Click(object sender, System.EventArgs e)
        {
            if (!System.String.IsNullOrWhiteSpace(txtBoxEmailVisiteur.Text))
            {
                Utilities.UtilitiesTool.StringUtilities stringTool = new Utilities.UtilitiesTool.StringUtilities();

                if (stringTool.IsValidEmail(txtBoxEmailVisiteur.Text))
                {
                    databaseHIA.LightClientDatabaseObject lDb = new databaseHIA.LightClientDatabaseObject(_databaseConnectionString);

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
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "notification", "notificationError(" + (int)ErrorType.VisiteurAuthError + ");", true);
                    }
                }
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "notification", "notificationError(" + (int)ErrorType.TextBoxAuthEmpty + ");", true);
            }
        }

        bool CheckTextBoxAuthValues()
        {
            foreach (System.Web.UI.Control control in divEtape2DemandeAutorisation.Controls)
            {
                if (control is System.Web.UI.WebControls.TextBox)
                {
                    System.Web.UI.WebControls.TextBox tb = control as System.Web.UI.WebControls.TextBox;
                    if (!System.String.IsNullOrWhiteSpace(tb.Text))
                    {
                        return true;
                    }
                }
            }
            return false;

        }

        protected void btnConfirmerInfoVisiteurAuth_Click(object sender, System.EventArgs e)
        {
            if (this.CheckTextBoxAuthValues())
            {
                System.Collections.Generic.List<System.String> txtBoxValues = new System.Collections.Generic.List<string>();

                foreach (System.Web.UI.Control control in divEtape2DemandeAutorisation.Controls)
                {
                    if (control is System.Web.UI.WebControls.TextBox)
                    {
                        System.Web.UI.WebControls.TextBox tb = control as System.Web.UI.WebControls.TextBox;
                        if (!System.String.IsNullOrWhiteSpace(tb.Text))
                        {
                            txtBoxValues.Add(tb.Text);
                        }
                    }
                }

                databaseHIA.LightClientDatabaseObject lDb = new databaseHIA.LightClientDatabaseObject(_databaseConnectionString);

                bool patientMatch = lDb.RecherchePatient(txtBoxValues[3], txtBoxValues[4], txtBoxValues[5], 2);

                if (patientMatch)
                {
                    Utilities.UtilitiesTool.StringUtilities stringTool = new Utilities.UtilitiesTool.StringUtilities();

                    if (stringTool.IsValidEmail(txtBoxValues[2]))
                    {
                        emailSender.EmailSenderObject emailSender = new emailSender.EmailSenderObject();

                        int patientId = lDb.GetPatientId(txtBoxNomPatientAuth.Text, txtBoxPrenPatientAuth.Text, txtBoxChambrePatientAuth.Text, 1);
                        if (lDb.AddToPrelist(patientId, txtBoxValues))
                        {
                            if (emailSender.SendNotification(txtBoxEmailVisiteurAuth.Text, global::emailSender.EmailSenderObject.Notification.PrelisteAccepted, txtBoxNomPatientAuth.Text))
                            {
                                panelEtape1.Visible = false;
                                panelEtapeNotificationEnvoiAutorisation.Visible = true;
                            }

                        }
                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "notification", "notificationError(" + (int)ErrorType.VisiteurEmailNotValid + ");", true);
                    }

                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "notification", "notificationError(" + (int)ErrorType.PatientNotFound + ");", true);
                }
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "notification", "notificationError(" + (int)ErrorType.TextBoxAuthEmpty + ");", true);
            }

        }

        protected void btnConfirmerPlageHoraire_Click(object sender, System.EventArgs e)
        {
            int indexPanel = 0;
            foreach (System.Web.UI.WebControls.Panel control in Enumerable.OfType<System.Web.UI.WebControls.Panel>(divEtapeHoraire.Controls))
            {
                foreach (System.Web.UI.WebControls.RadioButton rb in Enumerable.Where(Enumerable.Select(Enumerable.OfType<System.Web.UI.WebControls.RadioButton>(control.Controls), c => c as System.Web.UI.WebControls.RadioButton), rb => rb.Checked))
                {
                    foreach (System.Web.UI.Control cont in rb.Parent.Controls)
                    {
                        if (cont is System.Web.UI.WebControls.Label && cont.ID == "labelPlageHoraire" + indexPanel)
                        {
                            System.Web.UI.WebControls.Label label = cont as System.Web.UI.WebControls.Label;

                            myModalSubTitle.Text = label.Text;
                            myModalSubTitle.Style.Value = "margin-right:15px;";

                            labelHeureDebVisite.Style.Value = "margin-bottom:10px;";
                            labelHeureFinVisite.Style.Value = "margin-bottom:10px;";

                            InputValueDebutVisite = Enumerable.First(label.Text.Split(' '));
                            InputValueFinVisite = Enumerable.Last(label.Text.Split(' '));

                        }
                    }
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "openTimeMin", "openTimeMin();", true);
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "openTimeMax", "openTimeMax();", true);
                }
                indexPanel++;
            }
        }

        protected void btnConfirmerHeureModal_Click(object sender, System.EventArgs e)
        {
            string heureDebutVisite = Request.Form["heureDebutVisite"];
            string heureFinVisite = Request.Form["heureFinVisite"];

            System.TimeSpan plageDebutVisite = System.TimeSpan.Parse(Enumerable.First(myModalSubTitle.Text.Split(' ')));
            System.TimeSpan plageFinVisite = System.TimeSpan.Parse(Enumerable.Last(myModalSubTitle.Text.Split(' ')));

            Utilities.UtilitiesTool.TimeManipulation timeTool = new Utilities.UtilitiesTool.TimeManipulation();
            Utilities.UtilitiesTool.StringUtilities stringTool = new Utilities.UtilitiesTool.StringUtilities();
            string guidBonVisite = stringTool.GenerateGuid();

            QRcodeGenerator.QrGenerator generator = new QRcodeGenerator.QrGenerator();

            emailSender.EmailSenderObject emailSender = new emailSender.EmailSenderObject();

            databaseHIA.LightClientDatabaseObject lDb = new databaseHIA.LightClientDatabaseObject(_databaseConnectionString);

            if (timeTool.CompareTimeSpan(System.TimeSpan.Parse(heureDebutVisite), plageDebutVisite) && timeTool.CompareTimeSpan(plageFinVisite, System.TimeSpan.Parse(heureFinVisite)))
            {
                if (lDb.GetStatusPatient((int)Session["idPatient"]) == 1)
                {
                    if (lDb.SendDemandeDeVisite(System.TimeSpan.Parse(heureDebutVisite), System.TimeSpan.Parse(heureFinVisite), (int)Session["idPatient"], (int)Session["idVisiteur"], guidBonVisite, 1))
                    {
                        generator.GenerateQrCode(guidBonVisite, System.DateTime.Today.ToString(System.Globalization.CultureInfo.CurrentCulture), heureDebutVisite, heureFinVisite, txtBoxNomPatient.Text, txtBoxPrenPatient.Text, "2", "150");

                        //emailSender.SendNotification("t.maalem@aforp.eu", emailSenderObject.Notification.Accepted, Generator.QRCodeCompletePath);

                        panelEtape3.Visible = false;
                        panelInfoDemandeDeVisiteFinal.Visible = true;
                    }
                }
                else if (lDb.GetStatusPatient((int)Session["idPatient"]) == 3)
                {
                    if (lDb.SendDemandeDeVisite(System.TimeSpan.Parse(heureDebutVisite), System.TimeSpan.Parse(heureFinVisite), (int)Session["idVisiteur"], (int)Session["idPatient"], guidBonVisite, 3))
                    {
                        panelEtape3.Visible = false;
                        panelInfoDemandeDeVisiteBesoinConfirmation.Visible = true;
                    }
                }
            }
        }

        private void DisplayLabel(System.TimeSpan[,] horaire, System.TimeSpan[,] horaireIndispo)
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

            System.Collections.Generic.List<System.Web.UI.WebControls.Panel> panelList = new System.Collections.Generic.List<System.Web.UI.WebControls.Panel>();
            System.Collections.Generic.List<System.Web.UI.WebControls.Panel> panelListIndispo = new System.Collections.Generic.List<System.Web.UI.WebControls.Panel>();

            labelInfoAffluence.Style.Value = "margin-left:5px;";
            labelInfoChoixHoraire.Style.Value = "margin-left:25px;";

            Utilities.UtilitiesTool.StringUtilities stringTool = new Utilities.UtilitiesTool.StringUtilities();

            if (Session["panelList"] == null)
            {
                for (int i = 0; i < horaire.GetLength(0); i++)
                {
                    System.Web.UI.WebControls.Panel myPanel = new System.Web.UI.WebControls.Panel { ID = "panelPlageHoraire" + i, CssClass = "form-group" };

                    myPanel.Style.Value = "margin-left:50px";

                    System.Web.UI.WebControls.Label myLabel1 = new System.Web.UI.WebControls.Label
                    {
                        ID = "labelPlageHoraire" + i,
                        Text =
                            stringTool.SplitString(horaire[i, 0].ToString()) + @" - " +
                            stringTool.SplitString(horaire[i, 1].ToString()),
                        CssClass = "col-md-3 control-label"
                    };


                    System.Web.UI.WebControls.Label myLabel2 = new System.Web.UI.WebControls.Label();

                    string etat = System.String.Empty;

                    myLabel2.ID = "labelAffluence" + i;

                    databaseHIA.LightClientDatabaseObject lDb = new databaseHIA.LightClientDatabaseObject(_databaseConnectionString);

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

                    System.Web.UI.WebControls.RadioButton myRadiobutton = new System.Web.UI.WebControls.RadioButton
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

                if (horaireIndispo[0, 0] != System.TimeSpan.Zero)
                {
                    plageHoraireIndispoTitre.Visible = true;

                    for (int i = 0; i < horaireIndispo.GetLength(0); i++)
                    {
                        System.Web.UI.WebControls.Panel myPanel = new System.Web.UI.WebControls.Panel { ID = "panelPlageHoraireIndispo" + i, CssClass = "form-group" };
                        myPanel.Style.Value = "margin-left:50px";

                        System.Web.UI.WebControls.Label myLabel1 = new System.Web.UI.WebControls.Label
                        {
                            ID = "labelPlageHoraireIndispo" + i,
                            Text =
                                stringTool.SplitString(horaireIndispo[i, 0].ToString()) + @" - " +
                                stringTool.SplitString(horaireIndispo[i, 1].ToString()),
                            CssClass = "col-md-3 control-label"
                        };

                        System.Web.UI.WebControls.Label myLabel2 = new System.Web.UI.WebControls.Label
                        {
                            ID = "labelAffluenceIndispo" + i,
                            Text = @"Indisponible",
                            CssClass = "col-md-3 control-label"
                        };
                        myLabel2.Style.Value = "margin-left:20px;";

                        System.Web.UI.WebControls.RadioButton myRadioButton = new System.Web.UI.WebControls.RadioButton
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