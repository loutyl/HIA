using System.ComponentModel;
using System.Windows.Forms;

namespace HIA_client_lourd.Forms
{
    partial class HistoriqueVisite
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.cmBStatus = new System.Windows.Forms.ComboBox();
            this.lblStatus = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(0, 64);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowHeadersVisible = false;
            this.dataGridView2.Size = new System.Drawing.Size(611, 181);
            this.dataGridView2.TabIndex = 0;
            this.dataGridView2.Visible = false;
            // 
            // cmBStatus
            // 
            this.cmBStatus.FormattingEnabled = true;
            this.cmBStatus.Items.AddRange(new object[] {
            "Fini",
            "Accepter",
            "Refuser"});
            this.cmBStatus.Location = new System.Drawing.Point(85, 16);
            this.cmBStatus.Name = "cmBStatus";
            this.cmBStatus.Size = new System.Drawing.Size(162, 21);
            this.cmBStatus.TabIndex = 1;
            this.cmBStatus.TextChanged += new System.EventHandler(this.cmBStatus_TextChanged);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(13, 19);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(43, 13);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.Text = "Status :";
            // 
            // HistoriqueVisite
            // 
            this.ClientSize = new System.Drawing.Size(601, 245);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.cmBStatus);
            this.Controls.Add(this.dataGridView2);
            this.Name = "HistoriqueVisite";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DataGridView dataGridView2;
        private ComboBox cmBStatus;
        private Label lblStatus;
    }
}