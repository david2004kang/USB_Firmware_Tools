namespace USB_Firmware_Tools
{
    partial class CustomExitDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomExitDialog));
            button1 = new Button();
            label2 = new Label();
            pictureBox1 = new PictureBox();
            button2 = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Font = new Font("MS Reference Sans Serif", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button1.Location = new Point(19, 219);
            button1.Margin = new Padding(10);
            button1.Name = "button1";
            button1.Size = new Size(123, 42);
            button1.TabIndex = 7;
            button1.TabStop = false;
            button1.Text = "Cancel";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label2
            // 
            label2.Font = new Font("MS Reference Sans Serif", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(19, 132);
            label2.Name = "label2";
            label2.Padding = new Padding(10, 0, 10, 0);
            label2.Size = new Size(266, 75);
            label2.TabIndex = 6;
            label2.Text = "Do you really want to Exit the program?\r\n";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.None;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(105, 27);
            pictureBox1.Margin = new Padding(0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(92, 91);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 4;
            pictureBox1.TabStop = false;
            // 
            // button2
            // 
            button2.Font = new Font("MS Reference Sans Serif", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button2.Location = new Point(162, 219);
            button2.Margin = new Padding(10);
            button2.Name = "button2";
            button2.Size = new Size(123, 42);
            button2.TabIndex = 8;
            button2.TabStop = false;
            button2.Text = "Exit";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // CustomExitDialog
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(305, 285);
            ControlBox = false;
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(label2);
            Controls.Add(pictureBox1);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "CustomExitDialog";
            ShowIcon = false;
            ShowInTaskbar = false;
            Text = "Form2";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button button1;
        private Label label2;
        private PictureBox pictureBox1;
        private Button button2;
    }
}