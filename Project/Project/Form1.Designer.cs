namespace Project
{
    partial class Form1
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.capture_butt = new System.Windows.Forms.Button();
            this.cameraResolution = new System.Windows.Forms.ComboBox();
            this.detect_butt = new System.Windows.Forms.Button();
            this.results = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.pictureBox1.Location = new System.Drawing.Point(26, 28);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(866, 536);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // capture_butt
            // 
            this.capture_butt.Location = new System.Drawing.Point(269, 639);
            this.capture_butt.Name = "capture_butt";
            this.capture_butt.Size = new System.Drawing.Size(173, 41);
            this.capture_butt.TabIndex = 1;
            this.capture_butt.Text = "Capture";
            this.capture_butt.UseVisualStyleBackColor = true;
            this.capture_butt.Click += new System.EventHandler(this.captureButt_Click);
            // 
            // cameraResolution
            // 
            this.cameraResolution.FormattingEnabled = true;
            this.cameraResolution.Location = new System.Drawing.Point(245, 599);
            this.cameraResolution.Name = "cameraResolution";
            this.cameraResolution.Size = new System.Drawing.Size(435, 21);
            this.cameraResolution.TabIndex = 3;
            this.cameraResolution.SelectedIndexChanged += new System.EventHandler(this.cameraResolution_SelectedIndexChanged);
            // 
            // detect_butt
            // 
            this.detect_butt.Location = new System.Drawing.Point(485, 639);
            this.detect_butt.Name = "detect_butt";
            this.detect_butt.Size = new System.Drawing.Size(173, 41);
            this.detect_butt.TabIndex = 4;
            this.detect_butt.Text = "Detect Emotion";
            this.detect_butt.UseVisualStyleBackColor = true;
            this.detect_butt.Click += new System.EventHandler(this.detect_butt_Click);
            // 
            // results
            // 
            this.results.AutoSize = true;
            this.results.Location = new System.Drawing.Point(957, 28);
            this.results.Name = "results";
            this.results.Size = new System.Drawing.Size(42, 13);
            this.results.TabIndex = 5;
            this.results.Text = "Results";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1270, 692);
            this.Controls.Add(this.results);
            this.Controls.Add(this.detect_butt);
            this.Controls.Add(this.cameraResolution);
            this.Controls.Add(this.capture_butt);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "Camera";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button capture_butt;
        private System.Windows.Forms.ComboBox cameraResolution;
        private System.Windows.Forms.Button detect_butt;
        private System.Windows.Forms.Label results;
    }
}

