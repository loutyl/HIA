namespace HIA_client_lourd.Class
{
    //class bonDeVisite représentant un bon de visite envoyé au visiteur lorsque la demande est acceptée
    public class BonDeVisite
    {
        //Le numéro du bon de visite

        //acesseur
        public string NumBonVisite { get; set; }

        //La date de la visite

        //acesseur
        public string DateBonVisite { get; set; }

        //L'heure de début de la visite inscrite dans le bon de visite

        //acesseur
        public string HeureBonDVisite { get; set; }

        //L'heure de fin de la visite inscrite dans le bon de visite

        //acesseur
        public string HeureBonFVisite { get; set; }

        //Constructeur d'un bon de visite
        public BonDeVisite(string value1, string value2, string value3, string value4)
        {
            //initialisation des attributs du bon de visite avec les variables passées en paramêtre 
            //lors de l'instanciation d'un bon de visite
            DateBonVisite = value1;
            HeureBonDVisite = value2;
            HeureBonFVisite = value3;
            NumBonVisite = value4;

        }

    }
}
