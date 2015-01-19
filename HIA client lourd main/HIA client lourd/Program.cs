using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HIA_client_lourd
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            databaseHIA dbHIA = new databaseHIA();
            LoginForm LoginWindow = new LoginForm();
            LoginWindow.Show();
            Application.Run();
        }
    }
}
