using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace HIA_client_leger
{
    public partial class historiqueVisite : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HtmlGenericControl liItem = (HtmlGenericControl)Master.FindControl("historiqueVisite");
            liItem.Attributes.Add("class", "active");
        }

        protected void btnConfirmerEmailHistoVisite_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "initTable();", true);
            this.rowHistoriqueVisite.Visible = true;
        }
    }
}