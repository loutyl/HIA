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
            
        }
    
    }
}