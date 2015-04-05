namespace HIA_client_lourd.Class
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [System.STAThread]
        static void Main()
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

            Forms.LoginForm loginWindow = new Forms.LoginForm();

            loginWindow.Show();

            System.Windows.Forms.Application.Run();
        }
    }
}
