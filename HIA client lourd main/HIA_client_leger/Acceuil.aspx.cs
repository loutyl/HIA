using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace HIA_client_leger
{
    public partial class Acceuil : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HtmlGenericControl liItem = (HtmlGenericControl)Master.FindControl("acceuil");
            liItem.Attributes.Add("class", "active");
        }
    }
}