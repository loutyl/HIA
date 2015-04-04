<%@ Page Title="" Language="C#" MasterPageFile="~/HIA_client_leger.Master" AutoEventWireup="true" CodeBehind="Acceuil.aspx.cs" Inherits="HIA_client_leger.Acceuil" %>

<asp:Content ID="Content1" ContentPlaceHolderID="StyleSection" runat="server">
    <link href="Content/acceuil-custom.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentSection" runat="server">



    <div class="page-header uk-width-medium-1-1">
        <h2 class="text-center">Bienvenue sur le portail H.I.D.I.V</h2>
    </div>

    <div class="uk-width-medium-1-1">
        <h3 class="uk-text-center uk-text-bold">Présentation</h3>
        <p class="uk-text-center">H.I.D.I.V ou Hôpital Innovation Demande Informatisé de Visite est un nouveau service mis en place en collaboration avec l'hôpital H.I.A pour faciliter la gestion des visites des patients tant pour le personnel que pour les visiteurs extèrieure.</p>
    </div>
    <div class="tm-section">
        <div class="uk-container uk-container-center uk-text-center" style="margin-top: 40px">
            <h3 class="uk-heading-medium uk-text-bold">Fonctionnalité</h3>
            <p class="uk-text-medium">Très simple d'utilisation, ce service vous permet de très rapidement effectuer une demande de visite sur des plages horaire que vous avez choisis. </p>
            <div class="uk-grid">
                <div class="uk-width-medium-1-4 uk-hidden-medium">
                </div>
                <div class="uk-width-medium-1-4">
                    <div class="uk-panel" style="padding-right: 65px;">
                        <img src="fonts/calendar128.png" />
                        <h2 class="uk-margin-top">Des demandes</h2>
                        <p>
                            Facile d'utilisation, 3 cliques suffisent pour effectuer une demande de visite
                        </p>
                    </div>
                </div>
                <div class="uk-width-medium-1-4">
                    <div class="uk-panel" style="padding-left: 80px;">
                        <img src="fonts/tasks128.png" />
                        <h2 class="uk-margin-top">Un historique</h2>
                        <p>
                            Retrouvez facilement vos visite précédentes grâce à votre historique.
                        </p>
                    </div>
                </div>
                <div class="uk-width-medium-1-4 uk-hidden-medium">
                </div>
            </div>
        </div>
    </div>
    <div class="tm-section">
        <div class="uk-container uk-container-center uk-text-center" style="margin-top: 40px">
            <h3 class="uk-heading-medium uk-text-bold">Les etapes</h3>
            <p class="uk-text-medium">Très simple d'utilisation, ce service vous permet de très rapidement effectuer une demande de visite sur des plages horaire que vous choisisez. </p>

            <div class="uk-grid">
                
                <div class="uk-width-medium-1-3">
                    <div class="uk-panel">
                        <img src="fonts/locked128.png" />
                        <h2 class="uk-margin-top">Authentifiez-vous</h2>
                        <p>
                            Pour la sécurité des patients, se service recquiert une authentification.
                        </p>
                    </div>
                </div>
                <div class="uk-width-medium-1-3">
                    <div class="uk-panel" >
                        <img src="fonts/user128.png" />
                        <h2 class="uk-margin-top">Choisisez un patient</h2>
                        <p>
                            Retrouvez le patient que vous souhaitez visiter grâce à son code unique de visite.
                        </p>
                    </div>
                </div>
                <div class="uk-width-medium-1-3">
                    <div class="uk-panel">
                        <img src="fonts/calendarClock128.png" />
                        <h2 class="uk-margin-top">Choisisez votre plage horaire</h2>
                        <p>
                            Les plages horaires sont calculées pour garantir une disponibilité maximale des patients.
                        </p>
                    </div>
                </div>
                
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptSection" runat="server">
</asp:Content>
