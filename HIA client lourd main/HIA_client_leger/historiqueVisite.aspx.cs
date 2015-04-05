namespace HIA_client_leger
{
    public partial class HistoriqueVisite : System.Web.UI.Page
    {

        private readonly string _databaseConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString;

        enum ErrorType
        {
            EmailAddressNotValid = 1,
            NoResultFound = 2,
            FieldEmpty = 3
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (Master != null)
            {
                System.Web.UI.HtmlControls.HtmlGenericControl liItem = (System.Web.UI.HtmlControls.HtmlGenericControl)Master.FindControl("historiqueVisite");
                liItem.Attributes.Add("class", "active");
            }

            txtBoxEmailVisiteurHisto.Attributes.Add("placeholder", "Votre adresse e-mail");
            txtBoxNomVisiteurHisto.Attributes.Add("placeholder", "Votre nom");
        }

        protected void btnConfirmerEmailHistoVisite_Click(object sender, System.EventArgs e)
        {

            databaseHIA.LightClientDatabaseObject lDb = new databaseHIA.LightClientDatabaseObject(_databaseConnectionString);

            Utilities.UtilitiesTool.StringUtilities stringTool = new Utilities.UtilitiesTool.StringUtilities();

            if (!System.String.IsNullOrWhiteSpace(txtBoxNomVisiteurHisto.Text) || !System.String.IsNullOrWhiteSpace(txtBoxEmailVisiteurHisto.Text))
            {
                if (stringTool.IsValidEmail(txtBoxEmailVisiteurHisto.Text))
                {
                    var historiqueVisite = lDb.GetHistorique(lDb.GetVisiteurId(txtBoxNomVisiteurHisto.Text, txtBoxEmailVisiteurHisto.Text));

                    if (historiqueVisite.Count >= 1)
                    {
                        foreach (System.Collections.Generic.List<string> dataHistorique in historiqueVisite)
                        {
                            System.Web.UI.WebControls.TableRow row = new System.Web.UI.WebControls.TableRow();

                            foreach (string data in dataHistorique)
                            {
                                System.Web.UI.WebControls.TableCell cell = new System.Web.UI.WebControls.TableCell
                                {
                                    Text = !System.String.IsNullOrEmpty(data)
                                    ? data
                                    : @"Aucun numéro de visite"
                                };

                                row.Cells.Add(cell);
                            }

                            tableHistorique.Rows.Add(row);
                        }

                        rowHistoriqueVisite.Visible = true;
                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "notification", "notificationError(" + (int)ErrorType.NoResultFound + ");", true);
                    }
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "notification", "notificationError(" + (int)ErrorType.EmailAddressNotValid + ");", true);
                }
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "notification", "notificationError(" + (int)ErrorType.FieldEmpty + ");", true);
            }
        }
    }
}