namespace HIA_client_lourd.Class
{
    //class bonDeVisite représentant un bon de visite envoyé au visiteur lorsque la demande est acceptée
    public class BonDeVisite
    {

        public string NumBonVisite { get; set; }

        public string DateBonVisite { get; set; }

        public string HeureBonDVisite { get; set; }

        public string HeureBonFVisite { get; set; }

        public BonDeVisite(string dateBonVisite, string heureDVisite, string heureFVisite, string numBonVisite)
        {
            DateBonVisite = dateBonVisite;
            HeureBonDVisite = heureDVisite;
            HeureBonFVisite = heureFVisite;
            NumBonVisite = numBonVisite;

        }

    }
}
