using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PolyCardio
{
    public partial class FormConfig : Form
    {
        private readonly PolyConfig cfg;
        CheckBox[] CBArr;
        public FormConfig(PolyConfig cfgarg)
        {
            InitializeComponent();
            cfg = cfgarg;
            CBArr = new CheckBox[PolyConstants.NumOfChannels];
            CBArr[0] = checkBox1;
            CBArr[1] = checkBox2;
            CBArr[2] = checkBox3;
            CBArr[3] = checkBox4;
            CBArr[4] = checkBox5;
            CBArr[5] = checkBox6;
            CBArr[6] = checkBox7;
            for (int i = 0; i < PolyConstants.NumOfChannels; i++)
            {
                CBArr[i].Checked = cfg.VisibleGraphs[i];
            }
            numUDRecLen.Value = cfg.RecordLength;
            numUDStartDelay.Value = cfg.StartDelay;
            tbArcPath.Text = cfg.ArchiverPath;
            CB_Filter.Checked = cfg.FilterOn;
        }

        public PolyConfig GetDialogData()
        {
            return cfg;
        }

        private void butOk_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < PolyConstants.NumOfChannels; i++)
            {
                cfg.VisibleGraphs[i] = CBArr[i].Checked;
            }
            cfg.RecordLength = (int)numUDRecLen.Value;
            cfg.StartDelay = (int)numUDStartDelay.Value;
            cfg.ArchiverPath = tbArcPath.Text;
            cfg.FilterOn = CB_Filter.Checked;
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void butSelectArcPath_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowNewFolderButton = true;
            folderBrowserDialog1.SelectedPath = cfg.ArchiverPath;

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                tbArcPath.Text = folderBrowserDialog1.SelectedPath + @"\";
            }

        }

    }
}
