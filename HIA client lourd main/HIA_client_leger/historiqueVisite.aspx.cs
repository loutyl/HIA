﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using databaseHIA;
using Utilities;

namespace HIA_client_leger
{
    public partial class historiqueVisite : System.Web.UI.Page
    {

        private string _databaseConnectionString = WebConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString;

        enum errorType
        {
            emailAddressNotValid = 1,
            noResultFound = 2,
            fieldEmpty = 3
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            HtmlGenericControl liItem = (HtmlGenericControl)Master.FindControl("historiqueVisite");
            liItem.Attributes.Add("class", "active");

            this.txtBoxEmailVisiteurHisto.Attributes.Add("placeholder", "Votre adresse e-mail");
            this.txtBoxNomVisiteurHisto.Attributes.Add("placeholder", "Votre nom");
        }

        protected void btnConfirmerEmailHistoVisite_Click(object sender, EventArgs e)
        {

            lightClientDatabaseObject lDb = new lightClientDatabaseObject(this._databaseConnectionString);

            UtilitiesTool.stringUtilities stringTool = new UtilitiesTool.stringUtilities();

            List<List<string>> historiqueVisite = new List<List<string>>();

            if (!String.IsNullOrWhiteSpace(txtBoxNomVisiteurHisto.Text) || !String.IsNullOrWhiteSpace(txtBoxEmailVisiteurHisto.Text))
            {
                if (stringTool.isValidEmail(this.txtBoxEmailVisiteurHisto.Text))
                {
                    historiqueVisite = lDb.getHistorique(lDb.getVisiteurId(this.txtBoxNomVisiteurHisto.Text, this.txtBoxEmailVisiteurHisto.Text));

                    if (historiqueVisite.Count >= 1)
                    {
                        foreach (List<string> dataHistorique in historiqueVisite)
                        {
                            TableRow row = new TableRow();

                            foreach (string data in dataHistorique)
                            {
                                TableCell cell = new TableCell();

                                if (!String.IsNullOrEmpty(data))
                                    cell.Text = data;
                                else
                                    cell.Text = "Aucun numéro de visite";

                                row.Cells.Add(cell);
                            }

                            this.tableHistorique.Rows.Add(row);
                        }

                        this.rowHistoriqueVisite.Visible = true;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "notification", "notificationError(" + (int)errorType.noResultFound + ");", true);
                    }

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "notification", "notificationError(" + (int)errorType.emailAddressNotValid + ");", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "notification", "notificationError(" + (int)errorType.fieldEmpty + ");", true);
            }
        }
    }
}