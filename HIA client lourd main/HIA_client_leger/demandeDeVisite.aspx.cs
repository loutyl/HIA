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

namespace HIA_client_leger
{
    public partial class demandeDeVisite : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                initPageControl();
            }

        }

        private void initPageControl()
        {
            //Ajout de la class "active" à la li de la master page            
            HtmlGenericControl liItem = (HtmlGenericControl)Master.FindControl("demandeDeVisite");
            liItem.Attributes.Add("class", "active");

            #region init textBox etape1 placeholder
            txtBoxNomPatient.Attributes.Add("placeholder", "Nom du patient");
            txtBoxPrenPatient.Attributes.Add("placeholder", "Prénom du patient");
            txtBoxCodePatient.Attributes.Add("placeholder", "Exemple : A12B45");
            #endregion

            #region init textBox etape2 placeholder
            txtBoxNomVisiteur.Attributes.Add("placeholder", "Votre nom");
            txtBoxPrenVisiteur.Attributes.Add("placeholder", "Votre prénom");
            txtBoxEmailVisiteur.Attributes.Add("placeholder", "Votre adresse email");
            txtBoxTelVisiteur.Attributes.Add("placeholder", "Numéro de téléphone portable");
            #endregion

        }

        protected void btnConfirmerInfoPatient_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txtBoxNomPatient.Text) && !String.IsNullOrWhiteSpace(txtBoxPrenPatient.Text) &&
            !String.IsNullOrWhiteSpace(txtBoxCodePatient.Text))
            {
                bool bPatientMatch = recherchePatient(txtBoxNomPatient.Text, txtBoxPrenPatient.Text, txtBoxCodePatient.Text);

                if (bPatientMatch)
                {
                    panelEtape2.Visible = false;
                    string sClass = divBarEtape1.Attributes["class"].Replace("activestep", "");
                    divBarEtape2.Attributes["class"] = sClass;

                    divBarEtape3.Attributes["class"] += " activestep";

                    TimeSpan[,] plageHoraire = getSchedule(txtBoxNomPatient.Text, txtBoxPrenPatient.Text);

                    displayLabel(getDispo(plageHoraire));

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
                if (isValidEmail(txtBoxEmailVisiteur.Text))
                {
                    bool bVisiteurMatch = rechercheVisiteur(txtBoxNomVisiteur.Text, txtBoxPrenVisiteur.Text, txtBoxEmailVisiteur.Text, txtBoxTelVisiteur.Text);

                    if (bVisiteurMatch)
                    {
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

        protected void btnConfirmerPlageHoraire_Click(object sender, EventArgs e)
        {
            foreach (System.Web.UI.WebControls.Panel panel in divEtapeHoraire.Controls.OfType<System.Web.UI.WebControls.Panel>())
            {
                foreach (Control control in panel.Controls)
                {
                    if (control is RadioButton)
                    {
                        RadioButton rb = control as RadioButton;
                        if (rb.Checked)
                        {
                            panelEtape2.Visible = true;
                        }
                    }
                }
            }
        }

        private bool isValidEmail(string sEmail)
        {
            string symbole = "^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$";

            if (Regex.IsMatch(sEmail, symbole, RegexOptions.IgnoreCase))
            {
                return true;
            }
            return false;
        }

        //Méthode de recherche d'un visiteur dans la base
        private bool rechercheVisiteur(string sNomVisiteur, string sPrenVisiteur, string sEmailVisiteur, string sTelVisiteur)
        {
            bool bRet = false;

            SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString);

            //Ouverture d'une connection à la base de données
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT Count(*) FROM PreListe " +
                             "WHERE PL_nom_visiteur = @nomVisiteur AND PL_prenom_visiteur = @prenVisiteur AND PL_email_visiteur = @emailVisiteur;";

            try
            {
                //Ouverture de la connection
                connection.Open();
                //Passage par paramêtre des filtres WHERE à la requête
                cmd.Parameters.AddWithValue("@nomVisiteur", sNomVisiteur);
                cmd.Parameters.AddWithValue("@prenVisiteur", sPrenVisiteur);
                cmd.Parameters.AddWithValue("@emailVisiteur", sEmailVisiteur);

                int result = (int)cmd.ExecuteScalar();
                //Si le résultat est > à 0 on retourne vrai 
                if (result > 0)
                {
                    bRet = true;
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
                //Affichage du message d'erreur 
                throw new ApplicationException(ex.Message);
            }
            finally
            {
                //Destruction des objets cmd et connection
                cmd.Dispose();
                connection.Close();
            }
            return bRet;
        }

        private bool recherchePatient(string sNomPatient, string sPrenPatient, string sNumVisite)
        {
            bool bRet = false;

            SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString);

            //Ouverture d'une connection à la base de données
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT nom_patient, prenom_patient, num_visite " +
                "FROM LocalisationPatient WHERE nom_patient LIKE @nomPatient AND prenom_patient LIKE @prenPatient AND num_visite LIKE @numVisite;";

            try
            {
                //Ouverture de la connection
                connection.Open();
                //Passage par paramêtre des filtres WHERE à la requête
                cmd.Parameters.AddWithValue("@nomPatient", sNomPatient);
                cmd.Parameters.AddWithValue("@prenPatient", sPrenPatient);
                cmd.Parameters.AddWithValue("@numVisite", sNumVisite);

                SqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                if (reader.HasRows)
                {
                    string sNom = reader.GetString(0);
                    string sPren = reader.GetString(1);
                    string sNVisite = reader.GetString(2);

                    if (sNom.ToLower() == sNomPatient.ToLower() && sNVisite == sNumVisite)
                    {
                        bRet = true;
                    }
                }

            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
                //Affichage du message d'erreur 
                throw new ApplicationException(ex.Message);
            }
            finally
            {
                //Destruction des objets cmd et connection
                cmd.Dispose();
                connection.Close();
            }
            return bRet;

        }

        private TimeSpan[,] getSchedule(string sNomPatient, string sPrenPatient)
        {
            SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString);

            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT Count(*) FROM schedule WHERE nom_patient = @nomPatient " +
                                   "AND prenom_patient = @prenPatient AND date_examen = @dateVisite;";

            try
            {
                //Ouverture de la connection
                connection.Open();
                //Passage par paramêtre des filtres WHERE à la requête
                cmd.Parameters.AddWithValue("@nomPatient", sNomPatient);
                cmd.Parameters.AddWithValue("@prenPatient", sPrenPatient);
                cmd.Parameters.AddWithValue("@dateVisite", DateTime.Today);

                SqlDataReader reader = cmd.ExecuteReader();

                reader.Read();

                int nbRow = reader.GetInt32(0);

                reader.Close();

                TimeSpan[,] plageHoraire = new TimeSpan[nbRow, 2];

                SqlCommand cmd2 = connection.CreateCommand();
                cmd2.CommandText = "SELECT heure_debut_examen, heure_fin_examen FROM schedule WHERE nom_patient = @nomPatient " +
                                    "AND prenom_patient = @prenPatient AND date_examen = @dateVisite ORDER BY heure_debut_examen;";

                cmd2.Parameters.AddWithValue("@nomPatient", sNomPatient);
                cmd2.Parameters.AddWithValue("@prenPatient", sPrenPatient);
                cmd2.Parameters.AddWithValue("@dateVisite", DateTime.Today);

                reader = cmd2.ExecuteReader();

                int i = 0;
                while (reader.Read())
                {
                    plageHoraire[i, 0] = reader.GetTimeSpan(0);
                    plageHoraire[i, 1] = reader.GetTimeSpan(1);
                    i++;
                }

                return plageHoraire;

            }
            catch (Exception ex)
            {

                throw new ApplicationException(ex.Message);
            }
            finally
            {
                cmd.Dispose();
                connection.Close();
            }

        }

        private TimeSpan[,] getDispo(TimeSpan[,] time)
        {
            TimeSpan horaireDebutVisite = TimeSpan.FromHours(8);
            TimeSpan horaireFinVisite = TimeSpan.FromHours(22);

            int nbPlageInDispo = time.GetLength(0);
            int nbPlageDispo = 0;
            bool horaireLimite = false;
            bool horaireLimiteDown = false;
            bool horaireLimiteUp = false;
            bool horaireLimiteUpDown = false;

            #region calcule plage dispo

            if (nbPlageInDispo == 1)
            {
                if (time[0, 0] == horaireDebutVisite && time[0, 1] != horaireFinVisite || time[0, 0] != horaireDebutVisite && time[0, 1] == horaireFinVisite)
                {
                    nbPlageDispo = 1;
                    horaireLimite = true;
                    if (time[0, 0] == horaireDebutVisite && time[0, 1] != horaireFinVisite)
                    {
                        horaireLimiteDown = true;
                    }
                    else if (time[0, 0] != horaireDebutVisite && time[0, 1] == horaireFinVisite)
                    {
                        horaireLimiteUp = true;
                    }
                }
                else
                {
                    nbPlageDispo = nbPlageInDispo + 1;
                }
            }
            else if (nbPlageInDispo > 1)
            {
                if (time[0, 0] == horaireDebutVisite && time[time.GetUpperBound(0), 1] != horaireFinVisite)
                {
                    nbPlageDispo = nbPlageInDispo;
                    horaireLimite = true;
                    horaireLimiteDown = true;
                }
                else if (time[0, 0] != horaireDebutVisite && time[time.GetUpperBound(0), 1] == horaireFinVisite)
                {
                    nbPlageDispo = nbPlageInDispo;
                    horaireLimite = true;
                    horaireLimiteUp = true;
                }
                else if (time[0, 0] == horaireDebutVisite && time[time.GetUpperBound(0), 1] == horaireFinVisite)
                {
                    nbPlageDispo = nbPlageInDispo - 1;
                    horaireLimite = true;
                    horaireLimiteUpDown = true;
                }
                else
                {
                    nbPlageDispo = nbPlageInDispo + 1;
                }
            }

            #endregion

            TimeSpan[,] plageDispo = new TimeSpan[nbPlageDispo, 2];

            if (nbPlageDispo == 1)
            {
                for (int i = 0; i < nbPlageDispo; i++)
                {
                    if (horaireLimite)
                    {
                        if (horaireLimiteDown)
                        {
                            plageDispo[i, 0] = time[i, 1];
                            plageDispo[i, 1] = horaireFinVisite;
                        }
                        else if (horaireLimiteUp)
                        {
                            plageDispo[i, 0] = horaireDebutVisite;
                            plageDispo[i, 1] = time[i, 0];
                        }
                    }
                }

            }
            else if (nbPlageDispo > 1)
            {
                for (int i = 0; i < nbPlageDispo; i++)
                {
                    if (horaireLimite)
                    {
                        if (horaireLimiteDown)
                        {
                            if (i == 0)
                            {
                                plageDispo[i, 0] = time[i, 1];
                                plageDispo[i, 1] = time[(i + 1), 0];
                            }
                            else
                            {
                                plageDispo[i, 0] = time[i, 1];
                                plageDispo[i, 1] = horaireFinVisite;
                            }
                        }
                        else if (horaireLimiteUp)
                        {
                            if (i == 0)
                            {
                                plageDispo[i, 0] = horaireDebutVisite;
                                plageDispo[i, 1] = time[i, 0];
                            }
                            else
                            {
                                plageDispo[i, 0] = time[(i - 1), 1];
                                plageDispo[i, 1] = time[i, 0];
                            }
                        }
                        else if (horaireLimiteUpDown)
                        {
                            plageDispo[i, 0] = time[i, 1];
                            plageDispo[i, 1] = time[(i + 1), 0];
                        }
                    }
                    else
                    {
                        if (i == 0)
                        {
                            plageDispo[i, 0] = horaireDebutVisite;
                            plageDispo[i, 1] = time[i, 0];
                        }
                        else
                        {
                            plageDispo[i, 0] = time[(i - 1), 1];
                            if ((i + 1) == nbPlageDispo)
                            {
                                plageDispo[i, 1] = horaireFinVisite;
                            }
                            else
                            {
                                plageDispo[i, 1] = time[i, 0];
                            }
                        }
                    }
                }
            }
            return plageDispo;
        }

        private int getAffluence(TimeSpan time1, TimeSpan time2)
        {
            SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString);

            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT COUNT(id_visiteur) FROM Visiter WHERE date_visite = @dateVisite " +
                                "AND status_demande = @statusDemande AND heure_deb_visite >= @heureDebut " +
                                    "AND heure_fin_visite <= @heureFin ;";
            try
            {
                //Ouverture de la connection
                connection.Open();
                //Passage par paramêtre des filtres WHERE à la requête
                cmd.Parameters.AddWithValue("@dateVisite", DateTime.Today);
                cmd.Parameters.AddWithValue("@statusDemande", 1);
                cmd.Parameters.AddWithValue("@heureDebut", time1);
                cmd.Parameters.AddWithValue("@heureFin", time2);

                int affluence = 0;
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        affluence = reader.GetInt32(0);
                    }
                    reader.Close();
                }
                return affluence;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
            finally
            {
                cmd.Dispose();
                connection.Close();
            }

        }
        private void displayLabel(TimeSpan[,] horaire)
        {
            for (int i = 0; i < horaire.GetLength(0); i++)
            {
                Panel myPanel = new Panel();
                myPanel.ID = "panelPlageHoraire" + i;
                myPanel.CssClass = "form-group";
                myPanel.Style.Value = "margin-left:80px";

                Label myLabel1 = new Label();
                myLabel1.ID = "labelPlageHoraire" + i;
                myLabel1.CssClass = "col-md-3 control-label";
                myLabel1.Text = horaire[i, 0].ToString() + " - " + horaire[i, 1].ToString();

                Label myLabel2 = new Label();
                myLabel2.ID = "labelAffluence" + i;
                myLabel2.CssClass = "col-md-3 control-label";
                int affluence = getAffluence(horaire[i, 0], horaire[i, 1]);
                string etat = "";
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
                myLabel2.Text = etat;

                RadioButton myRadiobutton = new RadioButton();
                myRadiobutton.ID = "radioBtnHoraire" + i;
                myRadiobutton.CssClass = "col-md-3 control-label";

                myPanel.Controls.Add(myLabel1);
                myPanel.Controls.Add(myLabel2);
                myPanel.Controls.Add(myRadiobutton);

                divEtapeHoraire.Controls.Add(myPanel);

            }
        }
    }
}