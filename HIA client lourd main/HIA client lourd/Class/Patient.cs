using System;
using System.Collections.Generic;
using System.Configuration;
using databaseHIA;

namespace HIA_client_lourd.Class
{
    public class Patient
    {
        public string NomPatient { get; private set; }

        public string PrenomPatient { get; private set; }

        public string AgePatient { get; private set; }

        public string EtagePatient { get; private set; }

        public string ChambrePatient { get; private set; }

        public string LitPatient { get; private set; }

        public string IdPatient { get; private set; }

        public string NumVisitePatient { get; private set; }

        public int StatusVisite { get; private set; }

        public Patient(List<string> infoPatient)
        {
            IdPatient = infoPatient[0];
            NomPatient = infoPatient[1];
            PrenomPatient = infoPatient[2];
            AgePatient = infoPatient[3];
            EtagePatient = infoPatient[4];
            ChambrePatient = infoPatient[5];
            LitPatient = infoPatient[6];
            NumVisitePatient = infoPatient[7];
            StatusVisite = Convert.ToInt32(infoPatient[8]);

        }

        public List<DemandeDeVisite> GetDemandeDeVisite()
        {
            const int status = 3;

            List<DemandeDeVisite> listVisite = new List<DemandeDeVisite>();

            HeavyClientDatabaseObject hdb = new HeavyClientDatabaseObject(ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString);

            var listDemandeDeVisite = hdb.GetDemandeDeVisite(status, NomPatient);
            if (listDemandeDeVisite.Count <= 0) return listVisite;
            foreach (List<string> list in listDemandeDeVisite)
            {
                Visiteur visiteur = new Visiteur(list[0], list[1], list[2], list[3], list[4]);

                DemandeDeVisite demande = new DemandeDeVisite(visiteur, list[6], list[7], list[8]);

                listVisite.Add(demande);
            }
            return listVisite;

        }
    }
}
