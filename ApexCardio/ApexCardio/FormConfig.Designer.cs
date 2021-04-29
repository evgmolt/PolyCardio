namespace ApexCardio
{
    partial class FormConfig
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
            this.butOk = new System.Windows.Forms.Button();
            this.butCancel = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.checkBox6 = new System.Windows.Forms.CheckBox();
            this.checkBox7 = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numUDRecLen = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.tbArcPath = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.butSelectArcPath = new System.Windows.Forms.Button();
            this.CB_Filter = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.numUDStartDelay = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numUDRecLen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUDStartDelay)).BeginInit();
            this.SuspendLayout();
            // 
            // butOk
            // 
            this.butOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.butOk.Location = new System.Drawing.Point(413, 12);
            this.butOk.Name = "butOk";
            this.butOk.Size = new System.Drawing.Size(75, 23);
            this.butOk.TabIndex = 0;
            this.butOk.Text = "OK";
            this.butOk.UseVisualStyleBackColor = true;
            this.butOk.Click += new System.EventHandler(this.butOk_Click);
            // 
            // butCancel
            // 
            this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.butCancel.Location = new System.Drawing.Point(413, 50);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(75, 23);
            this.butCancel.TabIndex = 1;
            this.butCancel.Text = "Cancel";
            this.butCancel.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(12, 30);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(57, 17);
            this.checkBox1.TabIndex = 2;
            this.checkBox1.Text = "ECG 1";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(12, 53);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(57, 17);
            this.checkBox2.TabIndex = 3;
            this.checkBox2.Text = "ECG 2";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(12, 76);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(78, 17);
            this.checkBox3.TabIndex = 4;
            this.checkBox3.Text = "Reogram 1";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Location = new System.Drawing.Point(12, 99);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(78, 17);
            this.checkBox4.TabIndex = 5;
            this.checkBox4.Text = "Reogram 2";
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // checkBox5
            // 
            this.checkBox5.AutoSize = true;
            this.checkBox5.Location = new System.Drawing.Point(12, 122);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(99, 17);
            this.checkBox5.TabIndex = 6;
            this.checkBox5.Text = "Sphigmogram 1";
            this.checkBox5.UseVisualStyleBackColor = true;
            // 
            // checkBox6
            // 
            this.checkBox6.AutoSize = true;
            this.checkBox6.Location = new System.Drawing.Point(12, 145);
            this.checkBox6.Name = "checkBox6";
            this.checkBox6.Size = new System.Drawing.Size(99, 17);
            this.checkBox6.TabIndex = 7;
            this.checkBox6.Text = "Sphigmogram 2";
            this.checkBox6.UseVisualStyleBackColor = true;
            // 
            // checkBox7
            // 
            this.checkBox7.AutoSize = true;
            this.checkBox7.Location = new System.Drawing.Point(12, 168);
            this.checkBox7.Name = "checkBox7";
            this.checkBox7.Size = new System.Drawing.Size(105, 17);
            this.checkBox7.TabIndex = 8;
            this.checkBox7.Text = "Apex cardiogram";
            this.checkBox7.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "View:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 218);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Record length, sec";
            // 
            // numUDRecLen
            // 
            this.numUDRecLen.Location = new System.Drawing.Point(112, 216);
            this.numUDRecLen.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numUDRecLen.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numUDRecLen.Name = "numUDRecLen";
            this.numUDRecLen.Size = new System.Drawing.Size(46, 20);
            this.numUDRecLen.TabIndex = 11;
            this.numUDRecLen.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 277);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Archiver 7z path";
            // 
            // tbArcPath
            // 
            this.tbArcPath.Location = new System.Drawing.Point(112, 274);
            this.tbArcPath.Name = "tbArcPath";
            this.tbArcPath.Size = new System.Drawing.Size(329, 20);
            this.tbArcPath.TabIndex = 13;
            // 
            // butSelectArcPath
            // 
            this.butSelectArcPath.Location = new System.Drawing.Point(447, 272);
            this.butSelectArcPath.Name = "butSelectArcPath";
            this.butSelectArcPath.Size = new System.Drawing.Size(28, 23);
            this.butSelectArcPath.TabIndex = 14;
            this.butSelectArcPath.Text = "...";
            this.butSelectArcPath.UseVisualStyleBackColor = true;
            this.butSelectArcPath.Click += new System.EventHandler(this.butSelectArcPath_Click);
            // 
            // CB_Filter
            // 
            this.CB_Filter.AutoSize = true;
            this.CB_Filter.Location = new System.Drawing.Point(12, 308);
            this.CB_Filter.Name = "CB_Filter";
            this.CB_Filter.Size = new System.Drawing.Size(48, 17);
            this.CB_Filter.TabIndex = 15;
            this.CB_Filter.Text = "Filter";
            this.CB_Filter.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 248);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "Start delay, sec";
            // 
            // numUDStartDelay
            // 
            this.numUDStartDelay.Location = new System.Drawing.Point(112, 246);
            this.numUDStartDelay.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numUDStartDelay.Name = "numUDStartDelay";
            this.numUDStartDelay.Size = new System.Drawing.Size(46, 20);
            this.numUDStartDelay.TabIndex = 17;
            // 
            // FormConfig
            // 
            this.AcceptButton = this.butOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.butCancel;
            this.ClientSize = new System.Drawing.Size(500, 353);
            this.Controls.Add(this.numUDStartDelay);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.CB_Filter);
            this.Controls.Add(this.butSelectArcPath);
            this.Controls.Add(this.tbArcPath);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.numUDRecLen);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkBox7);
            this.Controls.Add(this.checkBox6);
            this.Controls.Add(this.checkBox5);
            this.Controls.Add(this.checkBox4);
            this.Controls.Add(this.checkBox3);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.butOk);
            this.KeyPreview = true;
            this.Name = "FormConfig";
            this.Text = "Settings";
            ((System.ComponentModel.ISupportInitialize)(this.numUDRecLen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUDStartDelay)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button butOk;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.CheckBox checkBox5;
        private System.Windows.Forms.CheckBox checkBox6;
        private System.Windows.Forms.CheckBox checkBox7;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numUDRecLen;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbArcPath;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button butSelectArcPath;
        private System.Windows.Forms.CheckBox CB_Filter;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numUDStartDelay;
    }
}