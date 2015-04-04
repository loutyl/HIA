﻿using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace HIA_client_lourd.Forms
{
    partial class LoginForm
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.ConnectBtn = new Button();
            this.lbl_id = new Label();
            this.lblPwd = new Label();
            this.textBox_id = new TextBox();
            this.textBox_Pwd = new TextBox();
            this.SuspendLayout();
            // 
            // ConnectBtn
            // 
            this.ConnectBtn.Location = new Point(127, 135);
            this.ConnectBtn.Name = "ConnectBtn";
            this.ConnectBtn.Size = new Size(75, 23);
            this.ConnectBtn.TabIndex = 0;
            this.ConnectBtn.Text = "Connexion";
            this.ConnectBtn.UseVisualStyleBackColor = true;
            this.ConnectBtn.MouseClick += new MouseEventHandler(this.ConnectBtn_MouseClick);
            // 
            // lbl_id
            // 
            this.lbl_id.AutoSize = true;
            this.lbl_id.Location = new Point(29, 37);
            this.lbl_id.Name = "lbl_id";
            this.lbl_id.Size = new Size(53, 13);
            this.lbl_id.TabIndex = 3;
            this.lbl_id.Text = "Identifiant";
            // 
            // lblPwd
            // 
            this.lblPwd.AutoSize = true;
            this.lblPwd.Location = new Point(29, 82);
            this.lblPwd.Name = "lblPwd";
            this.lblPwd.Size = new Size(71, 13);
            this.lblPwd.TabIndex = 4;
            this.lblPwd.Text = "Mot de passe";
            // 
            // textBox_id
            // 
            this.textBox_id.AccessibleName = "";
            this.textBox_id.Location = new Point(155, 37);
            this.textBox_id.Name = "textBox_id";
            this.textBox_id.Size = new Size(141, 20);
            this.textBox_id.TabIndex = 5;
            // 
            // textBox_Pwd
            // 
            this.textBox_Pwd.Location = new Point(155, 82);
            this.textBox_Pwd.Name = "textBox_Pwd";
            this.textBox_Pwd.PasswordChar = '*';
            this.textBox_Pwd.Size = new Size(141, 20);
            this.textBox_Pwd.TabIndex = 6;
            this.textBox_Pwd.UseSystemPasswordChar = true;
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(336, 195);
            this.Controls.Add(this.textBox_Pwd);
            this.Controls.Add(this.textBox_id);
            this.Controls.Add(this.lblPwd);
            this.Controls.Add(this.lbl_id);
            this.Controls.Add(this.ConnectBtn);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.Text = "H.I.D.I.V";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button ConnectBtn;
        private Label lbl_id;
        private Label lblPwd;
        private TextBox textBox_id;
        private TextBox textBox_Pwd;
    }
}

