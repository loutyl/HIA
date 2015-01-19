namespace HIA_client_lourd
{
    partial class DemandeVisitePatient
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnAccepterDemande = new System.Windows.Forms.Button();
            this.btnRefuserDemande = new System.Windows.Forms.Button();
            this.lblDemandeVisiteNomVisiteur = new System.Windows.Forms.Label();
            this.lblDemandeVisitePrenomVisiteur = new System.Windows.Forms.Label();
            this.lblDemandeVisiteDateVisite = new System.Windows.Forms.Label();
            this.lblDemandeVisiteHeureDVisite = new System.Windows.Forms.Label();
            this.lblDemandeVisiteHeureFVisite = new System.Windows.Forms.Label();
            this.btnDemandeVisiteSuivant = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblDemandeDeVisiteAffluence = new System.Windows.Forms.Label();
            this.lblDemandeDeVisiteNivAfflu = new System.Windows.Forms.Label();
            this.btnBloquerVisite = new System.Windows.Forms.Button();
            this.lblNbVisite = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnAccepterDemande
            // 
            this.btnAccepterDemande.Location = new System.Drawing.Point(285, 52);
            this.btnAccepterDemande.Name = "btnAccepterDemande";
            this.btnAccepterDemande.Size = new System.Drawing.Size(120, 23);
            this.btnAccepterDemande.TabIndex = 1;
            this.btnAccepterDemande.Text = "Accepter la visite";
            this.btnAccepterDemande.UseVisualStyleBackColor = true;
            this.btnAccepterDemande.Click += new System.EventHandler(this.btnAccepterDemande_Click);
            // 
            // btnRefuserDemande
            // 
            this.btnRefuserDemande.Location = new System.Drawing.Point(285, 90);
            this.btnRefuserDemande.Name = "btnRefuserDemande";
            this.btnRefuserDemande.Size = new System.Drawing.Size(120, 23);
            this.btnRefuserDemande.TabIndex = 2;
            this.btnRefuserDemande.Text = "Refuser la visite";
            this.btnRefuserDemande.UseVisualStyleBackColor = true;
            this.btnRefuserDemande.Click += new System.EventHandler(this.btnRefuserDemande_Click);
            // 
            // lblDemandeVisiteNomVisiteur
            // 
            this.lblDemandeVisiteNomVisiteur.AutoSize = true;
            this.lblDemandeVisiteNomVisiteur.Location = new System.Drawing.Point(12, 9);
            this.lblDemandeVisiteNomVisiteur.Name = "lblDemandeVisiteNomVisiteur";
            this.lblDemandeVisiteNomVisiteur.Size = new System.Drawing.Size(86, 13);
            this.lblDemandeVisiteNomVisiteur.TabIndex = 3;
            this.lblDemandeVisiteNomVisiteur.Text = "Nom du visiteur :";
            // 
            // lblDemandeVisitePrenomVisiteur
            // 
            this.lblDemandeVisitePrenomVisiteur.AutoSize = true;
            this.lblDemandeVisitePrenomVisiteur.Location = new System.Drawing.Point(12, 36);
            this.lblDemandeVisitePrenomVisiteur.Name = "lblDemandeVisitePrenomVisiteur";
            this.lblDemandeVisitePrenomVisiteur.Size = new System.Drawing.Size(100, 13);
            this.lblDemandeVisitePrenomVisiteur.TabIndex = 4;
            this.lblDemandeVisitePrenomVisiteur.Text = "Prénom du visiteur :";
            // 
            // lblDemandeVisiteDateVisite
            // 
            this.lblDemandeVisiteDateVisite.AutoSize = true;
            this.lblDemandeVisiteDateVisite.Location = new System.Drawing.Point(12, 62);
            this.lblDemandeVisiteDateVisite.Name = "lblDemandeVisiteDateVisite";
            this.lblDemandeVisiteDateVisite.Size = new System.Drawing.Size(89, 13);
            this.lblDemandeVisiteDateVisite.TabIndex = 5;
            this.lblDemandeVisiteDateVisite.Text = "Date de la visite :";
            // 
            // lblDemandeVisiteHeureDVisite
            // 
            this.lblDemandeVisiteHeureDVisite.AutoSize = true;
            this.lblDemandeVisiteHeureDVisite.Location = new System.Drawing.Point(12, 88);
            this.lblDemandeVisiteHeureDVisite.Name = "lblDemandeVisiteHeureDVisite";
            this.lblDemandeVisiteHeureDVisite.Size = new System.Drawing.Size(140, 13);
            this.lblDemandeVisiteHeureDVisite.TabIndex = 6;
            this.lblDemandeVisiteHeureDVisite.Text = "Heure de début de la visite :";
            // 
            // lblDemandeVisiteHeureFVisite
            // 
            this.lblDemandeVisiteHeureFVisite.AutoSize = true;
            this.lblDemandeVisiteHeureFVisite.Location = new System.Drawing.Point(12, 112);
            this.lblDemandeVisiteHeureFVisite.Name = "lblDemandeVisiteHeureFVisite";
            this.lblDemandeVisiteHeureFVisite.Size = new System.Drawing.Size(124, 13);
            this.lblDemandeVisiteHeureFVisite.TabIndex = 7;
            this.lblDemandeVisiteHeureFVisite.Text = "Heure de fin de la visite :";
            // 
            // btnDemandeVisiteSuivant
            // 
            this.btnDemandeVisiteSuivant.Location = new System.Drawing.Point(285, 9);
            this.btnDemandeVisiteSuivant.Name = "btnDemandeVisiteSuivant";
            this.btnDemandeVisiteSuivant.Size = new System.Drawing.Size(120, 23);
            this.btnDemandeVisiteSuivant.TabIndex = 8;
            this.btnDemandeVisiteSuivant.Text = "Demande suivante";
            this.btnDemandeVisiteSuivant.UseVisualStyleBackColor = true;
            this.btnDemandeVisiteSuivant.Click += new System.EventHandler(this.btnDemandeVisiteSuivant_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(194, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "label1";
            this.label1.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(194, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "label2";
            this.label2.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(194, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "label3";
            this.label3.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(194, 88);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "label4";
            this.label4.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(194, 112);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "label5";
            this.label5.Visible = false;
            // 
            // lblDemandeDeVisiteAffluence
            // 
            this.lblDemandeDeVisiteAffluence.AutoSize = true;
            this.lblDemandeDeVisiteAffluence.Location = new System.Drawing.Point(15, 139);
            this.lblDemandeDeVisiteAffluence.Name = "lblDemandeDeVisiteAffluence";
            this.lblDemandeDeVisiteAffluence.Size = new System.Drawing.Size(58, 13);
            this.lblDemandeDeVisiteAffluence.TabIndex = 14;
            this.lblDemandeDeVisiteAffluence.Text = "Affluence :";
            // 
            // lblDemandeDeVisiteNivAfflu
            // 
            this.lblDemandeDeVisiteNivAfflu.AutoSize = true;
            this.lblDemandeDeVisiteNivAfflu.ForeColor = System.Drawing.Color.Red;
            this.lblDemandeDeVisiteNivAfflu.Location = new System.Drawing.Point(194, 139);
            this.lblDemandeDeVisiteNivAfflu.Name = "lblDemandeDeVisiteNivAfflu";
            this.lblDemandeDeVisiteNivAfflu.Size = new System.Drawing.Size(31, 13);
            this.lblDemandeDeVisiteNivAfflu.TabIndex = 15;
            this.lblDemandeDeVisiteNivAfflu.Text = "Forte";
            // 
            // btnBloquerVisite
            // 
            this.btnBloquerVisite.Location = new System.Drawing.Point(285, 129);
            this.btnBloquerVisite.Name = "btnBloquerVisite";
            this.btnBloquerVisite.Size = new System.Drawing.Size(120, 23);
            this.btnBloquerVisite.TabIndex = 16;
            this.btnBloquerVisite.Text = "Bloquer les visites";
            this.btnBloquerVisite.UseVisualStyleBackColor = true;
            this.btnBloquerVisite.Click += new System.EventHandler(this.btnBloquerVisite_Click);
            // 
            // lblNbVisite
            // 
            this.lblNbVisite.AutoSize = true;
            this.lblNbVisite.Location = new System.Drawing.Point(386, 166);
            this.lblNbVisite.Name = "lblNbVisite";
            this.lblNbVisite.Size = new System.Drawing.Size(35, 13);
            this.lblNbVisite.TabIndex = 17;
            this.lblNbVisite.Text = "label6";
            this.lblNbVisite.Visible = false;
            // 
            // DemandeVisitePatient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(433, 188);
            this.Controls.Add(this.lblNbVisite);
            this.Controls.Add(this.btnBloquerVisite);
            this.Controls.Add(this.lblDemandeDeVisiteNivAfflu);
            this.Controls.Add(this.lblDemandeDeVisiteAffluence);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnDemandeVisiteSuivant);
            this.Controls.Add(this.lblDemandeVisiteHeureFVisite);
            this.Controls.Add(this.lblDemandeVisiteHeureDVisite);
            this.Controls.Add(this.lblDemandeVisiteDateVisite);
            this.Controls.Add(this.lblDemandeVisitePrenomVisiteur);
            this.Controls.Add(this.lblDemandeVisiteNomVisiteur);
            this.Controls.Add(this.btnRefuserDemande);
            this.Controls.Add(this.btnAccepterDemande);
            this.MaximizeBox = false;
            this.Name = "DemandeVisitePatient";
            this.Text = "Demande de visite ";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAccepterDemande;
        private System.Windows.Forms.Button btnRefuserDemande;
        private System.Windows.Forms.Label lblDemandeVisiteNomVisiteur;
        private System.Windows.Forms.Label lblDemandeVisitePrenomVisiteur;
        private System.Windows.Forms.Label lblDemandeVisiteDateVisite;
        private System.Windows.Forms.Label lblDemandeVisiteHeureDVisite;
        private System.Windows.Forms.Label lblDemandeVisiteHeureFVisite;
        private System.Windows.Forms.Button btnDemandeVisiteSuivant;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblDemandeDeVisiteAffluence;
        private System.Windows.Forms.Label lblDemandeDeVisiteNivAfflu;
        private System.Windows.Forms.Button btnBloquerVisite;
        private System.Windows.Forms.Label lblNbVisite;
    }
}