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

        public FormConfig(PolyConfig cfgarg)
        {
            InitializeComponent();
            cfg = cfgarg;
            
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
            cfg.RecordLength = (int)numUDRecLen.Value;
            cfg.StartDelay = (int)numUDStartDelay.Value;
            cfg.FilterOn = CB_Filter.Checked;
        }

    }
}
