namespace ApexCardio
{
    partial class FormEditMarkers
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.butOk = new System.Windows.Forms.Button();
            this.butCancel = new System.Windows.Forms.Button();
            this.panelGraph = new System.Windows.Forms.Panel();
            this.butFirstDeriv = new System.Windows.Forms.Button();
            this.butSecDeriv = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.butCalc = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 63.76989F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36.23011F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelGraph, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(817, 334);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(460, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(253, 308);
            this.panel1.TabIndex = 1;
            // 
            // butOk
            // 
            this.butOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.butOk.Location = new System.Drawing.Point(11, 156);
            this.butOk.Name = "butOk";
            this.butOk.Size = new System.Drawing.Size(75, 23);
            this.butOk.TabIndex = 1;
            this.butOk.Text = "Ok";
            this.butOk.UseVisualStyleBackColor = true;
            // 
            // butCancel
            // 
            this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.butCancel.Location = new System.Drawing.Point(11, 244);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(75, 23);
            this.butCancel.TabIndex = 0;
            this.butCancel.Text = "Close";
            this.butCancel.UseVisualStyleBackColor = true;
            // 
            // panelGraph
            // 
            this.panelGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelGraph.Location = new System.Drawing.Point(3, 3);
            this.panelGraph.Name = "panelGraph";
            this.panelGraph.Size = new System.Drawing.Size(451, 308);
            this.panelGraph.TabIndex = 2;
            // 
            // butFirstDeriv
            // 
            this.butFirstDeriv.Location = new System.Drawing.Point(11, 9);
            this.butFirstDeriv.Name = "butFirstDeriv";
            this.butFirstDeriv.Size = new System.Drawing.Size(75, 23);
            this.butFirstDeriv.TabIndex = 2;
            this.butFirstDeriv.Text = "First deriv";
            this.butFirstDeriv.UseVisualStyleBackColor = true;
            this.butFirstDeriv.Click += new System.EventHandler(this.butFirstDeriv_Click);
            // 
            // butSecDeriv
            // 
            this.butSecDeriv.Enabled = false;
            this.butSecDeriv.Location = new System.Drawing.Point(11, 38);
            this.butSecDeriv.Name = "butSecDeriv";
            this.butSecDeriv.Size = new System.Drawing.Size(75, 23);
            this.butSecDeriv.TabIndex = 3;
            this.butSecDeriv.Text = "Sec deriv";
            this.butSecDeriv.UseVisualStyleBackColor = true;
            this.butSecDeriv.Click += new System.EventHandler(this.butSecDeriv_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.butCalc);
            this.panel2.Controls.Add(this.butSecDeriv);
            this.panel2.Controls.Add(this.butFirstDeriv);
            this.panel2.Controls.Add(this.butCancel);
            this.panel2.Controls.Add(this.butOk);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(719, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(95, 308);
            this.panel2.TabIndex = 3;
            // 
            // butCalc
            // 
            this.butCalc.Enabled = false;
            this.butCalc.Location = new System.Drawing.Point(11, 89);
            this.butCalc.Name = "butCalc";
            this.butCalc.Size = new System.Drawing.Size(75, 23);
            this.butCalc.TabIndex = 4;
            this.butCalc.Text = "Calc";
            this.butCalc.UseVisualStyleBackColor = true;
            this.butCalc.Click += new System.EventHandler(this.butCalc_Click);
            // 
            // FormEditMarkers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.butCancel;
            this.ClientSize = new System.Drawing.Size(817, 334);
            this.Controls.Add(this.tableLayoutPanel1);
            this.KeyPreview = true;
            this.Name = "FormEditMarkers";
            this.Text = "FormEditMarkers";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Resize += new System.EventHandler(this.FormEditMarkers_Resize);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button butOk;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.Panel panelGraph;
        private System.Windows.Forms.Button butSecDeriv;
        private System.Windows.Forms.Button butFirstDeriv;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button butCalc;
    }
}