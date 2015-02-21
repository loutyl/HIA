using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using databaseHIA;

namespace HIA_client_lourd
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();

        }

        private void ConnectBtn_MouseClick(object sender, MouseEventArgs e)
        {
            Button ConnectBtn = sender as Button;

            heavyClientDatabaseObject hDB = new heavyClientDatabaseObject(ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString);
            
            if (hDB.login(textBox_id.Text, textBox_Pwd.Text))
            {
                MainForm MainWindow = new MainForm();

                MainWindow.Show();

                MainWindow.Activate();

                this.Close(); 
            }
            else
            {
                MessageBox.Show("Utilisateur ou mot de passe inconnu", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                textBox_id.Text = String.Empty;
                textBox_Pwd.Text = String.Empty;
            }
        }      
    }
}
