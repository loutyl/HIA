namespace HIA_client_leger
{
    public partial class Acceuil : System.Web.UI.Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            System.Web.UI.HtmlControls.HtmlGenericControl liItem = (System.Web.UI.HtmlControls.HtmlGenericControl)Master.FindControl("acceuil");
            liItem.Attributes.Add("class", "active");
        }
    }
}