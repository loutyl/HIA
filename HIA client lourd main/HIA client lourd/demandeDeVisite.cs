using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIA_client_lourd
{
    //Class demandeDeVisite représentant une demande de visite émise par un visiteur
    class demandeDeVisite
    {
        //Visiteur à l'origine de la demande de visite
        Visiteur visiteurOrigine;

        internal Visiteur VisiteurOrigine
        {
            get { return visiteurOrigine; }
            set { visiteurOrigine = value; }
        }

        //La date de la visite 
        private string dateVisite;

        //acesseur
        public string DateVisite
        {
            get { return dateVisite; }
            set { dateVisite = value; }
        }

        //L'heure de début de la visite
        private string heureDVisite;

        //acesseur
        public string HeureDVisite
        {
            get { return heureDVisite; }
            set { heureDVisite = value; }
        }

        //L'heure de fin de la visite
        private string heureFVisite;

        //acesseur
        public string HeureFVisite
        {
            get { return heureFVisite; }
            set { heureFVisite = value; }
        }       

        //Constructeur d'une demande de visite
        public demandeDeVisite(Visiteur visiteur, string dateV, string heureD, string heureF)
        {

            //initialisation des attributs d'une demande de visite avec les variables passées en paramêtre 
            //lors de l'instanciation d'une demande de visite
            visiteurOrigine = visiteur;
            dateVisite = dateV;
            heureDVisite = heureD;
            heureFVisite = heureF;

        }

    }
}
