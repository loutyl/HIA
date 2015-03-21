<%@ Page Title="" Language="C#" MasterPageFile="~/HIA_client_leger.Master" AutoEventWireup="true" CodeBehind="historiqueVisite.aspx.cs" Inherits="HIA_client_leger.historiqueVisite" %>

<asp:Content ID="Content1" ContentPlaceHolderID="StyleSection" runat="server">
    <link href="Content/jquery.dataTables.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentSection" runat="server">
    <form runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="form-inline">
                    <div class="form-group">
                        <label for="inputEmailVisiteurHistoVisite">Adresse E-mail</label>
                        <asp:TextBox ID="txtEmailVisiteurHisto" CssClass="form-control input-size" runat="server"></asp:TextBox>
                    </div>
                    <asp:Button ID="btnConfirmerEmailHistoVisite" CssClass="btn btn-primary" runat="server" Text="Confirmer" OnClick="btnConfirmerEmailHistoVisite_Click" />
                </div>

                <div id="rowHistoriqueVisite" class="row" style="padding-top: 25px;" Visible="false" runat="server">
                    <table id="example" class="display" cellspacing="0" width="100%">
                        <thead>
                            <tr>
                                <th>Name</th>
                                <th>Position</th>
                                <th>Office</th>
                                <th>Age</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>Tiger Nixon</td>
                                <td>System Architect</td>
                                <td>Edinburgh</td>
                                <td>61</td>
                            </tr>
                            <tr>
                                <td>Garrett Winters</td>
                                <td>Accountant</td>
                                <td>Tokyo</td>
                                <td>63</td>
                            </tr>
                            <tr>
                                <td>Ashton Cox</td>
                                <td>Junior Technical Author</td>
                                <td>San Francisco</td>
                                <td>66</td>
                            </tr>

                        </tbody>
                    </table>

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
    <script src="Scripts/jquery.dataTables.js"></script>
    <script type="text/javascript">
        function initTable() {
            $(document).ready(function () {
                $('#example').DataTable();
            });
        }
    </script>
</asp:Content>
