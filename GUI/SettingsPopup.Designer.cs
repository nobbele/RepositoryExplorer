namespace GUI
{
    partial class SettingsPopup
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.ipfield = new System.Windows.Forms.TextBox();
            this.passfield = new System.Windows.Forms.TextBox();
            this.portfield = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.deblocation = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ipfield
            // 
            this.ipfield.Location = new System.Drawing.Point(76, 2);
            this.ipfield.Name = "ipfield";
            this.ipfield.Size = new System.Drawing.Size(100, 20);
            this.ipfield.TabIndex = 0;
            this.ipfield.TextChanged += new System.EventHandler(this.ipfield_TextChanged);
            // 
            // passfield
            // 
            this.passfield.Location = new System.Drawing.Point(76, 54);
            this.passfield.Name = "passfield";
            this.passfield.Size = new System.Drawing.Size(100, 20);
            this.passfield.TabIndex = 1;
            this.passfield.TextChanged += new System.EventHandler(this.passfield_TextChanged);
            // 
            // portfield
            // 
            this.portfield.Location = new System.Drawing.Point(76, 28);
            this.portfield.Name = "portfield";
            this.portfield.Size = new System.Drawing.Size(100, 20);
            this.portfield.TabIndex = 2;
            this.portfield.TextChanged += new System.EventHandler(this.portfield_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "IP Address";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Port";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Password";
            // 
            // deblocation
            // 
            this.deblocation.Location = new System.Drawing.Point(85, 95);
            this.deblocation.Name = "deblocation";
            this.deblocation.Size = new System.Drawing.Size(100, 20);
            this.deblocation.TabIndex = 6;
            this.deblocation.TextChanged += new System.EventHandler(this.deblocation_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 95);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Deb location";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(191, 95);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "Browse...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // SettingsPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.deblocation);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.portfield);
            this.Controls.Add(this.passfield);
            this.Controls.Add(this.ipfield);
            this.Name = "SettingsPopup";
            this.Text = "SettingsPopup";
            this.Load += new System.EventHandler(this.SettingsPopup_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ipfield;
        private System.Windows.Forms.TextBox passfield;
        private System.Windows.Forms.TextBox portfield;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox deblocation;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1;
    }
}