namespace MelonyLovense
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            uidLabel = new Label();
            uidTB = new TextBox();
            statusLabel = new Label();
            statusPicture = new PictureBox();
            tick = new System.Windows.Forms.Timer(components);
            ((System.ComponentModel.ISupportInitialize)statusPicture).BeginInit();
            SuspendLayout();
            // 
            // uidLabel
            // 
            uidLabel.AutoSize = true;
            uidLabel.Location = new Point(9, 15);
            uidLabel.Name = "uidLabel";
            uidLabel.Size = new Size(61, 15);
            uidLabel.TabIndex = 0;
            uidLabel.Text = "Discord ID";
            // 
            // uidTB
            // 
            uidTB.Location = new Point(76, 12);
            uidTB.Name = "uidTB";
            uidTB.Size = new Size(200, 23);
            uidTB.TabIndex = 1;
            // 
            // statusLabel
            // 
            statusLabel.AutoSize = true;
            statusLabel.Location = new Point(9, 42);
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new Size(42, 15);
            statusLabel.TabIndex = 2;
            statusLabel.Text = "Status:";
            // 
            // statusPicture
            // 
            statusPicture.BackColor = Color.FromArgb(128, 255, 128);
            statusPicture.Location = new Point(282, 15);
            statusPicture.Name = "statusPicture";
            statusPicture.Size = new Size(32, 32);
            statusPicture.TabIndex = 3;
            statusPicture.TabStop = false;
            // 
            // tick
            // 
            tick.Enabled = true;
            tick.Interval = 250;
            tick.Tick += tick_Tick;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(326, 66);
            Controls.Add(statusPicture);
            Controls.Add(statusLabel);
            Controls.Add(uidTB);
            Controls.Add(uidLabel);
            Name = "Form1";
            Text = "Melony's Maid";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)statusPicture).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label uidLabel;
        private TextBox uidTB;
        private Label statusLabel;
        private PictureBox statusPicture;
        private System.Windows.Forms.Timer tick;
    }
}