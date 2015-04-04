﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIA_client_lourd
{
    public class Patient
    {
        private string _nomPatient;
        
        public string _NomPatient
        {
            get { return this._nomPatient; }
        }
       
        private string _prenomPatient;
        
        public string _PrenomPatient
        {
            get { return this._prenomPatient; }
        }
        
        private string _agePatient;
        
        public string AgePatient
        {
            get { return this._agePatient; }
        }
        
        private string _etagePatient;
        
        public string _EtagePatient
        {
            get { return this._etagePatient; }
        }
        
        private string _chambrePatient;
        
        public string _ChambrePatient
        {
            get { return this._chambrePatient; }
        }
  
        private string _litPatient;

        public string _LitPatient
        {
            get { return this._litPatient; }
        }

        private string _idPatient;

        public string _IdPatient
        {
            get { return this._idPatient; }
        }

        private string _numVisitePatient;

        public string _NumVisitePatient
        {
            get { return this._numVisitePatient; }
        }

        private int _statusVisite;

        public int _StatusVisite
        {
            get { return this._statusVisite; }
        }

        public Patient (List<string> infoPatient)
        {
            this._idPatient = infoPatient[0];
            this._nomPatient = infoPatient[1];
            this._prenomPatient = infoPatient[2];
            this._agePatient = infoPatient[3];
            this._etagePatient = infoPatient[4];
            this._chambrePatient = infoPatient[5];
            this._litPatient = infoPatient[6];            
            this._numVisitePatient = infoPatient[7];
            this._statusVisite = Convert.ToInt32(infoPatient[8]);

        }
    }
}