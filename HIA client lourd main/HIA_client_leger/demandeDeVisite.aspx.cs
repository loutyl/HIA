using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace HIA_client_leger
{
    public partial class demandeDeVisite : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            initPageControl();
           
            
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
        }
    
    }
}