using System;
using System.Configuration;
using System.Windows.Forms;
using databaseHIA;

namespace HIA_client_lourd.Forms
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            this.InitializeComponent();

        }

        private void ConnectBtn_MouseClick(object sender, MouseEventArgs e)
        {
            HeavyClientDatabaseObject hDb = new HeavyClientDatabaseObject(ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString);
            
            if (hDb.Login(textBox_id.Text, textBox_Pwd.Text))
            {
                MainForm mainWindow = new MainForm();

                mainWindow.Show();

                mainWindow.Activate();

                this.Close(); 
            }
            else
            {
                MessageBox.Show(@"Utilisateur ou mot de passe inconnu", @"Erreur", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                textBox_id.Text = String.Empty;
                textBox_Pwd.Text = String.Empty;
            }
        }      
    }
}
