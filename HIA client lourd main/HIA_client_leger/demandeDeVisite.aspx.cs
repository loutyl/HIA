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
            txtBoxEtagePatient.Attributes.Add("placeholder", "Numéro de l'étage");
            txtBoxChambrePatient.Attributes.Add("placeholder", "Numéro de la chambre");
            txtBoxLitPatient.Attributes.Add("placeholder", "Localisation du lit");
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
            bool bPatientMatch = recherchePatient(txtBoxNomPatient.Text, txtBoxPrenPatient.Text, txtBoxCodePatient.Text);

            if (bPatientMatch)
            {
                panelEtape1.Visible = false;
                string sClass = divBarEtape1.Attributes["class"].Replace("activestep", "");
                divBarEtape1.Attributes["class"] = sClass;

                divBarEtape2.Attributes["class"] += " activestep";
                panelEtape2.Visible = true;

            }
            else
            {
                panelEtape1.Visible = false;
                panelEtapeInfoPatientError.Visible = true;

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
                        panelEtape2.Visible = false;
                        string sClass = divBarEtape2.Attributes["class"].Replace("activestep", "");
                        divBarEtape2.Attributes["class"] = sClass;

                        divBarEtape3.Attributes["class"] += " activestep";

                        panelEtape3.Visible = true;
                    }
                    else
                    {
                        panelEtape2.Visible = false;
                        panelEtapeInfoVisiteurError.Visible = true;
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
    }
}