using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HIA_client_lourd
{
    class demandeDeVisiteBox
    {
        private Panel panel;

        public Panel Panel
        {
            get { return panel; }
            set { panel = value; }
        }




        public demandeDeVisiteBox(Panel nomPanel)
        {
            
            panel = nomPanel;

        }
    }
}
