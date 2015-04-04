namespace HIA_client_lourd.Class
{
    public class Visiteur
    {
        public string IdVisiteur { get; private set; }

        public string NomVisiteur { get; set; }

        public string PrenVisiteur { get; set; }

        public string EmailVisiteur { get; set; }

        public string TelVisiteur { get; set; }

        public string NumVisiteVisiteur { get; set; }

        public Visiteur(string id, string nom, string prenom, string email, string numVisite)
        {
            IdVisiteur = id;
            NomVisiteur = nom;
            PrenVisiteur = prenom;
            EmailVisiteur = email;
            NumVisiteVisiteur = numVisite;
        }

        public Visiteur(string id, string nom, string prenom, string email, string tel, string numVisite)
        {
            IdVisiteur = id;
            NomVisiteur = nom;
            PrenVisiteur = prenom;
            EmailVisiteur = email;
            TelVisiteur = tel;
            NumVisiteVisiteur = numVisite;
        }

    }
}
