namespace arrow_keys_move_button
{
    partial class MainForm
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
            pictureBox1 = new PictureBoxEx();
            pictureBox2 = new PictureBoxEx();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.LightBlue;
            pictureBox1.BorderStyle = BorderStyle.None;
            pictureBox1.Location = new Point(100, 68);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(75, 75);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            pictureBox2.BackColor = Color.LightBlue;
            pictureBox2.BorderStyle = BorderStyle.None;
            pictureBox2.Location = new Point(290, 68);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(75, 75);
            pictureBox2.TabIndex = 1;
            pictureBox2.TabStop = false;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(478, 244);
            Controls.Add(pictureBox1);
            Controls.Add(pictureBox2);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Main Form";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBoxEx pictureBox1;
        private PictureBoxEx pictureBox2;
    }
}