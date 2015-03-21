using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIA_client_lourd
{
    class Visiteur
    {
        private string _idVisiteur;

        private string _nomVisiteur;
        public string _NomVisiteur
        {
            get { return this._nomVisiteur; }
            set { this._nomVisiteur = value; }
        }

        private string _prenVisiteur;
        public string _PrenVisiteur
        {
            get { return this._prenVisiteur; }
            set { this._prenVisiteur = value; }
        }

        private string _emailVisiteur;
        public string _EmailVisiteur
        {
            get { return this._emailVisiteur; }
            set { this._emailVisiteur = value; }
        }

        private string _telVisiteur;
        public string _TelVisiteur
        {
            get { return this._telVisiteur; }
            set { this._telVisiteur = value; }
        }

        private string _numVisiteVisiteur;
        public string _NumVisiteVisiteur
        {
            get { return this._numVisiteVisiteur; }
            set { this._numVisiteVisiteur = value; }
        }

        public Visiteur(string id, string nom, string prenom, string email, string numVisite)
        {
            this._idVisiteur = id;
            this._nomVisiteur = nom;
            this._prenVisiteur = prenom;
            this._emailVisiteur = email;
            this._numVisiteVisiteur = numVisite;
        }

        public Visiteur(string id, string nom, string prenom, string email, string tel, string numVisite)
        {
            this._idVisiteur = id;
            this._nomVisiteur = nom;
            this._prenVisiteur = prenom;
            this._emailVisiteur = email;
            this._telVisiteur = tel;
            this._numVisiteVisiteur = numVisite;
        }

    }
}
