using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIA_client_lourd
{
    //class bonDeVisite représentant un bon de visite envoyé au visiteur lorsque la demande est acceptée
    public class bonDeVisite
    {
        //Le numéro du bon de visite
        private string num_bon_visite;

        //acesseur
        public string Num_bon_visite
        {
            get { return num_bon_visite; }
            set { num_bon_visite = value; }
        }

        //La date de la visite
        private string dateBonVisite;

        //acesseur
        public string DateBonVisite
        {
            get { return dateBonVisite; }
            set { dateBonVisite = value; }
        }

        //L'heure de début de la visite inscrite dans le bon de visite
        private string heureBonDVisite;

        //acesseur
        public string HeureBonDVisite
        {
            get { return heureBonDVisite; }
            set { heureBonDVisite = value; }
        }

        //L'heure de fin de la visite inscrite dans le bon de visite
        private string heureBonFVisite;

        //acesseur
        public string HeureBonFVisite
        {
            get { return heureBonFVisite; }
            set { heureBonFVisite = value; }
        }

        //Constructeur d'un bon de visite
        public bonDeVisite(string value1, string value2, string value3, string value4)
        {
            //initialisation des attributs du bon de visite avec les variables passées en paramêtre 
            //lors de l'instanciation d'un bon de visite
            dateBonVisite = value1;
            heureBonDVisite = value2;
            heureBonFVisite = value3;
            num_bon_visite = value4;

        }

    }
}
