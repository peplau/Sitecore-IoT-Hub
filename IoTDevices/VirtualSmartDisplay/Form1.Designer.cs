namespace IoTDevices.VirtualSmartDisplay
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.picBackground = new System.Windows.Forms.PictureBox();
            this.picTablet = new System.Windows.Forms.PictureBox();
            this.picTablet2 = new System.Windows.Forms.PictureBox();
            this.picMobile1 = new System.Windows.Forms.PictureBox();
            this.picMobile2 = new System.Windows.Forms.PictureBox();
            this.picMobile3 = new System.Windows.Forms.PictureBox();
            this.picHeadphones1 = new System.Windows.Forms.PictureBox();
            this.picHeadphones2 = new System.Windows.Forms.PictureBox();
            this.picHeadphones3 = new System.Windows.Forms.PictureBox();
            this.picSelected = new System.Windows.Forms.PictureBox();
            this.labSelected = new System.Windows.Forms.Label();
            this.outputLog = new System.Windows.Forms.TextBox();
            this.labUnselect = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picBackground)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTablet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTablet2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMobile1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMobile2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMobile3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHeadphones1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHeadphones2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHeadphones3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSelected)).BeginInit();
            this.SuspendLayout();
            // 
            // picBackground
            // 
            this.picBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picBackground.Image = ((System.Drawing.Image)(resources.GetObject("picBackground.Image")));
            this.picBackground.Location = new System.Drawing.Point(0, 0);
            this.picBackground.Name = "picBackground";
            this.picBackground.Size = new System.Drawing.Size(568, 720);
            this.picBackground.TabIndex = 0;
            this.picBackground.TabStop = false;
            this.picBackground.Paint += new System.Windows.Forms.PaintEventHandler(this.picBackground_Paint);
            // 
            // picTablet
            // 
            this.picTablet.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picTablet.Image = ((System.Drawing.Image)(resources.GetObject("picTablet.Image")));
            this.picTablet.Location = new System.Drawing.Point(247, 100);
            this.picTablet.Name = "picTablet";
            this.picTablet.Size = new System.Drawing.Size(138, 111);
            this.picTablet.TabIndex = 1;
            this.picTablet.TabStop = false;
            this.picTablet.Click += new System.EventHandler(this.picObject_Click);
            this.picTablet.Paint += new System.Windows.Forms.PaintEventHandler(this.picTablet_Paint);
            // 
            // picTablet2
            // 
            this.picTablet2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picTablet2.Image = ((System.Drawing.Image)(resources.GetObject("picTablet2.Image")));
            this.picTablet2.Location = new System.Drawing.Point(382, 100);
            this.picTablet2.Name = "picTablet2";
            this.picTablet2.Size = new System.Drawing.Size(158, 114);
            this.picTablet2.TabIndex = 2;
            this.picTablet2.TabStop = false;
            this.picTablet2.Click += new System.EventHandler(this.picObject_Click);
            this.picTablet2.Paint += new System.Windows.Forms.PaintEventHandler(this.picTablet2_Paint);
            // 
            // picMobile1
            // 
            this.picMobile1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picMobile1.Image = ((System.Drawing.Image)(resources.GetObject("picMobile1.Image")));
            this.picMobile1.Location = new System.Drawing.Point(283, 228);
            this.picMobile1.Name = "picMobile1";
            this.picMobile1.Size = new System.Drawing.Size(45, 107);
            this.picMobile1.TabIndex = 3;
            this.picMobile1.TabStop = false;
            this.picMobile1.Click += new System.EventHandler(this.picObject_Click);
            this.picMobile1.Paint += new System.Windows.Forms.PaintEventHandler(this.picMobile1_Paint);
            // 
            // picMobile2
            // 
            this.picMobile2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picMobile2.Image = ((System.Drawing.Image)(resources.GetObject("picMobile2.Image")));
            this.picMobile2.Location = new System.Drawing.Point(353, 217);
            this.picMobile2.Name = "picMobile2";
            this.picMobile2.Size = new System.Drawing.Size(59, 113);
            this.picMobile2.TabIndex = 4;
            this.picMobile2.TabStop = false;
            this.picMobile2.Click += new System.EventHandler(this.picObject_Click);
            this.picMobile2.Paint += new System.Windows.Forms.PaintEventHandler(this.picMobile2_Paint);
            // 
            // picMobile3
            // 
            this.picMobile3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picMobile3.Image = ((System.Drawing.Image)(resources.GetObject("picMobile3.Image")));
            this.picMobile3.Location = new System.Drawing.Point(428, 216);
            this.picMobile3.Name = "picMobile3";
            this.picMobile3.Size = new System.Drawing.Size(46, 105);
            this.picMobile3.TabIndex = 5;
            this.picMobile3.TabStop = false;
            this.picMobile3.Click += new System.EventHandler(this.picObject_Click);
            this.picMobile3.Paint += new System.Windows.Forms.PaintEventHandler(this.picMobile3_Paint);
            // 
            // picHeadphones1
            // 
            this.picHeadphones1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picHeadphones1.Image = ((System.Drawing.Image)(resources.GetObject("picHeadphones1.Image")));
            this.picHeadphones1.Location = new System.Drawing.Point(238, 363);
            this.picHeadphones1.Name = "picHeadphones1";
            this.picHeadphones1.Size = new System.Drawing.Size(102, 122);
            this.picHeadphones1.TabIndex = 6;
            this.picHeadphones1.TabStop = false;
            this.picHeadphones1.Click += new System.EventHandler(this.picObject_Click);
            this.picHeadphones1.Paint += new System.Windows.Forms.PaintEventHandler(this.picHeadphones1_Paint);
            // 
            // picHeadphones2
            // 
            this.picHeadphones2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picHeadphones2.Image = ((System.Drawing.Image)(resources.GetObject("picHeadphones2.Image")));
            this.picHeadphones2.Location = new System.Drawing.Point(319, 350);
            this.picHeadphones2.Name = "picHeadphones2";
            this.picHeadphones2.Size = new System.Drawing.Size(123, 123);
            this.picHeadphones2.TabIndex = 7;
            this.picHeadphones2.TabStop = false;
            this.picHeadphones2.Click += new System.EventHandler(this.picObject_Click);
            this.picHeadphones2.Paint += new System.Windows.Forms.PaintEventHandler(this.picHeadphones2_Paint);
            // 
            // picHeadphones3
            // 
            this.picHeadphones3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picHeadphones3.Image = ((System.Drawing.Image)(resources.GetObject("picHeadphones3.Image")));
            this.picHeadphones3.Location = new System.Drawing.Point(439, 350);
            this.picHeadphones3.Name = "picHeadphones3";
            this.picHeadphones3.Size = new System.Drawing.Size(100, 98);
            this.picHeadphones3.TabIndex = 8;
            this.picHeadphones3.TabStop = false;
            this.picHeadphones3.Click += new System.EventHandler(this.picObject_Click);
            this.picHeadphones3.Paint += new System.Windows.Forms.PaintEventHandler(this.picHeadphones3_Paint);
            // 
            // picSelected
            // 
            this.picSelected.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.picSelected.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picSelected.Location = new System.Drawing.Point(60, 72);
            this.picSelected.Name = "picSelected";
            this.picSelected.Size = new System.Drawing.Size(152, 119);
            this.picSelected.TabIndex = 9;
            this.picSelected.TabStop = false;
            this.picSelected.Visible = false;
            this.picSelected.Click += new System.EventHandler(this.picSelected_Click);
            // 
            // labSelected
            // 
            this.labSelected.AutoSize = true;
            this.labSelected.Location = new System.Drawing.Point(78, 114);
            this.labSelected.Name = "labSelected";
            this.labSelected.Size = new System.Drawing.Size(113, 13);
            this.labSelected.TabIndex = 10;
            this.labSelected.Text = "Click a product to start";
            // 
            // outputLog
            // 
            this.outputLog.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.outputLog.Location = new System.Drawing.Point(0, 563);
            this.outputLog.Multiline = true;
            this.outputLog.Name = "outputLog";
            this.outputLog.ReadOnly = true;
            this.outputLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.outputLog.Size = new System.Drawing.Size(568, 157);
            this.outputLog.TabIndex = 11;
            // 
            // labUnselect
            // 
            this.labUnselect.AutoSize = true;
            this.labUnselect.Location = new System.Drawing.Point(70, 60);
            this.labUnselect.Name = "labUnselect";
            this.labUnselect.Size = new System.Drawing.Size(142, 13);
            this.labUnselect.TabIndex = 12;
            this.labUnselect.Text = "Click the product to unselect";
            this.labUnselect.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(568, 720);
            this.Controls.Add(this.labUnselect);
            this.Controls.Add(this.outputLog);
            this.Controls.Add(this.labSelected);
            this.Controls.Add(this.picSelected);
            this.Controls.Add(this.picHeadphones1);
            this.Controls.Add(this.picHeadphones2);
            this.Controls.Add(this.picHeadphones3);
            this.Controls.Add(this.picMobile3);
            this.Controls.Add(this.picMobile2);
            this.Controls.Add(this.picMobile1);
            this.Controls.Add(this.picTablet2);
            this.Controls.Add(this.picTablet);
            this.Controls.Add(this.picBackground);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "Sitecore IoT Hub - Smart Display (Virtual Device)";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picBackground)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTablet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTablet2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMobile1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMobile2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMobile3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHeadphones1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHeadphones2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHeadphones3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSelected)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picBackground;
        private System.Windows.Forms.PictureBox picTablet;
        private System.Windows.Forms.PictureBox picTablet2;
        private System.Windows.Forms.PictureBox picMobile1;
        private System.Windows.Forms.PictureBox picMobile2;
        private System.Windows.Forms.PictureBox picMobile3;
        private System.Windows.Forms.PictureBox picHeadphones1;
        private System.Windows.Forms.PictureBox picHeadphones2;
        private System.Windows.Forms.PictureBox picHeadphones3;
        private System.Windows.Forms.PictureBox picSelected;
        private System.Windows.Forms.Label labSelected;
        private System.Windows.Forms.TextBox outputLog;
        private System.Windows.Forms.Label labUnselect;
    }
}

