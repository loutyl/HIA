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
            InitializeComponent();

        }

        private void ConnectBtn_MouseClick(object sender, MouseEventArgs e)
        {
            heavyClientDatabaseObject hDb = new heavyClientDatabaseObject(ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString);
            
            if (hDb.login(textBox_id.Text, textBox_Pwd.Text))
            {
                MainForm mainWindow = new MainForm();

                mainWindow.Show();

                mainWindow.Activate();

                Close(); 
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
