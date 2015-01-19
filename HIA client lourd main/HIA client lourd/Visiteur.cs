using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIA_client_lourd
{
    class Visiteur
    {
        //ID du visiteur
        private string idVisiteur;

        //Nom du visiteur
        private string nomVisiteur;
        //accesseur
        public string NomVisiteur
        {
            get { return nomVisiteur; }
            set { nomVisiteur = value; }
        }

        //Prenom du visiteur
        private string prenVisiteur;
        //accesseur
        public string PrenVisiteur
        {
            get { return prenVisiteur; }
            set { prenVisiteur = value; }
        }

        //Email du visiteur
        private string emailVisiteur;
        //accesseur
        public string EmailVisiteur
        {
            get { return emailVisiteur; }
            set { emailVisiteur = value; }
        }

        //Numéro de téléphone du visiteur
        private string telVisiteur;
        //accesseur
        public string TelVisiteur
        {
            get { return telVisiteur; }
            set { telVisiteur = value; }
        }

        //Numéro de visite du visiteur
        private string numVisiteVisiteur;
        //accesseur
        public string NumVisiteVisiteur
        {
            get { return numVisiteVisiteur; }
            set { numVisiteVisiteur = value; }
        }

        //Constructeur de visiteur
        public Visiteur(string id, string nom, string prenom, string email, string numVisite)
        {
            idVisiteur = id;
            nomVisiteur = nom;
            prenVisiteur = prenom;
            emailVisiteur = email;
            numVisiteVisiteur = numVisite;
        }

        public Visiteur(string id, string nom, string prenom, string email, string tel, string numVisite)
        {
            idVisiteur = id;
            nomVisiteur = nom;
            prenVisiteur = prenom;
            emailVisiteur = email;
            telVisiteur = tel;
            numVisiteVisiteur = numVisite;
        }

    }
}
