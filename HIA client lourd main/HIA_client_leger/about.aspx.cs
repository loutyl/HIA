using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace HIA_client_leger
{
    public partial class About : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HtmlGenericControl liItem = (HtmlGenericControl)Master.FindControl("aboutID");
            liItem.Attributes.Add("class", "active");
        }
    }
}