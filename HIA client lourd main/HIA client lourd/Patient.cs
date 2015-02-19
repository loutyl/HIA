using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIA_client_lourd
{
    //Class réprésentant un patient
    public class Patient
    {
        //Nom du patient
        private string nomPatient;
        //acesseurs
        public string NomPatient
        {
            get { return nomPatient; }
            set { nomPatient = value; }
        }
        //Prénom du patient
        private string prenomPatient;
        //acesseurs
        public string PrenomPatient
        {
            get { return prenomPatient; }
            set { prenomPatient = value; }
        }
        //Age du patient
        private string agePatient;
        //acesseurs
        public string AgePatient
        {
            get { return agePatient; }
            set { agePatient = value; }
        }
        //Etage du patient
        private string etagePatient;
        //acesseurs
        public string EtagePatient
        {
            get { return etagePatient; }
            set { etagePatient = value; }
        }
        //Chambre du patient
        private string chambrePatient;
        //acesseurs
        public string ChambrePatient
        {
            get { return chambrePatient; }
            set { chambrePatient = value; }
        }
        //Lit du patient
        private string litPatient;
        //acesseurs
        public string LitPatient
        {
            get { return litPatient; }
            set { litPatient = value; }
        }
        //Id du patient
        private string idPatient;
        //accesseurs
        public string IdPatient
        {
            get { return idPatient; }
            set { idPatient = value; }
        }
        //numéro de visite du patient
        private string numVisitePatient;
        //accesseurs
        public string NumVisitePatient
        {
            get { return numVisitePatient; }
            set { numVisitePatient = value; }
        }

        private int statusVisite;

        public int StatusVisite
        {
            get { return statusVisite; }
            set { statusVisite = value; }
        }
        //Constructeur d'un patient
        public Patient (string value1, string value2, string value3, string value4, string value5, string value6, string value7, string value8, int value9)
        {
            //initialisation des attributs du patient avec les variables passées en paramêtre 
            //lors de l'instanciation du patient
            nomPatient = value1;
            prenomPatient = value2;
            agePatient = value3;
            etagePatient = value4;
            chambrePatient = value5;
            litPatient = value6;
            idPatient = value7;
            numVisitePatient = value8;
            statusVisite = value9;

        }

    }
}
