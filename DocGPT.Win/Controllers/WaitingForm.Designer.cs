namespace DocGPT.Win.Controllers
{
    partial class WaitingForm
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
            label1 = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(125, 32);
            label1.Name = "label1";
            label1.Size = new Size(35, 13);
            label1.TabIndex = 0;
            label1.Text = "label1";
            // 
            // WaitingForm
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(335, 135);
            Controls.Add(label1);
            Name = "WaitingForm";
            Text = "WaitingForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        public Label label1;
    }
}