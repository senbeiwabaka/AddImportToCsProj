namespace ViewAnalysis.HelperForms
{
    partial class ViewAllDataForm
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
            this.rtbData = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // rtbData
            // 
            this.rtbData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbData.Location = new System.Drawing.Point(0, 0);
            this.rtbData.Name = "rtbData";
            this.rtbData.ReadOnly = true;
            this.rtbData.Size = new System.Drawing.Size(460, 346);
            this.rtbData.TabIndex = 0;
            this.rtbData.Text = "";
            this.rtbData.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.RtbData_LinkClicked);
            // 
            // ViewAllDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(460, 346);
            this.Controls.Add(this.rtbData);
            this.MinimizeBox = false;
            this.Name = "ViewAllDataForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "View All Data Form";
            this.Shown += new System.EventHandler(this.ViewAllDataForm_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbData;
    }
}