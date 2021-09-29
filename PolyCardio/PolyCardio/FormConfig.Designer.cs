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
            this.label2 = new System.Windows.Forms.Label();
            this.numUDRecLen = new System.Windows.Forms.NumericUpDown();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.CB_Filter = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.numUDStartDelay = new System.Windows.Forms.NumericUpDown();
            this.CB_PressureRelief = new System.Windows.Forms.CheckBox();
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
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Record length, sec";
            // 
            // numUDRecLen
            // 
            this.numUDRecLen.Location = new System.Drawing.Point(115, 15);
            this.numUDRecLen.Maximum = new decimal(new int[] {
            20,
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
            this.CB_Filter.Location = new System.Drawing.Point(15, 130);
            this.CB_Filter.Name = "CB_Filter";
            this.CB_Filter.Size = new System.Drawing.Size(48, 17);
            this.CB_Filter.TabIndex = 15;
            this.CB_Filter.Text = "Filter";
            this.CB_Filter.UseVisualStyleBackColor = true;
            this.CB_Filter.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "Start delay, sec";
            // 
            // numUDStartDelay
            // 
            this.numUDStartDelay.Location = new System.Drawing.Point(115, 45);
            this.numUDStartDelay.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numUDStartDelay.Name = "numUDStartDelay";
            this.numUDStartDelay.Size = new System.Drawing.Size(46, 20);
            this.numUDStartDelay.TabIndex = 17;
            // 
            // CB_PressureRelief
            // 
            this.CB_PressureRelief.AutoSize = true;
            this.CB_PressureRelief.Location = new System.Drawing.Point(15, 88);
            this.CB_PressureRelief.Name = "CB_PressureRelief";
            this.CB_PressureRelief.Size = new System.Drawing.Size(193, 17);
            this.CB_PressureRelief.TabIndex = 18;
            this.CB_PressureRelief.Text = "Relief pressure after stop recording ";
            this.CB_PressureRelief.UseVisualStyleBackColor = true;
            // 
            // FormConfig
            // 
            this.AcceptButton = this.butOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.butCancel;
            this.ClientSize = new System.Drawing.Size(520, 187);
            this.Controls.Add(this.CB_PressureRelief);
            this.Controls.Add(this.numUDStartDelay);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.CB_Filter);
            this.Controls.Add(this.numUDRecLen);
            this.Controls.Add(this.label2);
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
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numUDRecLen;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.CheckBox CB_Filter;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numUDStartDelay;
        private System.Windows.Forms.CheckBox CB_PressureRelief;
    }
}