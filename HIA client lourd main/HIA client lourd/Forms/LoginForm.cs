using databaseHIA;

namespace HIA_client_lourd.Forms
{
    public partial class LoginForm : System.Windows.Forms.Form
    {
        public LoginForm()
        {
            this.InitializeComponent();

        }

        private void ConnectBtn_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            HeavyClientDatabaseObject hDb = new HeavyClientDatabaseObject(System.Configuration.ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString);
            
            if (hDb.Login(textBox_id.Text, textBox_Pwd.Text))
            {
                MainForm mainWindow = new MainForm();

                mainWindow.Show();

                mainWindow.Activate();

                this.Close(); 
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(@"Utilisateur ou mot de passe inconnu", @"Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);

                textBox_id.Text = System.String.Empty;
                textBox_Pwd.Text = System.String.Empty;
            }
        }      
    }
}
