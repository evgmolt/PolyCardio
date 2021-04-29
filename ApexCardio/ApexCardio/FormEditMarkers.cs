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
    public struct SingleData
    {
        public double[] MainData;
        public double[] FirstDerivData;
        public double[] SecDerivData;
        public double[] FirstDerivNorm;
        public double[] SecDerivNorm;
        public string Name;
    }


    public partial class FormEditMarkers : Form
    {
        private struct FieldInfo
        {
            public BufferedPanel BufPanel;
            public string Name;
        }

        List<List<int>> VisirsList;
        List<FieldInfo> FInfoList;
        List<SingleData> Data;
        List<List<double>> ResultListFirst;
        List<List<double>> ResultListSec;
        List<string> NameList;
        private bool MoveVisirNow;
        private int NumOfVisir;
        private bool FocusVisir;
        private int VisirX;
        private int DerivNum; //0 - no, 1 - first, 2 - second
        int ScaleX = 2;

        public FormEditMarkers(List<SingleData> data)
        {
            InitializeComponent();
            Data = data;
/*            foreach (SingleData s in Data)
            {
                DataProcessing.Norm(s.FirstDerivData);
                DataProcessing.Norm(s.SecDerivData);
            }*/
            FInfoList = new List<FieldInfo>();
            VisirsList = new List<List<int>>();
            NameList = new List<string>();
            for (int i = 0; i < Data.Count(); i++)
            {
                VisirsList.Add(new List<int>());
                NameList.Add(Data[i].Name);
            }
            ResultListFirst = new List<List<double>>();
            ResultListSec = new List<List<double>>();
            UpdateGr();
        }

        private double GetAver(double[] data, int start, int stop)
        {
            double sum = 0;
            for (int i = start; i < stop; i++)
            {
                sum = sum + Math.Abs(data[i]);
            }
            return sum / (stop - start);
        }

        private List<double> CountResult(List<int> points, double[] data)
        {
            var res = new List<double>();
            for (int i = 0; i < points.Count() - 1; i++)
            {
                res.Add(GetAver(data, points[i], points[i + 1]));
            }
            return res;
        }

        private void UpdateGr()
        {
            foreach (FieldInfo fi in FInfoList)
            {
                if (fi.BufPanel != null) fi.BufPanel.Dispose();
                panelGraph.Controls.Remove(fi.BufPanel);
            }
            int NumOfFields = Data.Count();
            int space = 1;
            int Y = 0;
            int singleHeight = panelGraph.Height / NumOfFields - space;

            for (int i = 0; i < NumOfFields; i++)
            {
                FieldInfo fi;
                fi.BufPanel = null;
                fi.BufPanel = new BufferedPanel(i);
                fi.BufPanel.Paint += bufPanelEditMarkers_Paint;
                fi.BufPanel.MouseMove += bufpanelMouseMove;
                fi.BufPanel.MouseDown += bufpanelMouseDown;
                fi.BufPanel.MouseUp += bufpanelMouseUp;
                fi.BufPanel.MouseDoubleClick += bufpanelMouseDoubleClick;
                fi.BufPanel.Location = new Point(space, Y + space);
                Y = Y + singleHeight + space;
                fi.BufPanel.Size = new Size(panelGraph.Width - space, singleHeight);
                panelGraph.Controls.Add(fi.BufPanel);
                fi.Name = Data[i].Name;
                FInfoList.Add(fi);
                fi.BufPanel.Refresh();
            }
        }

        private void PaintCurve(Control panel, double[] data, Color color, PaintEventArgs e)
        {
            double Max = -1000000;
            double Min = 1000000;
//            foreach (int elem in data)
            for (int i = 0; i < data.Length; i++)
            {
                Max = Math.Max(Max, data[i]);
                Min = Math.Min(Min, data[i]);
            }
            Max = Max - Min;
            Max = (int)Math.Round(Max * 1.5);
            Max = Max + Max / 10;
            float tension = 0.1F;
            Point[] PaintArray = ViewArrayMaker.MakeArrayForView(panel, data, 0, Max, 1, ScaleX);
            var pen = new Pen(color, 1);
            e.Graphics.DrawCurve(pen, PaintArray, tension);
            pen.Dispose();
        }
        
        private void bufPanelEditMarkers_Paint(object sender, PaintEventArgs e)
        {
            int num = ((BufferedPanel)sender).Number;
            var R0 = e.ClipRectangle;
            var pen0 = new Pen(Color.Black, 1);
            e.Graphics.Clear(Color.White);
            e.Graphics.DrawRectangle(pen0, R0);
            Font f = new Font("Arial", 16);
            SolidBrush b = new SolidBrush(Color.Black);
            e.Graphics.DrawString(Data[num].Name, f, b, new PointF(5,5)); 
            f.Dispose();
            b.Dispose();
                            
            double[] DataArr = Data[num].MainData;
            double[] DerivArr = null;
            switch (DerivNum)
            {
                case 0 : 
                    DerivArr = null;
                    break;
                case 1 : 
                    DerivArr = Data[num].FirstDerivData;
                    break;
                case 2 : 
                    DerivArr = Data[num].SecDerivData;
                    break;
            }
            PaintCurve((BufferedPanel)sender, DataArr, Color.Black, e);
            if (DerivArr != null)
            {
//                if (num == 1)
//                    num = 2;
                PaintCurve((BufferedPanel)sender, DerivArr, Color.Blue, e);
            }
            var pen = new Pen(Color.Red, 1);
            int Y = R0.Height / 2;
            e.Graphics.DrawLine(pen, 0, Y, R0.Width, Y);
            if (Data[num].Name != ApexConstants.ECG1name & Data[num].Name != ApexConstants.ECG2name)
            {
                for (int i = 0; i < VisirsList[num].Count(); i++)
                {
                    e.Graphics.DrawLine(pen, VisirsList[num][i] * ScaleX, 0, VisirsList[num][i] * ScaleX, R0.Height);
                }
            }
            pen.Dispose();
        }

        private void bufpanelMouseDown(object sender, MouseEventArgs e)
        {
            int num = ((BufferedPanel)sender).Number;
            if (FocusVisir)
            {
                MoveVisirNow = true;
                int i1 = VisirsList[num].BinarySearch(e.X / ScaleX);
/*                int i2 = VisirsList[num].BinarySearch(e.X - 1);
                int i3 = VisirsList[num].BinarySearch(e.X + 1);
                if (i1 >= 0)
                {
                    NumOfVisir = i1;
                }
                else
                {
                    if (i2 >= 0)
                    {
                        NumOfVisir = i2;
                    }
                    else
                    {
                        NumOfVisir = i3;
                    }
                }*/
                NumOfVisir = i1;
                if (e.Button == MouseButtons.Left)
                {
                    VisirX = e.X / ScaleX;
                }
                if (e.Button == MouseButtons.Right)
                {
                    VisirsList[num].RemoveAt(NumOfVisir);
                    ((BufferedPanel)sender).Refresh();
                    MoveVisirNow = false;
                    NumOfVisir = -1;
                }
            }
        }

        private void bufpanelMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MoveVisirNow = false;
            }
        }

        private void bufpanelMouseDoubleClick(object sender, MouseEventArgs e)
        {
            int num = ((BufferedPanel)sender).Number;
            if (e.Button == MouseButtons.Left)
            {
                VisirsList[num].Add(e.X / 2);
                VisirsList[num].Sort();
                ((BufferedPanel)sender).Refresh();
            }
        }

        private void bufpanelMouseMove(object sender, MouseEventArgs e)
        {
            int num = ((BufferedPanel)sender).Number;
            if (MoveVisirNow)
            {
                VisirsList[num][NumOfVisir] = e.X / ScaleX;
                ((BufferedPanel)sender).Refresh();
            }
            if (VisirsList[num].Contains(e.X / ScaleX)) //| VisirsList[num].Contains(e.X - 1) | VisirsList[num].Contains(e.X + 1))
            
            {
                this.Cursor = Cursors.VSplit;
                FocusVisir = true;
            }
            else
            {
                this.Cursor = Cursors.Default;
                FocusVisir = false;
            }
        }


        private void FormEditMarkers_Resize(object sender, EventArgs e)
        {
            UpdateGr();
        }

        private void butFirstDeriv_Click(object sender, EventArgs e)
        {
            DerivNum = 1;
            butCalc.Enabled = true;
            butSecDeriv.Enabled = true;
            butFirstDeriv.Enabled = false;
            Refresh();
        }

        private void butSecDeriv_Click(object sender, EventArgs e)
        {
            DerivNum = 2;
            butSecDeriv.Enabled = false;
            for (int i= 0; i < VisirsList.Count(); i++)
            {
                VisirsList[i].Clear();
            }
            Refresh();
        }

        private void butCalc_Click(object sender, EventArgs e)
        {
            double[] data = null;
            List < List < double >> reslist = null;
            for (int i = 0; i < VisirsList.Count(); i++)
            {
                switch (DerivNum)
                {
                    case 0:
                        data = null;
                        break;
                    case 1:
                        reslist = ResultListFirst;
                        data = Data[i].FirstDerivNorm;
                        break;
                    case 2:
                        reslist = ResultListSec;
                        data = Data[i].SecDerivNorm;
                        break;
                }
                if (Data[i].Name != ApexConstants.ECG1name & Data[i].Name != ApexConstants.ECG2name)
                {
                    reslist.Add(CountResult(VisirsList[i], data));
                    NameList.Add(Data[i].Name);
                }
                else
                {
                    reslist.Add(null);
                    NameList.Add(null);
                }
            }
            var fr = new FormResult(ResultListFirst, ResultListSec, NameList);
            fr.ShowDialog();
            fr.Dispose();
            Refresh();
        }
    }
}
