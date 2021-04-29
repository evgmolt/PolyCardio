namespace PolyCardio
{
    partial class FormRecordInfo
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
            this.labDateTime = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gbSex = new System.Windows.Forms.GroupBox();
            this.rbFemale = new System.Windows.Forms.RadioButton();
            this.rbMale = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.butOk = new System.Windows.Forms.Button();
            this.butCancel = new System.Windows.Forms.Button();
            this.nudAge = new System.Windows.Forms.NumericUpDown();
            this.nudHeight = new System.Windows.Forms.NumericUpDown();
            this.nudWeight = new System.Windows.Forms.NumericUpDown();
            this.labLength = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tbComment = new System.Windows.Forms.TextBox();
            this.clbDiagn = new System.Windows.Forms.CheckedListBox();
            this.label6 = new System.Windows.Forms.Label();
            this.gbSex.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudAge)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWeight)).BeginInit();
            this.SuspendLayout();
            // 
            // labDateTime
            // 
            this.labDateTime.AutoSize = true;
            this.labDateTime.Location = new System.Drawing.Point(22, 18);
            this.labDateTime.Name = "labDateTime";
            this.labDateTime.Size = new System.Drawing.Size(67, 13);
            this.labDateTime.TabIndex = 0;
            this.labDateTime.Text = "labDateTime";
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(91, 74);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(337, 20);
            this.tbName.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 77);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Name";
            // 
            // gbSex
            // 
            this.gbSex.Controls.Add(this.rbFemale);
            this.gbSex.Controls.Add(this.rbMale);
            this.gbSex.Location = new System.Drawing.Point(25, 114);
            this.gbSex.Name = "gbSex";
            this.gbSex.Size = new System.Drawing.Size(131, 42);
            this.gbSex.TabIndex = 1;
            this.gbSex.TabStop = false;
            this.gbSex.Text = "Sex";
            // 
            // rbFemale
            // 
            this.rbFemale.AutoSize = true;
            this.rbFemale.Location = new System.Drawing.Point(66, 19);
            this.rbFemale.Name = "rbFemale";
            this.rbFemale.Size = new System.Drawing.Size(59, 17);
            this.rbFemale.TabIndex = 1;
            this.rbFemale.TabStop = true;
            this.rbFemale.Text = "Female";
            this.rbFemale.UseVisualStyleBackColor = true;
            // 
            // rbMale
            // 
            this.rbMale.AutoSize = true;
            this.rbMale.Location = new System.Drawing.Point(6, 19);
            this.rbMale.Name = "rbMale";
            this.rbMale.Size = new System.Drawing.Size(48, 17);
            this.rbMale.TabIndex = 0;
            this.rbMale.TabStop = true;
            this.rbMale.Text = "Male";
            this.rbMale.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(300, 132);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Age";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(300, 160);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Height";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(300, 189);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Weight";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // butOk
            // 
            this.butOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.butOk.Location = new System.Drawing.Point(263, 397);
            this.butOk.Name = "butOk";
            this.butOk.Size = new System.Drawing.Size(75, 23);
            this.butOk.TabIndex = 5;
            this.butOk.Text = "Ok";
            this.butOk.UseVisualStyleBackColor = true;
            this.butOk.Click += new System.EventHandler(this.butOk_Click);
            // 
            // butCancel
            // 
            this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.butCancel.Location = new System.Drawing.Point(353, 397);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(75, 23);
            this.butCancel.TabIndex = 6;
            this.butCancel.Text = "Cancel";
            this.butCancel.UseVisualStyleBackColor = true;
            // 
            // nudAge
            // 
            this.nudAge.Location = new System.Drawing.Point(363, 130);
            this.nudAge.Name = "nudAge";
            this.nudAge.Size = new System.Drawing.Size(65, 20);
            this.nudAge.TabIndex = 2;
            this.nudAge.ValueChanged += new System.EventHandler(this.nudAge_ValueChanged);
            // 
            // nudHeight
            // 
            this.nudHeight.Location = new System.Drawing.Point(363, 158);
            this.nudHeight.Maximum = new decimal(new int[] {
            250,
            0,
            0,
            0});
            this.nudHeight.Name = "nudHeight";
            this.nudHeight.Size = new System.Drawing.Size(65, 20);
            this.nudHeight.TabIndex = 3;
            this.nudHeight.ValueChanged += new System.EventHandler(this.nudHeight_ValueChanged);
            // 
            // nudWeight
            // 
            this.nudWeight.Location = new System.Drawing.Point(363, 187);
            this.nudWeight.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.nudWeight.Name = "nudWeight";
            this.nudWeight.Size = new System.Drawing.Size(65, 20);
            this.nudWeight.TabIndex = 4;
            this.nudWeight.ValueChanged += new System.EventHandler(this.nudWeight_ValueChanged);
            // 
            // labLength
            // 
            this.labLength.AutoSize = true;
            this.labLength.Location = new System.Drawing.Point(22, 40);
            this.labLength.Name = "labLength";
            this.labLength.Size = new System.Drawing.Size(54, 13);
            this.labLength.TabIndex = 18;
            this.labLength.Text = "labLength";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(28, 357);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 13);
            this.label5.TabIndex = 19;
            this.label5.Text = "Comment";
            // 
            // tbComment
            // 
            this.tbComment.Location = new System.Drawing.Point(91, 350);
            this.tbComment.Name = "tbComment";
            this.tbComment.Size = new System.Drawing.Size(337, 20);
            this.tbComment.TabIndex = 20;
            // 
            // clbDiagn
            // 
            this.clbDiagn.CheckOnClick = true;
            this.clbDiagn.FormattingEnabled = true;
            this.clbDiagn.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11"});
            this.clbDiagn.Location = new System.Drawing.Point(91, 178);
            this.clbDiagn.Name = "clbDiagn";
            this.clbDiagn.Size = new System.Drawing.Size(120, 169);
            this.clbDiagn.TabIndex = 21;
            this.clbDiagn.SelectedIndexChanged += new System.EventHandler(this.checkedListBox1_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(28, 178);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 13);
            this.label6.TabIndex = 22;
            this.label6.Text = "Diagnosis";
            // 
            // FormRecordInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.butCancel;
            this.ClientSize = new System.Drawing.Size(460, 478);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.clbDiagn);
            this.Controls.Add(this.tbComment);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.labLength);
            this.Controls.Add(this.nudWeight);
            this.Controls.Add(this.nudHeight);
            this.Controls.Add(this.nudAge);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.butOk);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.gbSex);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.labDateTime);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormRecordInfo";
            this.Text = "Record information";
            this.gbSex.ResumeLayout(false);
            this.gbSex.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudAge)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWeight)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labDateTime;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox gbSex;
        private System.Windows.Forms.RadioButton rbFemale;
        private System.Windows.Forms.RadioButton rbMale;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button butOk;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.NumericUpDown nudAge;
        private System.Windows.Forms.NumericUpDown nudHeight;
        private System.Windows.Forms.NumericUpDown nudWeight;
        private System.Windows.Forms.Label labLength;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbComment;
        private System.Windows.Forms.CheckedListBox clbDiagn;
        private System.Windows.Forms.Label label6;
    }
}