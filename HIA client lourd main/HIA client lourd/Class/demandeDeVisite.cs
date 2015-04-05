namespace HIA_client_lourd.Class
{
    public class DemandeDeVisite
    {
        internal Visiteur VisiteurOrigine { get; set; }

        public string DateVisite { get; set; }

        public string HeureDVisite { get; set; }

        public string HeureFVisite { get; set; }

        public DemandeDeVisite(Visiteur visiteur, string dateV, string heureD, string heureF)
        {
            VisiteurOrigine = visiteur;
            DateVisite = dateV;
            HeureDVisite = heureD;
            HeureFVisite = heureF;
        }
    }
}
