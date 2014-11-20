namespace BattleShip
{
    partial class FormForIP
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
            this.IPHere = new System.Windows.Forms.TextBox();
            this.Accept = new System.Windows.Forms.Button();
            this.Exit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // IPHere
            // 
            this.IPHere.Location = new System.Drawing.Point(88, 23);
            this.IPHere.Name = "IPHere";
            this.IPHere.Size = new System.Drawing.Size(100, 20);
            this.IPHere.TabIndex = 1;
            this.IPHere.Tag = "Enter IP here";
            this.IPHere.Text = "Enter IP here";
            this.IPHere.Enter += new System.EventHandler(this.IPHere_Enter);
            this.IPHere.Leave += new System.EventHandler(this.IPHere_Leave);
            // 
            // Accept
            // 
            this.Accept.Location = new System.Drawing.Point(25, 61);
            this.Accept.Name = "Accept";
            this.Accept.Size = new System.Drawing.Size(75, 23);
            this.Accept.TabIndex = 0;
            this.Accept.Text = "Принять";
            this.Accept.UseVisualStyleBackColor = true;
            this.Accept.Click += new System.EventHandler(this.Accept_Click);
            // 
            // Exit
            // 
            this.Exit.Location = new System.Drawing.Point(171, 61);
            this.Exit.Name = "Exit";
            this.Exit.Size = new System.Drawing.Size(75, 23);
            this.Exit.TabIndex = 2;
            this.Exit.Text = "Отмена";
            this.Exit.UseVisualStyleBackColor = true;
            this.Exit.Click += new System.EventHandler(this.Exit_Click);
            // 
            // FormForIP
            // 
            this.AcceptButton = this.Accept;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Exit;
            this.ClientSize = new System.Drawing.Size(284, 107);
            this.Controls.Add(this.Exit);
            this.Controls.Add(this.Accept);
            this.Controls.Add(this.IPHere);
            this.Name = "FormForIP";
            this.Text = "FormForIP";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Accept;
        private System.Windows.Forms.Button Exit;
        public System.Windows.Forms.TextBox IPHere;
    }
}