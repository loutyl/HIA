using databaseHIA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIA_client_lourd
{
    public class demandeDeVisite
    {
        Visiteur _visiteurOrigine;
        internal Visiteur VisiteurOrigine
        {
            get { return this._visiteurOrigine; }
            set { this._visiteurOrigine = value; }
        }

        private string _dateVisite;
        public string DateVisite
        {
            get { return this._dateVisite; }
            set { this._dateVisite = value; }
        }

        private string _heureDVisite;
        public string HeureDVisite
        {
            get { return this._heureDVisite; }
            set { this._heureDVisite = value; }
        }

        private string _heureFVisite;
        public string HeureFVisite
        {
            get { return this._heureFVisite; }
            set { this._heureFVisite = value; }
        }

        public demandeDeVisite(Visiteur visiteur, string dateV, string heureD, string heureF)
        {
            this._visiteurOrigine = visiteur;
            this._dateVisite = dateV;
            this._heureDVisite = heureD;
            this._heureFVisite = heureF;

        }
    }
}
