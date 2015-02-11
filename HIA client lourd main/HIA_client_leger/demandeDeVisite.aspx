<%@ Page Title="" Language="C#" MasterPageFile="~/HIA_client_leger.Master" AutoEventWireup="true" CodeBehind="demandeDeVisite.aspx.cs" Inherits="HIA_client_leger.demandeDeVisite" %>

<asp:Content ID="Content1" ContentPlaceHolderID="StyleSection" runat="server">
    <link href="Content/demande-visite-custom.css" rel="stylesheet" />
    <link href="Content/font-awesome.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentSection" runat="server">
    <form runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Panel ID="panelBarEtape" runat="server">
                    <div class="row">
                        <div class="row step col-md-12 text-center" style="margin-left: 270px;">
                            <div id="divBarEtape1" class="col-md-2 activestep" style="height: 110px;" runat="server">
                                <span class="fa fa-user"></span>
                                <p>Authentification</p>
                            </div>
                            <div id="divBarEtape2" class="col-md-2" style="height: 110px;" runat="server">
                                <span class="fa fa-users"></span>
                                <p>Informations du patient</p>
                            </div>
                            <div id="divBarEtape3" class="col-md-2" style="height: 110px;" runat="server">
                                <span class="fa fa-calendar"></span>
                                <p>Finalisation de la demande de visite</p>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnConfirmerInfoPatient" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnConfirmerInfoVisiteur" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
        <div class="row setup-content step activeStepInfo" id="step-1">
            <div class="col-md-12">
                <div class="well text-center">
                    <asp:UpdatePanel ID="updatePanelEtape1" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="panelEtape1" runat="server">
                                <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true">
                                    <div class="panel panel-default">
                                        <div class="panel-heading" role="tab" id="headingOne">
                                            <h4 class="panel-title">
                                                <a data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="true" aria-controls="collapseOne">Option 1 : Authentification
                                                </a>
                                            </h4>
                                        </div>
                                        <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne">
                                            <div class="panel-body">
                                                <h4 style="margin-bottom: 30px;">Informations personnelles</h4>
                                                <div class="form-horizontal" id="divEtape2Form">
                                                    <div class="form-group">
                                                        <label for="inputNomVisiteur" class="col-md-4 control-label">Nom</label>
                                                        <div class="col-md-8">
                                                            <asp:TextBox ID="txtBoxNomVisiteur" CssClass="form-control input-size" runat="server"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label for="inputPrenVisiteur" class="col-md-4 control-label">Prénom</label>
                                                        <div class="col-md-8">
                                                            <asp:TextBox ID="txtBoxPrenVisiteur" CssClass="form-control input-size" runat="server"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label for="inputEmailVisiteur" class="col-md-4 control-label">Adresse e-mail</label>
                                                        <div class="col-md-8">
                                                            <asp:TextBox ID="txtBoxEmailVisiteur" CssClass="form-control input-size" runat="server"></asp:TextBox>
                                                        </div>
                                                    </div>                                                   
                                                    <div class="form-group">
                                                        <div class="col-md-8 col-md-offset-2">
                                                            <asp:Button ID="btnConfirmerInfoVisiteur" CssClass="btn btn-default" runat="server" Text="Confirmer" OnClick="btnConfirmerInfoVisiteur_Click" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="panel panel-default">
                                        <div class="panel-heading" role="tab" id="headingTwo">
                                            <h4 class="panel-title">
                                                <a class="collapsed" data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo">Option 2 : Demande d'autorisation de visite
                                                </a>
                                            </h4>
                                        </div>
                                        <div id="collapseTwo" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingTwo">
                                            <div class="panel-body">
                                                <h4 style="margin-bottom: 30px;">Informations personnelles</h4>
                                                <div class="form-horizontal" id="divEtape2DemandeAutorisation" style="margin-left:-130px; display: inline-block;" runat="server">
                                                    <div class="form-group">
                                                        <label for="inputNomVisiteurAuth" class="col-md-4 control-label">Nom</label>
                                                        <div class="col-md-8">
                                                            <asp:TextBox ID="txtBoxNomVisiteurAuth" CssClass="form-control input-size" runat="server"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label for="inputPrenVisiteurAuth" class="col-md-4 control-label">Prénom</label>
                                                        <div class="col-md-8">
                                                            <asp:TextBox ID="txtBoxPrenVisiteurAuth" CssClass="form-control input-size" runat="server"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label for="inputEmailVisiteurAuth" class="col-md-4 control-label">Adresse e-mail</label>
                                                        <div class="col-md-8">
                                                            <asp:TextBox ID="txtBoxEmailVisiteurAuth" CssClass="form-control input-size" runat="server"></asp:TextBox>
                                                        </div>
                                                    </div>                                                    
                                                    <h4 id="InfoPatientAuth" style="margin-bottom: 30px;">Information du patient</h4>
                                                    <div class="form-group">
                                                        <label for="inputNomPatientAuth" class="col-md-4 control-label">Nom du patient</label>
                                                        <div class="col-md-8">
                                                            <asp:TextBox ID="txtBoxNomPatientAuth" CssClass="form-control input-size" runat="server" AutoPostBack="False"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label for="inputPrenPatientAuth" class="col-md-4 control-label">Prénom du patient</label>
                                                        <div class="col-md-8">
                                                            <asp:TextBox ID="txtBoxPrenPatientAuth" CssClass="form-control input-size" runat="server" AutoPostBack="False"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label for="inputChambrePatientAuth" class="col-md-4 control-label">Chambre</label>
                                                        <div class="col-md-8">
                                                            <asp:TextBox ID="txtBoxChambrePatientAuth" CssClass="form-control input-size" runat="server" AutoPostBack="False"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <div class="col-md-8 col-md-offset-2">
                                                            <asp:Button ID="btnConfirmerInfoVisiteurAuth" CssClass="btn btn-default" runat="server" Text="Confirmer" OnClick="btnConfirmerInfoVisiteurAuth_Click" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="panelEtape2" runat="server" Visible="false">
                                <h4 id="InfoPatientTitre">Informations du patient</h4>
                                <div class="form-horizontal" id="divEtape1Form">
                                    <div class="form-group">
                                        <label for="inputNomPatient" class="col-md-4 control-label">Nom du patient</label>
                                        <div class="col-md-8">
                                            <asp:TextBox ID="txtBoxNomPatient" CssClass="form-control input-size" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label for="inputPrenPatient" class="col-md-4 control-label">Prénom du patient</label>
                                        <div class="col-md-8">
                                            <asp:TextBox ID="txtBoxPrenPatient" CssClass="form-control input-size" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label for="inputCodePatient" class="col-md-4 control-label">Code de visite du patient</label>
                                        <div class="col-md-8">
                                            <asp:TextBox ID="txtBoxCodePatient" CssClass="form-control input-size" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-7 col-md-offset-2">
                                            <asp:Button ID="btnConfirmerInfoPatient" CssClass="btn btn-default" runat="server" Text="Confirmer" OnClick="btnConfirmerInfoPatient_Click" />
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="panelEtape3" runat="server" Visible="false">
                                <h4 id="plageHoraireTitre">Plage horaire disponnible</h4>
                                <div class="form-horizontal" id="divEtapeHoraire" runat="server">
                                                                        
                                </div>
                                <div class="row text-center">
                                    <asp:Button ID="btnConfirmerPlageHoraire" CssClass="btn btn-default" runat="server" Text="Confirmer" OnClick="btnConfirmerPlageHoraire_Click" />
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="panelEtapeInfoPatientError" runat="server" Visible="false">
                                <h1>Infor patient error</h1>
                            </asp:Panel>
                            <asp:Panel ID="panelEtapeInfoVisiteurError" runat="server" Visible="false">
                                <h3>Info visiteur error</h3>
                            </asp:Panel>
                            <asp:Panel ID="panelEtapeNotificationEnvoiAutorisation" runat="server" Visible="false">
                                <h1>Votre demande d'autorisation de visite à bien été prise en compte, un email vous sera envoyé sous peu.
                                Cliquez <a href="demandeDeVisite.aspx">ici</a> pour être redirigé.
                                </h1>
                            </asp:Panel>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnConfirmerInfoPatient" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnConfirmerInfoVisiteur" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnConfirmerInfoVisiteurAuth" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnConfirmerPlageHoraire" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptSection" runat="server">
    <script src="Scripts/demande-de-visite.js">
</script>
</asp:Content>
