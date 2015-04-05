namespace HIA_client_leger
{
    public partial class Contact : System.Web.UI.Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            System.Web.UI.HtmlControls.HtmlGenericControl liItem = (System.Web.UI.HtmlControls.HtmlGenericControl)Master.FindControl("contactID");
            liItem.Attributes.Add("class", "active");
        }


    }
}