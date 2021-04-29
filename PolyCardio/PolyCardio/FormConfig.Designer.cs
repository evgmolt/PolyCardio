namespace PolyCardio
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
            this.checkBoxECG = new System.Windows.Forms.CheckBox();
            this.checkBoxReo = new System.Windows.Forms.CheckBox();
            this.checkBoxSphigmo1 = new System.Windows.Forms.CheckBox();
            this.checkBoxSphigmo2 = new System.Windows.Forms.CheckBox();
            this.checkBoxApex = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numUDRecLen = new System.Windows.Forms.NumericUpDown();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
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
            // checkBoxECG
            // 
            this.checkBoxECG.AutoSize = true;
            this.checkBoxECG.Location = new System.Drawing.Point(12, 30);
            this.checkBoxECG.Name = "checkBoxECG";
            this.checkBoxECG.Size = new System.Drawing.Size(57, 17);
            this.checkBoxECG.TabIndex = 2;
            this.checkBoxECG.Text = "ECG 1";
            this.checkBoxECG.UseVisualStyleBackColor = true;
            // 
            // checkBoxReo
            // 
            this.checkBoxReo.AutoSize = true;
            this.checkBoxReo.Location = new System.Drawing.Point(12, 50);
            this.checkBoxReo.Name = "checkBoxReo";
            this.checkBoxReo.Size = new System.Drawing.Size(69, 17);
            this.checkBoxReo.TabIndex = 4;
            this.checkBoxReo.Text = "Reogram";
            this.checkBoxReo.UseVisualStyleBackColor = true;
            // 
            // checkBoxSphigmo1
            // 
            this.checkBoxSphigmo1.AutoSize = true;
            this.checkBoxSphigmo1.Location = new System.Drawing.Point(12, 73);
            this.checkBoxSphigmo1.Name = "checkBoxSphigmo1";
            this.checkBoxSphigmo1.Size = new System.Drawing.Size(99, 17);
            this.checkBoxSphigmo1.TabIndex = 6;
            this.checkBoxSphigmo1.Text = "Sphigmogram 1";
            this.checkBoxSphigmo1.UseVisualStyleBackColor = true;
            // 
            // checkBoxSphigmo2
            // 
            this.checkBoxSphigmo2.AutoSize = true;
            this.checkBoxSphigmo2.Location = new System.Drawing.Point(12, 96);
            this.checkBoxSphigmo2.Name = "checkBoxSphigmo2";
            this.checkBoxSphigmo2.Size = new System.Drawing.Size(99, 17);
            this.checkBoxSphigmo2.TabIndex = 7;
            this.checkBoxSphigmo2.Text = "Sphigmogram 2";
            this.checkBoxSphigmo2.UseVisualStyleBackColor = true;
            // 
            // checkBoxApex
            // 
            this.checkBoxApex.AutoSize = true;
            this.checkBoxApex.Location = new System.Drawing.Point(12, 119);
            this.checkBoxApex.Name = "checkBoxApex";
            this.checkBoxApex.Size = new System.Drawing.Size(105, 17);
            this.checkBoxApex.TabIndex = 8;
            this.checkBoxApex.Text = "Apex cardiogram";
            this.checkBoxApex.UseVisualStyleBackColor = true;
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
            this.Controls.Add(this.numUDRecLen);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkBoxApex);
            this.Controls.Add(this.checkBoxSphigmo2);
            this.Controls.Add(this.checkBoxSphigmo1);
            this.Controls.Add(this.checkBoxReo);
            this.Controls.Add(this.checkBoxECG);
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
        private System.Windows.Forms.CheckBox checkBoxECG;
        private System.Windows.Forms.CheckBox checkBoxReo;
        private System.Windows.Forms.CheckBox checkBoxSphigmo1;
        private System.Windows.Forms.CheckBox checkBoxSphigmo2;
        private System.Windows.Forms.CheckBox checkBoxApex;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numUDRecLen;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.CheckBox CB_Filter;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numUDStartDelay;
    }
}