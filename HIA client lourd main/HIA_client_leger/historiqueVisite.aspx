<%@ Page Title="" Language="C#" MasterPageFile="~/HIA_client_leger.Master" AutoEventWireup="true" CodeBehind="historiqueVisite.aspx.cs" Inherits="HIA_client_leger.HistoriqueVisite" %>

<asp:Content ID="Content1" ContentPlaceHolderID="StyleSection" runat="server">
    <link href="Content/demande-visite-custom.css" rel="stylesheet" type="text/css" />
    <link href="Content/uikit.css" rel="stylesheet" />
    <link href="Content/notify.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentSection" runat="server">
    <form runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="form-inline">
                    <div class="form-group">
                        <asp:TextBox ID="txtBoxNomVisiteurHisto" CssClass="form-control input-size" runat="server"></asp:TextBox>
                        <asp:TextBox ID="txtBoxEmailVisiteurHisto" CssClass="form-control input-size" runat="server"></asp:TextBox>
                    </div>
                    <asp:Button ID="btnConfirmerEmailHistoVisite" CssClass="btn btn-primary" runat="server" Text="Confirmer" OnClick="btnConfirmerEmailHistoVisite_Click" />
                </div>

                <div id="rowHistoriqueVisite" class="row" style="padding-top: 25px;" visible="false" runat="server">

                    <asp:Table ClientIDMode="static" ID="tableHistorique" CssClass="table table-hover text-center" runat="server">
                        <asp:TableHeaderRow CssClass="text-center">
                            <asp:TableCell><strong>Date de Visite</strong></asp:TableCell>
                            <asp:TableCell><strong>Heure de début</strong></asp:TableCell>
                            <asp:TableCell><strong>Heure de fin</strong></asp:TableCell>
                            <asp:TableCell><strong>Numéro de visite</strong></asp:TableCell>
                        </asp:TableHeaderRow>

                    </asp:Table>
                </div>

            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnConfirmerEmailHistoVisite" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptSection" runat="server">
    <script src="Scripts/jquery-2.1.1.js"></script>
    <script src="Scripts/uikit.js"></script>
    <script src="Scripts/notify.js"></script>
    <script type="text/javascript">
        function notificationError(errorType) {

            var error = errorType;
            switch (error) {

                case 1: UIkit.notify('L&#39addresse e-mail renseignée est incorrecte.', { status: 'danger' });
                    break;
                case 2: UIkit.notify('Aucun résultat n&#39a été trouvé', { status: 'danger' });
                    break;
                case 3: UIkit.notify('Veuillez entrer votre nom et votre adresse e-mail.', { status: 'danger' });
                    break;
            }
        }
    </script>
</asp:Content>
