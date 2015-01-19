<%@ Page Title="" Language="C#" MasterPageFile="~/HIA_client_leger.Master" AutoEventWireup="true" CodeBehind="demandeDeVisite.aspx.cs" Inherits="HIA_client_leger.demandeDeVisite" %>

<asp:Content ID="Content1" ContentPlaceHolderID="StyleSection" runat="server">
    <link href="Content/demande-visite-custom.css" rel="stylesheet" />
    <link href="Content/font-awesome.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentSection" runat="server">
    <form runat="server">
        <div class="row">
            <div class="row step col-md-12 text-center" style="margin-left: 270px;">
                <div id="div1" class="col-md-2 activestep" style="height: 110px;" onclick="javascript: resetActive(event, 0, 'step-1');">
                    <span class="fa fa-users"></span>
                    <p>Informations du patient</p>
                </div>
                <div class="col-md-2" style="height: 110px;" onclick="javascript: resetActive(event, 20, 'step-2');">
                    <span class="fa fa-user"></span>
                    <p>Informations personnelles</p>
                </div>
                <div class="col-md-2" style="height: 110px;" onclick="javascript: resetActive(event, 40, 'step-3');">
                    <span class="fa fa-calendar"></span>
                    <p>Finalisation de la demande de visite</p>
                </div>
            </div>
        </div>
        <div class="row setup-content step activeStepInfo" id="step-1">
            <div class="col-xs-12">
                <div class="col-md-12 well text-center">
                    <h1>STEP 1</h1>
                </div>
            </div>
        </div>
        <div class="row setup-content step hiddenStepInfo" id="step-2">
            <div class="col-xs-12">
                <div class="col-md-12 well text-center">
                    <h1>STEP 2</h1>
                </div>
            </div>
        </div>
        <div class="row setup-content step hiddenStepInfo" id="step-3">
            <div class="col-xs-12">
                <div class="col-md-12 well text-center">
                    <h1>STEP 3</h1>
                </div>
            </div>
        </div>
    </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptSection" runat="server">
    <script src="Scripts/demande-de-visite.js">
</script>
</asp:Content>
