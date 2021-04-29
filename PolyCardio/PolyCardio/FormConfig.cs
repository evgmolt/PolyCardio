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
            CBArr[0] = checkBoxECG;
            CBArr[1] = checkBoxReo;
            CBArr[2] = checkBoxSphigmo1;
            CBArr[3] = checkBoxSphigmo2;
            CBArr[4] = checkBoxApex;
            for (int i = 0; i < PolyConstants.NumOfChannels; i++)
            {
                CBArr[i].Checked = cfg.VisibleGraphs[i];
            }
            numUDRecLen.Value = cfg.RecordLength;
            numUDStartDelay.Value = cfg.StartDelay;
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
            cfg.FilterOn = CB_Filter.Checked;
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
