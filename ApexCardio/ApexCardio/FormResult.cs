using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ApexCardio
{
    public partial class FormResult : Form
    {
        List<List<double>> ResList;
        public FormResult(List<List<double>> reslist1, List<List<double>> reslist2, List<string> namelist)
        {
            InitializeComponent();
            tbResult.AppendText("First deriv (speed)" + Convert.ToChar(0xA));
            tbResult.AppendText("  " + Convert.ToChar(0xA));
            AppendDataFromList(reslist1, namelist);
            tbResult.AppendText("  " + Convert.ToChar(0xA));
            tbResult.AppendText("Second deriv (accel)" + Convert.ToChar(0xA));
            tbResult.AppendText("  " + Convert.ToChar(0xA));
            AppendDataFromList(reslist2, namelist);
        }

        private void AppendDataFromList(List<List<double>> reslist, List<string> names)
        {
            for (int k = 0; k < reslist.Count(); k++)
            {
                string s = names[k] + "  ";
                if (reslist[k] != null)
                {
                    for (int i = 0; i < reslist[k].Count(); i++)
                    {
                        s += String.Format("{0:0.00}", Convert.ToDouble(reslist[k][i])) + "    ";
                    }
                    tbResult.AppendText(s + Convert.ToChar(0xA));
                }
            }
        }

    }
}
