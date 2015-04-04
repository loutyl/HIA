<%@ Page Title="" Language="C#" MasterPageFile="~/HIA_client_leger.Master" AutoEventWireup="true" CodeBehind="demandeDeVisite.aspx.cs" Inherits="HIA_client_leger.DemandeDeVisite" %>

<asp:Content ID="Content1" ContentPlaceHolderID="StyleSection" runat="server">
    <link href="Content/demande-visite-custom.css" rel="stylesheet" type="text/css" />
    <link href="Content/font-awesome.css" rel="stylesheet" />
    <link href="Content/bootstrap-clockpicker.css" rel="stylesheet" />
    <link href="Content/uikit.css" rel="stylesheet" />
    <link href="Content/notify.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentSection" runat="server">
    <form runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <!--<asp:Panel ID="panelBarEtape" runat="server">
                    <div class="row">
                        <div class="row" style="margin-left: 375px;">
                            <div class="step text-center">
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
                                    <p>Planification</p>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>-->
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

                            <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                                <div class="modal-dialog">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                            <h4 class="modal-title text-center" id="myModalLabel">Plage horaire</h4>
                                            <asp:Label ID="myModalSubTitle" CssClass="modal-title text-center" runat="server" Text="Label"></asp:Label>
                                        </div>

                                        <asp:Panel ID="panelModalBody" CssClass="modal-body" runat="server" Height="220">
                                            <asp:Label ID="labelHeureDebVisite" runat="server" Text="Heure de début de la visite :"></asp:Label>
                                            <br />

                                            <div class="input-group clockpicker" style="margin-top: 10px;" data-autoclose="true">

                                                <input type="text" name="heureDebutVisite" id="inputTimeMin" value="<%= InputValueDebutVisite %>" class="form-control">
                                                <span class="input-group-addon" id="btnInputTimeMin">
                                                    <span class="glyphicon glyphicon-time"></span>
                                                </span>
                                            </div>
                                            <br />

                                            <asp:Label ID="labelHeureFinVisite" runat="server" Text="Heure de fin de la visite :"></asp:Label>
                                            <br />
                                            <div class="input-group clockpicker" style="margin-top: 10px;" data-autoclose="true">
                                                <input type="text" name="heureFinVisite" id="inputTimeMax" class="form-control" value="<%= InputValueFinVisite %>">
                                                <span class="input-group-addon" id="btnInputTimeMax">
                                                    <span class="glyphicon glyphicon-time"></span>
                                                </span>
                                            </div>

                                            <!--<div id="divErreurHoraire" class="row" style="margin-bottom: 20px; margin-top: 20px; color: red;" runat="server" visible="false">
                                                L'heure spécifiée ne respecte pas la plage horaire
                                            </div>-->

                                        </asp:Panel>

                                        <div class="modal-footer">
                                            <asp:Button ID="btnConfirmerHeureModal" CssClass="btn btn-primary" runat="server" Text="Confirmer" OnClick="btnConfirmerHeureModal_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <asp:Panel ID="panelEtape1" runat="server">
                                <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true">
                                    <div class="panel panel-default">
                                        <div class="panel-heading" role="tab" id="headingOne">
                                            <h4 class="panel-title">
                                                <a data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="false" aria-controls="collapseOne">Option 1 : Authentification
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
                                                            <asp:Button ID="btnConfirmerInfoVisiteur" CssClass="btn btn-primary" runat="server" Text="Confirmer" OnClick="btnConfirmerInfoVisiteur_Click" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="panel panel-default">
                                        <div class="panel-heading" role="tab" id="headingTwo">
                                            <h4 class="panel-title">
                                                <a class="collapsed" data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" aria-expanded="true" aria-controls="collapseTwo">Option 2 : Demande d'autorisation de visite
                                                </a>
                                            </h4>
                                        </div>
                                        <div id="collapseTwo" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingTwo">
                                            <div class="panel-body">
                                                <h4 style="margin-bottom: 30px;">Informations personnelles</h4>
                                                <div class="form-horizontal" id="divEtape2DemandeAutorisation" style="margin-left: -130px; display: inline-block;" runat="server">
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
                                                            <asp:Button ID="btnConfirmerInfoVisiteurAuth" CssClass="btn btn-primary" runat="server" Text="Confirmer" OnClick="btnConfirmerInfoVisiteurAuth_Click" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="panelEtape2" runat="server" Visible="false">
                                <h4 id="InfoPatientTitle">Informations du patient</h4>
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
                                            <asp:Button ID="btnConfirmerInfoPatient" CssClass="btn btn-primary" runat="server" Text="Confirmer" OnClick="btnConfirmerInfoPatient_Click" />
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="panelEtape3" runat="server" Visible="false">
                                <h4 id="plageHoraireTitre">Plage horaire disponible</h4>
                                <asp:Panel ID="divEtapeHoraire" CssClass="form-horizontal" runat="server">
                                    <div class="form-group" style="margin-left: 50px">
                                        <asp:Label ID="labelInfoPlageHoraire" CssClass="col-md-3 control-label" runat="server" Text="Plage horaire"></asp:Label>
                                        <asp:Label ID="labelInfoAffluence" CssClass="col-md-3 control-label" runat="server" Text="Affluence"></asp:Label>
                                        <asp:Label ID="labelInfoChoixHoraire" CssClass="col-md-3 control-label" runat="server" Text="Choix horaire"></asp:Label>
                                    </div>
                                </asp:Panel>
                                <h4 id="plageHoraireIndispoTitre" style="margin-top: 10px;" visible="false" runat="server">Plage horaire indisponible</h4>
                                <asp:Panel ID="divEtapeHoraireIndisponible" CssClass="form-horizontal" runat="server">
                                </asp:Panel>
                                <div class="row text-center">
                                    <asp:Button ID="btnConfirmerPlageHoraire" CssClass="btn btn-primary" runat="server" Text="Choisir" OnClick="btnConfirmerPlageHoraire_Click" />
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="panelEtapeNotificationEnvoiAutorisation" runat="server" Visible="false">
                                <h6>Votre demande d'autorisation de visite à bien été prise en compte, un email vous sera envoyé sous peu.
                                Cliquez <a href="demandeDeVisite.aspx">ici</a> pour être redirigé.
                                </h6>
                            </asp:Panel>
                            <asp:Panel ID="panelInfoDemandeDeVisiteFinal" runat="server" Visible="false">
                                <h6>Votre demande de visite a bien été prise en compte vous recevrez un email sous peu avec votre bon de visite
                                Cliquez <a href="Acceuil.aspx">ici</a> pour retourner à l'accueil.
                                </h6>
                            </asp:Panel>
                            <asp:Panel ID="panelInfoDemandeDeVisiteBesoinConfirmation" runat="server" Visible="false">
                                <h6>Votre demande de visite a bien été prise en compte, elle requiert la confirmation d'un chef de service, vous recevrez
                                un email sous peu vous informant de l'état de votre demande. Cliquez <a href="Acceuil.aspx">ici</a> pour retourner à l'accueil.
                                </h6>
                            </asp:Panel>
                            <asp:Panel ID="panelPatientStatusBloquer" runat="server" Visible="false">
                                <h6>Les visites de ce patient sont actuellement bloquées, veuillez réessayer ultérieurement. Cliquez <a href="Acceuil.aspx">ici</a> pour retourner à l'accueil.
                                </h6>
                            </asp:Panel>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnConfirmerInfoPatient" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnConfirmerInfoVisiteur" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnConfirmerPlageHoraire" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnConfirmerInfoVisiteurAuth" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnConfirmerHeureModal" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptSection" runat="server">
    <script type="text/javascript" src="Scripts/demande-de-visite.js">
    </script>
    <script src="Scripts/bootstrap-clockpicker.js"></script>
    <script type="text/javascript">

        function openModal() {
            $('#myModal').modal({ show: true });
        }
    </script>
    <script type="text/javascript">
        function openTimeMin() {

            var inputMin = $('#inputTimeMin').clockpicker({
                autoclose: true

            });

            $('#btnInputTimeMin').click(function (e) {

                e.stopPropagation();
                inputMin.clockpicker('show')
                        .clockpicker('toggleView', 'minutes');
            });

        }
    </script>
    <script type="text/javascript">
        function openTimeMax() {

            var inputMax = $('#inputTimeMax').clockpicker({
                autoclose: true

            });

            $('#btnInputTimeMax').click(function (e) {

                e.stopPropagation();
                inputMax.clockpicker('show')
                        .clockpicker('toggleView', 'minutes');
            });
        }
    </script>
    <script src="Scripts/uikit.js"></script>
    <script src="Scripts/notify.js"></script>
    <script type="text/javascript">
        function notificationError(errorType) {

            var error = errorType;
            switch (error) {

                case 1: UIkit.notify('Les informations entrées sont incorrectes.', { status: 'danger' });
                    break;
                case 2: UIkit.notify('L&#39email renseigné n&#39est pas valide.', { status: 'danger' });
                    break;
                case 3: UIkit.notify('Vous n&#39êtes pas autorisé à effectuer une demande de visite pour ce patient, veuillez ressayer ultérieurement.', { status: 'danger' });
                    break;
                case 4: UIkit.notify('Aucun patient n&#39a été trouvé, veuillez vérifiez vos informations.', { status: 'danger' });
                    break;
                case 5: UIkit.notify('Veuillez renseigner tous les champs requis', { status: 'danger' });
                    break;
            }
        }
    </script>
    
    
</asp:Content>
