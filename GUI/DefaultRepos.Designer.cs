namespace GUI
{
    partial class DefaultRepos
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
            this.view = new System.Windows.Forms.ListView();
            this.add = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // view
            // 
            this.view.HideSelection = false;
            this.view.Location = new System.Drawing.Point(12, 12);
            this.view.Name = "view";
            this.view.Size = new System.Drawing.Size(121, 237);
            this.view.TabIndex = 1;
            this.view.UseCompatibleStateImageBehavior = false;
            this.view.SelectedIndexChanged += new System.EventHandler(this.view_SelectedIndexChanged);
            // 
            // add
            // 
            this.add.Location = new System.Drawing.Point(150, 75);
            this.add.Name = "add";
            this.add.Size = new System.Drawing.Size(75, 23);
            this.add.TabIndex = 2;
            this.add.Text = "Add";
            this.add.UseVisualStyleBackColor = true;
            this.add.Click += new System.EventHandler(this.add_Click);
            // 
            // DefaultRepos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.add);
            this.Controls.Add(this.view);
            this.Name = "DefaultRepos";
            this.Text = "DefaultRepos";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ListView view;
        private System.Windows.Forms.Button add;
    }
}