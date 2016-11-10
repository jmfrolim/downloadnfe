namespace DownloadNfe
{
    partial class FrmPrincipal
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
            this.btnManifesta = new System.Windows.Forms.Button();
            this.btnDownload = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnManifesta
            // 
            this.btnManifesta.Location = new System.Drawing.Point(107, 29);
            this.btnManifesta.Name = "btnManifesta";
            this.btnManifesta.Size = new System.Drawing.Size(75, 23);
            this.btnManifesta.TabIndex = 0;
            this.btnManifesta.Text = "Manifestacao";
            this.btnManifesta.UseVisualStyleBackColor = true;
            this.btnManifesta.Click += new System.EventHandler(this.btnManifesta_Click);
            // 
            // btnDownload
            // 
            this.btnDownload.Location = new System.Drawing.Point(107, 111);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(75, 23);
            this.btnDownload.TabIndex = 1;
            this.btnDownload.Text = "Download";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // FrmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 187);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.btnManifesta);
            this.Name = "FrmPrincipal";
            this.Text = "Download NFE";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnManifesta;
        private System.Windows.Forms.Button btnDownload;
    }
}

