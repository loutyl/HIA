using System;
using System.Windows.Forms;
using HIA_client_lourd.Forms;

namespace HIA_client_lourd.Class
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

            LoginForm loginWindow = new LoginForm();

            loginWindow.Show();

            Application.Run();
        }
    }
}
