using System;

namespace USB_Firmware_Tools
{
    partial class ProgressDialogForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProgressDialogForm));
            progressBar1 = new ProgressBar();
            Label1 = new Label();
            Label2 = new Label();
            button1 = new Button();
            SuspendLayout();
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(18, 68);
            progressBar1.Margin = new Padding(4, 5, 4, 5);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(638, 46);
            progressBar1.Step = 1;
            progressBar1.TabIndex = 0;
            // 
            // Label1
            // 
            Label1.Font = new Font("Microsoft Sans Serif", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 128);
            Label1.Location = new Point(18, 202);
            Label1.Margin = new Padding(4, 0, 4, 0);
            Label1.Name = "Label1";
            Label1.Size = new Size(638, 26);
            Label1.TabIndex = 1;
            Label1.Text = "Checking device...";
            Label1.Visible = false;
            // 
            // Label2
            // 
            Label2.Font = new Font("Microsoft Sans Serif", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 128);
            Label2.Location = new Point(18, 15);
            Label2.Margin = new Padding(4, 0, 4, 0);
            Label2.Name = "Label2";
            Label2.Padding = new Padding(0, 8, 0, 0);
            Label2.Size = new Size(638, 34);
            Label2.TabIndex = 2;
            Label2.Text = "Please wait ...";
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            button1.Location = new Point(248, 135);
            button1.Name = "button1";
            button1.Size = new Size(146, 29);
            button1.TabIndex = 3;
            button1.Text = "Abort && Quit";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // ProgressDialogForm
            // 
            AutoScaleDimensions = new SizeF(9F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(676, 175);
            ControlBox = false;
            Controls.Add(button1);
            Controls.Add(Label2);
            Controls.Add(Label1);
            Controls.Add(progressBar1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 5, 4, 5);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ProgressDialogForm";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Firmware Updating ...";
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label Label1;
        public string GetLabel1Text()
        {
            return Label1.Text;
        }
        public void SetLabel1Text(string strToChange)
        {
            Label1.Text = strToChange;
        }
        private System.Windows.Forms.Label Label2;
        public string GetLabel2Text()
        {
            return Label2.Text;
        }
        public void SetLabel2Text(string strToChange)
        {
            Label2.Text = strToChange;
        }

        private Button button1;
    }
}