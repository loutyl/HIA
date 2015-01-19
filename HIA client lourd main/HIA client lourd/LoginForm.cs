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

namespace HIA_client_lourd
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            //Initialisation des composants du Form
            InitializeComponent();

        }
                 
        //Evenement lors d'un click sur le boutton connect
        private void ConnectBtn_MouseClick(object sender, MouseEventArgs e)
        {
            //Désignaiton du sender de l'évenement
            Button ConnectBtn = sender as Button;

            databaseHIA db = new databaseHIA();

            string username = textBox_id.Text;
            string pwd = textBox_Pwd.Text;

            //Si la méthode login() de l'objet db retourne vrai 
            if (db.login(username,pwd))
            {
                //Instanciation de la Window princiaple
                MainForm MainWindow = new MainForm();
                //Affichage de MainWindow
                MainWindow.Show();
                //Activation de MainWindow
                MainWindow.Activate();
                //Fermeture du Form login
                this.Close(); 
            }
            //Sinon si login() retourne faux
            else
            {
                //Affichage d'un message d'erreur
                MessageBox.Show("Utilisateur ou mot de passe inconnu", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //Reset des textBox
                textBox_id.Text = null;
                textBox_Pwd.Text = null;
            }

        }

        

    }
}
