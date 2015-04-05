using System;
using System.Collections.Generic;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using databaseHIA;
using Utilities;

namespace HIA_client_leger
{
    public partial class HistoriqueVisite : Page
    {

        private readonly string _databaseConnectionString = WebConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString;

        enum ErrorType
        {
            EmailAddressNotValid = 1,
            NoResultFound = 2,
            FieldEmpty = 3
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Master != null)
            {
                HtmlGenericControl liItem = (HtmlGenericControl)Master.FindControl("historiqueVisite");
                liItem.Attributes.Add("class", "active");
            }

            txtBoxEmailVisiteurHisto.Attributes.Add("placeholder", "Votre adresse e-mail");
            txtBoxNomVisiteurHisto.Attributes.Add("placeholder", "Votre nom");
        }

        protected void btnConfirmerEmailHistoVisite_Click(object sender, EventArgs e)
        {

            LightClientDatabaseObject lDb = new LightClientDatabaseObject(_databaseConnectionString);

            UtilitiesTool.StringUtilities stringTool = new UtilitiesTool.StringUtilities();

            if (!String.IsNullOrWhiteSpace(txtBoxNomVisiteurHisto.Text) || !String.IsNullOrWhiteSpace(txtBoxEmailVisiteurHisto.Text))
            {
                if (stringTool.IsValidEmail(txtBoxEmailVisiteurHisto.Text))
                {
                    var historiqueVisite = lDb.GetHistorique(lDb.GetVisiteurId(txtBoxNomVisiteurHisto.Text, txtBoxEmailVisiteurHisto.Text));

                    if (historiqueVisite.Count >= 1)
                    {
                        foreach (List<string> dataHistorique in historiqueVisite)
                        {
                            TableRow row = new TableRow();

                            foreach (string data in dataHistorique)
                            {
                                TableCell cell = new TableCell
                                {
                                    Text = !String.IsNullOrEmpty(data)
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
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "notification", "notificationError(" + (int)ErrorType.NoResultFound + ");", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "notification", "notificationError(" + (int)ErrorType.EmailAddressNotValid + ");", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "notification", "notificationError(" + (int)ErrorType.FieldEmpty + ");", true);
            }
        }
    }
}