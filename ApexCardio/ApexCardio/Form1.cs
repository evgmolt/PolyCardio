using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ApexCardio
{
    struct GraphicsInfo
    {
        public BufferedPanel BufPanel;
        public bool Visible;
        public Label LabName;
    }
    
    public partial class Form1 : Form, IMessageHandler
    {
        public event Action<Message> WindowsMessage;
        private DataArrays DataA;
        public ApexConfig Cfg;
        USBserialPort USBPort;
        public bool Connected;
        private ByteDecomposer decomposer;
        GraphicsInfo[] GInfoArr;
        TrackBar[] AmpBarArr;
        int[] AmpBarVals;
        private string[] ChannelsNames = { "ECG 1", "ECG 2", "Reogram 1", "Reogram 2", "Sphigmogram 1", "Sphigmogram 2", "Apex cardiogram" };
        private int[] ChannelsMaxSize = { 60000, 60000, 5000, 5000, 40000, 40000, 4000 };
        private int chECG1 = 0;
        private int chECG2 = 1;
        private int chReo1 = 2;
        private int chReo2 = 3;
        private int chSphigmo1 = 4;
        private int chSphigmo2 = 5;
        private int chApex = 6;
        private double[] ChannelsScaleY;
        private int ViewShift = 0;
        private bool ViewMode = false;
        FileStream filestream;
        StreamWriter textWriter;
        private string CurrentFile;
        private RecordInfo CurrentRecordInfo;
        private bool DataReadyToSave = false;
        private Archiver7z Archiver;
        private int DelayCounter;
        private ApexVisirs V;
//        private bool PaintAver = false;
    
        public Form1()
        {
            InitializeComponent();
            GInfoArr = new GraphicsInfo[ApexConstants.NumOfChannels];
            AmpBarArr = new TrackBar[4];
            AmpBarVals = new int[4];
            ChannelsScaleY = new double[4];
            for (int i = 0; i < 4; i++)
            {
                ChannelsScaleY[i] = 1;
                AmpBarVals[i] = 0;
            }
            USBPort = new USBserialPort(this, 115200);
            USBPort.ConnectionFailure += onConnectionFailure;
            USBPort.Connect();

            Cfg = ApexConfig.GetConfig();
            try
            {
                Archiver = new Archiver7z(Cfg.ArchiverPath + @"7z.exe");
            }
            catch (Exception ex)
            {
                //                MessageBox.Show(ex.Message);
            }

            numUDplevel1.Value = Cfg.PressLevel1;
            numUDplevel2.Value = Cfg.PressLevel2;

            InitArraysForFlow();
            CheckDataDir();

            if (Cfg.Maximized) WindowState = FormWindowState.Maximized;
            else
            {
                WindowState = FormWindowState.Normal;
                Width = Cfg.WindowWidth;
                Height = Cfg.WindowHeight;
            }


            for (int i = 0; i < ApexConstants.NumOfChannels; i++)
            {
                GInfoArr[i].Visible = Cfg.VisibleGraphs[i];
            }
            V = new ApexVisirs();
            UpdateGraphics();
        }

        private void InitDevice()
        {
            USBPort.WriteByte(ApexConstants.cmSetPressLevel1);
            USBPort.WriteByte(Cfg.PressLevel1);
            USBPort.WriteByte(ApexConstants.cmSetPressLevel2);
            USBPort.WriteByte(Cfg.PressLevel2);
        }

        private void InitArraysForFlow()
        {
            DataA = new DataArrays(ByteDecomposer.DataArrSize);
            decomposer = new ByteDecomposer(DataA);
            decomposer.ConnectionBreakdown += ConnectionBreak;
            decomposer.DecomposeLineEvent += LineReceived;
            decomposer.BPcompleted += BPcomplete;
        }

        private void CheckDataDir()
        {
            openFileDialog1.InitialDirectory = Cfg.DataDir;
            DirectoryInfo datadir = new DirectoryInfo(Cfg.DataDir);
            if (!datadir.Exists) datadir.Create();
        }

        void ConnectionBreak(object sender, EventArgs arg)
        {
        }

        void LineReceived(object sender, EventArgs arg)
        {
            labP1.Text = (DataA.Reo1Array[decomposer.MainIndex]).ToString();
            labP2.Text = (DataA.Reo2Array[decomposer.MainIndex]).ToString();
            labP3.Text = (DataA.ECG1Array[decomposer.MainIndex]).ToString();

//            labP1.Text = (DataA.Sphigmo1Array[decomposer.MainIndex] / k).ToString();
//            labP2.Text = (DataA.Sphigmo2Array[decomposer.MainIndex] / k).ToString();
//            labP3.Text = (DataA.ApexArray[decomposer.MainIndex] / k).ToString();
            labStatus.Text = Convert.ToString(decomposer.Status, 2);
            if ((decomposer.Status & ByteDecomposer.bp1error) == 0)
                labBP1.Text = decomposer.PrSYS1.ToString() + " / " + decomposer.PrDIA1.ToString();
            if ((decomposer.Status & ByteDecomposer.bp2error) == 0)
                labBP2.Text = decomposer.PrSYS2.ToString() + " / " + decomposer.PrDIA2.ToString();

        }

        void NewLineReceived(object sender, EventArgs agr)
        {
//            if (decomposer.LineCounter % ByteDecomposer.SamplingFrequency != 0) return;
            int lc = decomposer.LineCounter / ByteDecomposer.SamplingFrequency;
            labRecordSize.Text = String.Concat("Record size : ",
                                               lc.ToString(),
                                               " sec");
            if (decomposer.LineCounter > (Cfg.RecordLength * ByteDecomposer.SamplingFrequency))
            {
                StopRecord();
                return;
            }
            pbRecordProgress.Value = decomposer.LineCounter;
        }


        private void BPcomplete(object sender, BPEventArgs arg)
        {
            string mess;
            if ((arg.status & ByteDecomposer.bp1completed) != 0)
            {
                mess = "BP 1 " + decomposer.PrSYS1.ToString() + "/" +
                                 decomposer.PrDIA1.ToString();
                labBP1.Text = decomposer.PrSYS1.ToString() + " / "+ decomposer.PrDIA1.ToString();
                if (CurrentRecordInfo != null)
                {
                    CurrentRecordInfo.Data.ABP[0] = decomposer.PrSYS1;
                    CurrentRecordInfo.Data.ABP[1] = decomposer.PrDIA1;
                    CurrentRecordInfo.Save(Cfg);
                    UpdateRecInfoBox(CurrentRecordInfo);
                }
            }
            if ((arg.status & ByteDecomposer.bp1error) != 0)
            {
                mess = "BP 1 error";
                labBP1.Text = "Error";
            }
            if ((arg.status & ByteDecomposer.bp2completed) != 0)
            {
                mess = "BP 2 " + decomposer.PrSYS2.ToString() + "/" +
                                 decomposer.PrDIA2.ToString();
                labBP2.Text = decomposer.PrSYS2.ToString() + " / " + decomposer.PrDIA2.ToString();
                if (CurrentRecordInfo != null)
                {
                    CurrentRecordInfo.Data.ABP[0] = decomposer.PrSYS2;
                    CurrentRecordInfo.Data.ABP[1] = decomposer.PrDIA2;
                    CurrentRecordInfo.Save(Cfg);
                    UpdateRecInfoBox(CurrentRecordInfo);
                }

            }
            if ((arg.status & ByteDecomposer.bp2error) != 0)
            {
                mess = "BP 2 error";
                labBP2.Text = "Error";
            }
        }



        public void UpdateGraphics()
        {
            for (int i = 0; i < ApexConstants.NumOfChannels; i++)
            {
                GInfoArr[i].Visible = Cfg.VisibleGraphs[i];
            }
            int num = 0;
            foreach (GraphicsInfo gi in GInfoArr)
            {
                if (gi.Visible) num++;
                if (gi.BufPanel != null) gi.BufPanel.Dispose();
                if (gi.LabName != null) gi.LabName.Dispose();
                panelGraph.Controls.Remove(gi.BufPanel);
                panelGraph.Controls.Remove(gi.LabName);
            }
            if (num == 0) return;
            int space = 14;
            int Y = 0;
            int singleHeight = panelGraph.Height / num - space;
            for (int i = 0; i < ApexConstants.NumOfChannels; i++)
            {
                GraphicsInfo gi = GInfoArr[i];
                if (gi.Visible)
                {
                    gi.LabName = new Label();
                    gi.LabName.Location = new Point(space, Y);
                    gi.LabName.Text = ChannelsNames[i]; 
                    gi.BufPanel = new BufferedPanel(i);
                    gi.BufPanel.MouseDown += BufPanel_MouseDown;
                    gi.BufPanel.Cursor = Cursors.Cross;
                    gi.BufPanel.Location = new Point(space, Y + space);
                    Y = Y + singleHeight + space;
                    gi.BufPanel.Size = new Size(panelGraph.Width - space, singleHeight);
                    if (i == 0) gi.BufPanel.Paint += bufferedPanel0_Paint;
                    if (i == 1) gi.BufPanel.Paint += bufferedPanel1_Paint;
                    if (i == 2) gi.BufPanel.Paint += bufferedPanel2_Paint;
                    if (i == 3) gi.BufPanel.Paint += bufferedPanel3_Paint;
                    if (i == 4) gi.BufPanel.Paint += bufferedPanel4_Paint;
                    if (i == 5) gi.BufPanel.Paint += bufferedPanel5_Paint;
                    if (i == 6) gi.BufPanel.Paint += bufferedPanel6_Paint;
                    panelGraph.Controls.Add(gi.BufPanel);
                    panelGraph.Controls.Add(gi.LabName);
                    GInfoArr[i] = gi;
                    gi.BufPanel.Refresh();
                }
                else
                {
                    gi.BufPanel = null;
                }
            }
            UpdateAmpBars(space, singleHeight);
        }


        private void UpdateAmpBars(int space, int singleHeight)
        {
            for (int i = 0; i < AmpBarArr.Count(); i++)
            {
                if (AmpBarArr[i] != null)
                {
                    AmpBarArr[i].Dispose();
                    panelAmp.Controls.Remove(AmpBarArr[i]);
                    AmpBarArr[i] = null;
                }
            }
            if (GInfoArr[chECG1].Visible | GInfoArr[chECG2].Visible)
            {
                AmpBarArr[0] = new TrackBar();
                AmpBarArr[0].ValueChanged += trackBarECG_ValueChanged;
            }
            if (GInfoArr[chReo1].Visible | GInfoArr[chReo2].Visible)
            {
                AmpBarArr[1] = new TrackBar();
                AmpBarArr[1].ValueChanged += trackBarReo_ValueChanged;
            }
            if (GInfoArr[chSphigmo1].Visible | GInfoArr[chSphigmo2].Visible)
            {
                AmpBarArr[2] = new TrackBar();
                AmpBarArr[2].ValueChanged += trackBarSphigmo_ValueChanged;
            }
            if (GInfoArr[chApex].Visible)
            {
                AmpBarArr[3] = new TrackBar();
                AmpBarArr[3].ValueChanged += trackBarApex_ValueChanged;
            }

            for (int i = 0; i < 4; i++)
            {
                if (AmpBarArr[i] != null)
                {
                    AmpBarArr[i].Minimum = -5;
                    AmpBarArr[i].Maximum = 5;
                    AmpBarArr[i].Orientation = Orientation.Vertical;
                    AmpBarArr[i].Value = AmpBarVals[i];
                    AmpBarArr[i].LargeChange = 1;
                }
            }
            if (AmpBarArr[0] != null)
            {
                if ((GInfoArr[chECG1].Visible ^ GInfoArr[chECG2].Visible))
                {
                    AmpBarArr[0].Size = new Size(45, singleHeight);
                }
                else
                {
                    AmpBarArr[0].Size = new Size(45, singleHeight * 2);
                }
            }
            if (AmpBarArr[1] != null)
            {
                if ((GInfoArr[chReo1].Visible ^ GInfoArr[chReo2].Visible))
                {
                    AmpBarArr[1].Size = new Size(45, singleHeight);
                }
                else
                {
                    AmpBarArr[1].Size = new Size(45, singleHeight * 2);
                }
            }
            if (AmpBarArr[2] != null)
            {
                if ((GInfoArr[chSphigmo1].Visible ^ GInfoArr[chSphigmo2].Visible))
                {
                    AmpBarArr[2].Size = new Size(45, singleHeight);
                }
                else
                {
                    AmpBarArr[2].Size = new Size(45, singleHeight * 2);
                }
            }
            if (AmpBarArr[3] != null)
            {
                AmpBarArr[3].Size = new Size(45, singleHeight);
            }
            int Y = space;
            for (int i = 0; i < AmpBarArr.Count(); i++)
            {
                if (AmpBarArr[i] != null)
                {
                    int a;
                    if (AmpBarArr[i].Height == singleHeight)
                        a = space;
                    else
                        a = space * 2;
                    AmpBarArr[i].Location = new Point(0, Y);
                    Y = Y + AmpBarArr[i].Height + a;
                    panelAmp.Controls.Add(AmpBarArr[i]);
                }
            }
        }


        private void buffPanel_Paint(int[] data, int[] derivdata, Control panel, double ScaleY, int MaxSize, PaintEventArgs e)
        {
//            if (decomposer.MainIndex < 2 & !ViewMode) return;
            float tension = 0.1F;
            var R0 = new Rectangle(0, 0, panel.Width - 1, panel.Height - 1);
            var pen0 = new Pen(Color.Black, 1);
            e.Graphics.Clear(Color.White);
            e.Graphics.DrawRectangle(pen0, R0);
            pen0.Dispose();
            Point[] OutArray;
            if (!ViewMode)
            {
                OutArray = ViewArrayMaker.MakeArray(panel, data, decomposer.MainIndex, MaxSize, ScaleY);
            }
            else
            {
                OutArray = ViewArrayMaker.MakeArrayForView(panel, data, ViewShift, MaxSize, ScaleY);
            }
            var pen = new Pen(Color.Red, 1);
            e.Graphics.DrawCurve(pen, OutArray, tension);
            pen.Dispose();
            if (derivdata != null)
            {
                Point[] DerivArray = ViewArrayMaker.MakeArrayForView(panel, derivdata, ViewShift, MaxSize, ScaleY);
                var penD = new Pen(Color.Black, 1);
                e.Graphics.DrawCurve(penD, DerivArray, tension);
                penD.Dispose();
            }

            var pen1 = new Pen(Color.Blue, 1);
            if (V.Visir1.Visible)
                e.Graphics.DrawLine(pen1, V.Visir1.X - ViewShift, R0.Top, V.Visir1.X - ViewShift, R0.Height);
            if (V.Visir2.Visible)
                e.Graphics.DrawLine(pen1, V.Visir2.X - ViewShift, R0.Top, V.Visir2.X - ViewShift, R0.Height);
            pen1.Dispose();

            if (data == DataA.ECG1ViewArray)
            
                if (V.Visir1.Visible & V.Visir2.Visible)
                {
                    var penR = new Pen(Color.Green, 1);
                    for (int i = 0; i < DataA.RInd; i++)
                    {
                        e.Graphics.DrawLine(penR, DataA.RArray[i] - ViewShift, R0.Top, DataA.RArray[i] - ViewShift, R0.Height);
                    }
                    penR.Dispose();
                }
        }

        private void bufferedPanel0_Paint(object sender, PaintEventArgs e)
        {
                buffPanel_Paint(DataA.ECG1ViewArray, null, GInfoArr[0].BufPanel, ChannelsScaleY[0], ChannelsMaxSize[0], e);
        }
        private void bufferedPanel1_Paint(object sender, PaintEventArgs e)
        {
                buffPanel_Paint(DataA.ECG2ViewArray, null, GInfoArr[1].BufPanel, ChannelsScaleY[0], ChannelsMaxSize[1], e);
        }
        private void bufferedPanel2_Paint(object sender, PaintEventArgs e)
        {
                buffPanel_Paint(DataA.Reo1ViewArray, null, GInfoArr[2].BufPanel, ChannelsScaleY[1], ChannelsMaxSize[2], e);
        }
        private void bufferedPanel3_Paint(object sender, PaintEventArgs e)
        {
                buffPanel_Paint(DataA.Reo2ViewArray, null, GInfoArr[3].BufPanel, ChannelsScaleY[1], ChannelsMaxSize[3], e);
        }
        private void bufferedPanel4_Paint(object sender, PaintEventArgs e)
        {
                buffPanel_Paint(DataA.Sphigmo1ViewArray, null, GInfoArr[4].BufPanel, ChannelsScaleY[2], ChannelsMaxSize[4], e);
        }
        private void bufferedPanel5_Paint(object sender, PaintEventArgs e)
        {
                buffPanel_Paint(DataA.Sphigmo2ViewArray, null, GInfoArr[5].BufPanel, ChannelsScaleY[2], ChannelsMaxSize[5], e);
        }
        private void bufferedPanel6_Paint(object sender, PaintEventArgs e)
        {
                buffPanel_Paint(DataA.ApexViewArray, null, GInfoArr[6].BufPanel, ChannelsScaleY[3], ChannelsMaxSize[6], e);
        }


        private void onConnectionFailure(Exception obj)
        {
            string messageText = ApexStrings.txErrorInitPort;
            string caption = ApexStrings.txError;
            MessageBoxButtons but = MessageBoxButtons.OK;
            MessageBoxIcon icon = MessageBoxIcon.Error;
            MessageBox.Show(messageText, caption, but, icon);
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_DEVICECHANGE = 0x0219;
            if (WindowsMessage != null)
            {
                if (m.Msg == WM_DEVICECHANGE) WindowsMessage(m);
            }
            base.WndProc(ref m);
        }


        private void panelGraph_Resize(object sender, EventArgs e)
        {
            UpdateGraphics();
/*            if (ViewMode & decomposer != null)
            {
                UpdateScrollBar(decomposer.Data.ApexArray.Length);
            }
            else
            {
                hScrollBar1.Visible = false;
            }*/
        }

        private void butSettings_Click(object sender, EventArgs e)
        {
            var fc = new FormConfig(Cfg);
            if (fc.ShowDialog() == DialogResult.OK)
            {
                Cfg = fc.GetDialogData();
                ApexConfig.SaveConfig(Cfg);
                UpdateGraphics();
            }
            fc.Dispose();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (decomposer != null)
            {
                if (Connected & decomposer.DeviceTurnedOn)
                {
                    USBPort.WriteByte(ApexConstants.cmStopPump1);
                    USBPort.WriteByte(ApexConstants.cmStopPump2);
                }
            }
            if (WindowState == FormWindowState.Maximized) Cfg.Maximized = true;
            else Cfg.Maximized = false;
            Cfg.WindowWidth = Width;
            Cfg.WindowHeight = Height;
            ApexConfig.SaveConfig(Cfg);
        }

        private void timerPaint_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < ApexConstants.NumOfChannels; i++)
            {
                if (GInfoArr[i].Visible)
                {
                    GInfoArr[i].BufPanel.Refresh();
                }
            }
        }

        private void timerRead_Tick(object sender, EventArgs e)
        {
            if (USBPort == null) return;
            if (USBPort.PortHandle == null) return;
            if (!USBPort.PortHandle.IsOpen) return;
            if (decomposer != null)
            {
                decomposer.Decompos(USBPort, filestream, textWriter, Cfg).ToString();
            }
        }

        private void timerStatus_Tick(object sender, EventArgs e)
        {
            if (ViewMode)
            {
                butFlow.Text = "Start stream";
            }
            else
            {
                butFlow.Text = "Stop stream";
            }

            butSaveRecord.Enabled = DataReadyToSave;

            if (decomposer != null)
            {
                butNewRecord.Enabled = Connected & decomposer.DeviceTurnedOn & !decomposer.RecordStarted & !decomposer.BP1measStarted;
                butStartRecord.Enabled = Connected & !decomposer.RecordStarted & !ViewMode & decomposer.DeviceTurnedOn & CurrentRecordInfo != null & !decomposer.BP1measStarted;
                butFlow.Enabled = Connected & decomposer.DeviceTurnedOn & !decomposer.RecordStarted;
                butEditRecInfo.Enabled = CurrentRecordInfo != null & !decomposer.RecordStarted;
                butBPMeas1.Enabled = Connected & decomposer.DeviceTurnedOn & !decomposer.RecordStarted & !decomposer.BP1measStarted;
                butBPabort1.Enabled = Connected & decomposer.DeviceTurnedOn & !decomposer.RecordStarted & decomposer.BP1measStarted;
                butBPmeas2.Enabled = Connected & decomposer.DeviceTurnedOn & !decomposer.RecordStarted & !decomposer.BP2measStarted;
                butBPabort2.Enabled = Connected & decomposer.DeviceTurnedOn & !decomposer.RecordStarted & decomposer.BP2measStarted;
                butPump1start.Enabled = Connected & decomposer.DeviceTurnedOn & !decomposer.Pump1Started;
                butPump2start.Enabled = Connected & decomposer.DeviceTurnedOn & !decomposer.Pump2Started; ;
                butPump1stop.Enabled = Connected & decomposer.DeviceTurnedOn;
                butPump2stop.Enabled = Connected & decomposer.DeviceTurnedOn;
                butPumpsStart.Enabled = Connected & decomposer.DeviceTurnedOn & (!decomposer.Pump1Started | !decomposer.Pump2Started);
                butPumpsStop.Enabled = Connected & decomposer.DeviceTurnedOn;
            }
            else
            {
                butFlow.Enabled = false;
                butEditRecInfo.Enabled = CurrentRecordInfo != null;
                butBPMeas1.Enabled = CurrentRecordInfo != null;
                butBPMeas1.Enabled = false;
                butBPabort1.Enabled = false;
            }

            if (USBPort == null)
            {
                labConnected.Text = "Disconnected";
                Connected = false;
                return;
            }
            if (USBPort.PortHandle == null)
            {
                labConnected.Text = "Disconnected";
                Connected = false;
                return;
            }
            if (USBPort.PortHandle.IsOpen)
            {
                string s = "Connected to " + USBPort.PortNames[USBPort.CurrentPort];
                if (decomposer != null)
                    if (!decomposer.DeviceTurnedOn)
                    {
                        s = s + "    Device turned off";
                        labConnected.ForeColor = System.Drawing.Color.Red;
                        labConnected.Font = new Font(this.Font, FontStyle.Bold);
                    }
                    else
                    {
                        labConnected.ForeColor = System.Drawing.Color.Black;
                        labConnected.Font = new Font(this.Font, FontStyle.Regular);
                    }
                labConnected.Text = s;
                Connected = true;
            }
            else
            {
                labConnected.Text = "Disconnected";
                Connected = false;
            }

        }

        private void UpdateRecInfoBox(RecordInfo riarg)
        {
            textBoxRecInfo.Clear();
//            textBoxRecInfo.Text += "Record information" + System.Environment.NewLine + System.Environment.NewLine;
            textBoxRecInfo.Text += "File : " + Convert.ToChar(9) + Path.GetFileName(riarg.FileName) + System.Environment.NewLine;
            string[] lines = riarg.GetRecInfo(true);
/*            foreach (string line in lines)
            {
                textBoxRecInfo.Text += line + System.Environment.NewLine;
            }*/
            for (int i = 0; i < lines.Length; i++)
            {
                string s;
                if (i < ApexConstants.PatientFieldsCount)
                {
                    s = lines[i] + System.Environment.NewLine;
                    textBoxRecInfo.Text += s;
                }
                else
                    if (riarg.Data.DiagnArray[i - ApexConstants.PatientFieldsCount])
                    {
                        s = riarg.Data.ItemNames[i] + " ";
                        textBoxRecInfo.Text += s;
                    }
            }
            textBoxRecInfo.SelectionStart = 0;
        }

        private void butNewRecord_Click(object sender, EventArgs e)
        {
//            CurrentFile = Cfg.DataFileNum.ToString("D4");
            saveFileDialog1.FileName = "";
            saveFileDialog1.InitialDirectory = Cfg.DataDir.ToString();
            saveFileDialog1.Filter = "Text files | *.txt|All files | *.*";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Cfg.DataDir = Path.GetDirectoryName(saveFileDialog1.FileName) + @"\";
                ApexConfig.SaveConfig(Cfg);
                CurrentFile = Path.GetFileName(saveFileDialog1.FileName);
                var ri = new RecordInfo(Cfg, CurrentFile);
                ri.Data.Date = DateTime.Now;
                ri.Save(Cfg);
                UpdateRecInfoBox(ri);
                CurrentRecordInfo = ri;
            }
        }

        private void StartRecord()
        {
            CurrentRecordInfo.Data.Date = DateTime.Now;
            CurrentRecordInfo.Save(Cfg);
            UpdateRecInfoBox(CurrentRecordInfo);

            pbRecordProgress.Maximum = Cfg.RecordLength * ByteDecomposer.SamplingFrequency;
            //            string fname = String.Concat(Cfg.DataDir.ToString(), CurrentFile, ApexConstants.DataFileExtension);
            //            filestream = new FileStream(fname, FileMode.Create);
            textWriter = new StreamWriter(Cfg.DataDir.ToString() + ApexConstants.TmpTextFile);
            decomposer.TotalBytes = 0;
            decomposer.LineCounter = 0;
            decomposer.DecomposeLineEvent += NewLineReceived;
            decomposer.RecordStarted = true;
        }

        private void butStartRecord_Click(object sender, EventArgs e)
        {
//            timerDelay.Interval = Cfg.StartDelay * 1000;
            timerDelay.Enabled = true;
            DelayCounter = Cfg.StartDelay;
            labRecordSize.Text = "Start delay : " + DelayCounter.ToString() + " sec";
            labRecordSize.Visible = true;
            pbRecordProgress.Visible = true;
            pbRecordProgress.Value = 0;
            pbRecordProgress.Maximum = Cfg.StartDelay;
        }

        private void timerDelay_Tick(object sender, EventArgs e)
        {
            if (DelayCounter != 0)
            {
                DelayCounter--;
                pbRecordProgress.Value++;
                labRecordSize.Text = "Start delay : " + DelayCounter.ToString() + " sec";
            }
            if (DelayCounter == 0)
            {
                StartRecord();
                timerDelay.Enabled = false;
            }
        }

        private void UpdateScrollBar(int size)
        {
            int space = 14;
            hScrollBar1.Maximum = size;
            hScrollBar1.LargeChange = panelGraph.Width - space - 50;
            hScrollBar1.SmallChange = panelGraph.Width - space / 10;
            hScrollBar1.AutoSize = true;
            hScrollBar1.Value = 0;
            hScrollBar1.Visible = hScrollBar1.Maximum > hScrollBar1.Width;
        }

        private void StopRecord()
        {
            V.ResetVisirs();
            pbRecordProgress.Visible = false;
            labRecordSize.Visible = false;
            CurrentRecordInfo.Data.Length = decomposer.TotalBytes / ByteDecomposer.BytesInBlock / ByteDecomposer.SamplingFrequency;
            CurrentRecordInfo.Save(Cfg);
            UpdateRecInfoBox(CurrentRecordInfo);
            decomposer.DecomposeLineEvent -= NewLineReceived;
            decomposer.DecomposeLineEvent -= LineReceived;
//            String fname = filestream.Name;
            decomposer.RecordStarted = false;
            decomposer = null;
//            filestream.Close();
//            filestream.Dispose();
            if (textWriter != null) textWriter.Dispose();
            string[] lines = File.ReadAllLines(Cfg.DataDir.ToString() + ApexConstants.TmpTextFile);
            File.AppendAllLines(CurrentRecordInfo.FileName, lines);

            ViewMode = true;
            timerRead.Enabled = false;
//            timerPaint.Enabled = false;

            DataA = new DataArrays(lines.Length);
            decomposer = ParseData(lines, DataA, 0);
            decomposer.CountViewArrays(lines.Length, Cfg.FilterOn);
            SetScale(lines.Length);
            for (int i = 0; i < AmpBarVals.Length; i++)
            {
                AmpBarVals[i] = 0;
            }
            for (int i = 0; i < ChannelsScaleY.Length; i++)
            {
                ChannelsScaleY[i] = 1;
            }
            for (int i = 0; i < AmpBarArr.Length; i++)
            {
                AmpBarArr[i].Value = 0;
            }
            UpdateScrollBar(lines.Length);
            foreach (GraphicsInfo gi in GInfoArr)
            {
                if (gi.Visible) gi.BufPanel.Refresh();
            }
            DataReadyToSave = true;
        }

        private void butSaveRecord_Click(object sender, EventArgs e)
        {
            if (Archiver != null)
            {
                Archiver.AddToArchive(Cfg.DataDir.ToString() + CurrentFile + ApexConstants.ArchiverFileExtension,
                                      Cfg.DataDir.ToString() + CurrentFile);
            }
            Cfg.DataFileNum++;
            ApexConfig.SaveConfig(Cfg);
            DataReadyToSave = false;
            CurrentRecordInfo = null;
            textBoxRecInfo.Clear();
        }

        

        private void EditRecInfo()
        {
            if (decomposer == null) return;
            if (decomposer.RecordStarted)
            {
                if (textBoxRecInfo.Text.Length == 0) return;
                textBoxRecInfo.SelectionStart = 0;
                var ri = new RecordInfo(Cfg, CurrentFile);
                var fri = new FormRecordInfo(ri);
                if (fri.ShowDialog() == DialogResult.OK)
                {
                    ri = fri.GetDialogData();
                    ri.Save(Cfg);
                    UpdateRecInfoBox(ri);
                }
                fri.Dispose();
            }
            else
            {
                string[] lines = File.ReadAllLines(Cfg.DataDir.ToString() + CurrentFile, Encoding.Default);
                string[] ldata = lines.Skip(ApexConstants.HeaderSize).ToArray();
                string[] lrec = lines.Take(ApexConstants.HeaderSize).ToArray();
                var ri = new RecordInfo(Cfg, CurrentFile, lrec);
                var fri = new FormRecordInfo(ri);
                if (fri.ShowDialog() == DialogResult.OK)
                {
                    ri = fri.GetDialogData();
                    ri.Save(Cfg);
                    UpdateRecInfoBox(ri);
                }
                CurrentRecordInfo = ri;
                File.AppendAllLines(ri.FileName, ldata);
                fri.Dispose();
            }
        }

        private void trackBarECG_ValueChanged(object sender, EventArgs e)
        {
            AmpBarVals[0] = AmpBarArr[0].Value;
            double a = AmpBarArr[0].Value;
            ChannelsScaleY[0] = Math.Pow(2, a / 2);
            if (ViewMode)
            {
                if (GInfoArr[chECG1].Visible) GInfoArr[chECG1].BufPanel.Refresh();
                if (GInfoArr[chECG2].Visible) GInfoArr[chECG2].BufPanel.Refresh();
            }
        }

        private void trackBarReo_ValueChanged(object sender, EventArgs e)
        {
            AmpBarVals[1] = AmpBarArr[1].Value;
            double a = AmpBarArr[1].Value;
            ChannelsScaleY[1] = Math.Pow(2, a / 2);
            if (ViewMode)
            {
                if (GInfoArr[chReo1].Visible) GInfoArr[chReo1].BufPanel.Refresh();
                if (GInfoArr[chReo2].Visible) GInfoArr[chReo2].BufPanel.Refresh();
            }
        }

        private void trackBarSphigmo_ValueChanged(object sender, EventArgs e)
        {
            AmpBarVals[2] = AmpBarArr[2].Value;
            double a = AmpBarArr[2].Value;
            ChannelsScaleY[2] = Math.Pow(2, a / 2);
            if (ViewMode)
            {
                if (GInfoArr[chSphigmo1].Visible) GInfoArr[chSphigmo1].BufPanel.Refresh();
                if (GInfoArr[chSphigmo2].Visible) GInfoArr[chSphigmo2].BufPanel.Refresh();
            }
        }

        private void trackBarApex_ValueChanged(object sender, EventArgs e)
        {
            AmpBarVals[3] = AmpBarArr[3].Value;
            double a = AmpBarArr[3].Value;
            ChannelsScaleY[3] = Math.Pow(2, a / 2);
            if (ViewMode)
            {
                if (GInfoArr[chApex].Visible) GInfoArr[chApex].BufPanel.Refresh();
            }
        }

        private ByteDecomposer ParseData(string[] lines, DataArrays a, int skip)
        {
            for (int i = skip; i < lines.Length; i++)
            {
                string[] s = lines[i].Split(Convert.ToChar(9));
                a.ECG1Array[i - skip] = Convert.ToInt32(s[1]);
                a.ECG2Array[i - skip] = Convert.ToInt32(s[2]);
                a.Reo1Array[i - skip] = Convert.ToInt32(s[3]);
                a.Reo2Array[i - skip] = Convert.ToInt32(s[4]);
                a.Sphigmo1Array[i - skip] = Convert.ToInt32(s[5]);
                a.Sphigmo2Array[i - skip] = Convert.ToInt32(s[6]);
                a.ApexArray[i - skip] = Convert.ToInt32(s[7]);
            }
            var d = new ByteDecomposer(a);
            d.MainIndex = (uint)lines.Length;
            return d;
        }

        private void butOpenFile_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.InitialDirectory = Cfg.DataDir.ToString();
            textBoxRecInfo.SelectionStart = 0;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(openFileDialog1.FileName))
                {
                    V.ResetVisirs();
                    Cfg.DataDir = Path.GetDirectoryName(openFileDialog1.FileName) + @"\";
                    ApexConfig.SaveConfig(Cfg);
                    ViewMode = true;
                    ViewShift = 0;
                    timerRead.Enabled = false;

                    CurrentFile = Path.GetFileName(openFileDialog1.FileName);
                    var ri = new RecordInfo(Cfg, CurrentFile);
                    UpdateRecInfoBox(ri);
                    CurrentRecordInfo = ri;

                    string[] lines = File.ReadAllLines(openFileDialog1.FileName);
                    int size = lines.Count() - ApexConstants.HeaderSize;
                    UpdateScrollBar(size);
                    
                    if (size == 0)
                    {
                        MessageBox.Show("Error reading file " + openFileDialog1.FileName);
                        return;
                    }

                    DataA = new DataArrays(size+1000);
                    decomposer = ParseData(lines, DataA, ApexConstants.HeaderSize);
                    decomposer.CountViewArrays(lines.Length - ApexConstants.HeaderSize, Cfg.FilterOn);
                    SetScale(size);
                    foreach (GraphicsInfo gi in GInfoArr)
                    {
                        if (gi.Visible) gi.BufPanel.Refresh();
                    }
                    textBoxRecInfo.SelectionStart = 0;
                    
                }
            }

        }

        private void SetScale(int size)
        {
            double coeff = 1.5;
            int r1 = 0;
            int r2 = 0;
            if (GInfoArr[0].Visible)
            {
                r1 = DataProcessing.GetRange(DataA.ECG1ViewArray, size);
            }
            if (GInfoArr[1].Visible)
            {
                r2 = DataProcessing.GetRange(DataA.ECG2ViewArray, size);
            }
            ChannelsMaxSize[0] = (int)(coeff * Math.Max(r1, r2));
            ChannelsMaxSize[1] = (int)(coeff * Math.Max(r1, r2));
            r1 = 0;
            r2 = 0;
            if (GInfoArr[2].Visible)
            {
                r1 = DataProcessing.GetRange(DataA.Reo1ViewArray, size);
            }
            if (GInfoArr[3].Visible)
            {
                r2 = DataProcessing.GetRange(DataA.Reo2ViewArray, size);
            }
            ChannelsMaxSize[2] = (int)(coeff * Math.Max(r1, r2));
            ChannelsMaxSize[3] = (int)(coeff * Math.Max(r1, r2));
            if (GInfoArr[4].Visible)
            {
                r1 = DataProcessing.GetRange(DataA.Sphigmo1ViewArray, size);
            }
            if (GInfoArr[5].Visible)
            {
                r2 = DataProcessing.GetRange(DataA.Sphigmo2ViewArray, size);
            }
            ChannelsMaxSize[4] = (int)(coeff * Math.Max(r1, r2));
            ChannelsMaxSize[5] = (int)(coeff * Math.Max(r1, r2));
            ChannelsMaxSize[6] = (int)(DataProcessing.GetRange(DataA.ApexViewArray, size) * coeff);
        }

        private void butFlow_Click(object sender, EventArgs e)
        {
            DataReadyToSave = false;
            ViewMode = !ViewMode;
            timerRead.Enabled = !ViewMode;
            timerPaint.Enabled = !ViewMode;
            if (!ViewMode)
            {
                butFlow.Text = "Stop stream";
                InitArraysForFlow();
//                textBoxRecInfo.Clear();
                labRecordSize.Visible = false;
                hScrollBar1.Visible = false;
            }
        }

        private void hScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            ViewShift = hScrollBar1.Value;
        }

        private void butEditRecInfo_Click(object sender, EventArgs e)
        {
            EditRecInfo();
        }

 
        private void numUDplevel1_ValueChanged(object sender, EventArgs e)
        {
            Cfg.PressLevel1 = (byte)numUDplevel1.Value;
            USBPort.WriteByte(ApexConstants.cmSetPressLevel1);
            USBPort.WriteByte(Cfg.PressLevel1);
        }

        private void numUDplevel2_ValueChanged(object sender, EventArgs e)
        {
            Cfg.PressLevel2 = (byte)numUDplevel2.Value;
            USBPort.WriteByte(ApexConstants.cmSetPressLevel2);
            USBPort.WriteByte(Cfg.PressLevel2);
        }

        private void butPumpsStart_Click(object sender, EventArgs e)
        {
            USBPort.WriteByte(ApexConstants.cmSetPressLevel1);
            USBPort.WriteByte(Cfg.PressLevel1);
            USBPort.WriteByte(ApexConstants.cmSetPressLevel2);
            USBPort.WriteByte(Cfg.PressLevel2);
            USBPort.WriteByte(ApexConstants.cmStartPump1);
            USBPort.WriteByte(ApexConstants.cmStartPump2);
            if (decomposer != null)
            {
                decomposer.Pump1Started = true;
                decomposer.Pump2Started = true;
            }
        }

        private void butPump1start_Click(object sender, EventArgs e)
        {
            USBPort.WriteByte(ApexConstants.cmSetPressLevel1);
            USBPort.WriteByte(Cfg.PressLevel1);
            USBPort.WriteByte(ApexConstants.cmStartPump1);
            if (decomposer != null)
            {
                decomposer.Pump1Started = true;
            }
        }

        private void butPump2start_Click(object sender, EventArgs e)
        {
            USBPort.WriteByte(ApexConstants.cmSetPressLevel2);
            USBPort.WriteByte(Cfg.PressLevel2);
            USBPort.WriteByte(ApexConstants.cmStartPump2);
            if (decomposer != null)
            {
                decomposer.Pump2Started = true;
            }
        }

        private void butPumpsStop_Click(object sender, EventArgs e)
        {
            USBPort.WriteByte(ApexConstants.cmStopPump1);
            USBPort.WriteByte(ApexConstants.cmStopPump2);
            if (decomposer != null)
            {
                decomposer.Pump1Started = false;
                decomposer.Pump2Started = false;
            }
        }

        private void butPump1stop_Click(object sender, EventArgs e)
        {
            USBPort.WriteByte(ApexConstants.cmStopPump1);
            if (decomposer != null)
            {
                decomposer.Pump1Started = false;
            }
        }

        private void butPump2stop_Click(object sender, EventArgs e)
        {
            USBPort.WriteByte(ApexConstants.cmStopPump2);
            if (decomposer != null)
            {
                decomposer.Pump2Started = false;
            }
        }

        private void butBPMeas_Click(object sender, EventArgs e)
        {
            USBPort.WriteByte(ApexConstants.cmStartBP1);
        }

        private void butBPabort_Click(object sender, EventArgs e)
        {
            USBPort.WriteByte(ApexConstants.cmCancelBP1);
        }

        private void butBPmeas2_Click(object sender, EventArgs e)
        {
            USBPort.WriteByte(ApexConstants.cmStartBP2);
        }

        private void butBPabort2_Click(object sender, EventArgs e)
        {
            USBPort.WriteByte(ApexConstants.cmCancelBP2);
        }

        
        private void BufPanel_MouseDown(object sender, MouseEventArgs e)
        {
            V.UpdateVisirs(e.X, ViewShift);
            label1.Text = V.Visir1.X.ToString() + " / " + V.Visir2.X.ToString();
            if (V.Visir1.Visible & V.Visir2.Visible)
            {
                DetectECG();
                butAnalyze.Enabled = true;
            }
        }


        private void DetectECG()
        {
            DataA.RInd = 0;
            int InsideC = 0;
            int start = V.Visir1.X;
            int stop = V.Visir2.X;
            int max = 0;
            for (int i = start; i < stop; i++)
            {
                max = Math.Max(max, (int)Math.Round(DataProcessing.GetCorr(DataA.ECG1ViewArray, i)));
            }
            double DetectLevel = max / 2;
            bool Rnow = false;
            InsideC = 0;
            for (int i = start; i < stop; i++)
            {
                InsideC++;
                if (InsideC > 20)
                {
                    if (!Rnow)
                    {
                        if (DataProcessing.GetCorr(DataA.ECG1ViewArray, i) > DetectLevel)
                        {
                            Rnow = true;
                        }
                    }
                    else
                    {
                        if (DataA.ECG1ViewArray[i] < DataA.ECG1ViewArray[i - 1])
                        {
                            DataA.RArray[DataA.RInd] = i + DataProcessing.CorrPattLen / 2;
                            DataA.RInd++;
                            InsideC = 0;
                            Rnow = false;
                        }
                    }
                }
            }
        }

        private void CountAllAver()
        {
            for (int i = 0; i < DataA.AverList.Count(); i++)
            {
                DataProcessing.CountAver(DataA.AverList[i], DataA.ViewList[i], DataA.RArray, DataA.RInd, 0);
            }

            for (int i = 2; i < DataA.AverList.Count(); i++)
            {
                DataProcessing.CountDerivative(DataA.AverList[i], DataA.FirstDerivList[i], DataA.FirstDerivApex.Length);
                DataProcessing.CountDerivative(DataA.FirstDerivList[i], DataA.SecDerivList[i], DataA.FirstDerivApex.Length);
            }

            int Size = 20;
            for (int i = 0; i < DataA.AverList.Count(); i++)
            {
                DataProcessing.CutArray(DataA.AverList[i], Size);
            }
            for (int i = 0; i < DataA.FirstDerivList.Count(); i++)
            {
                DataProcessing.CutArray(DataA.FirstDerivList[i], Size);
            }
            for (int i = 0; i < DataA.SecDerivList.Count(); i++)
            {
                DataProcessing.CutArray(DataA.SecDerivList[i], Size);
            }
        }

        private string GetName(int n)
        {
            switch (n)
            {
                case 0:
                    return ApexConstants.ECG1name;
                case 1:
                    return ApexConstants.ECG2name;
                case 2:
                    return ApexConstants.Reo1name;
                case 3:
                    return ApexConstants.Reo2name;
                case 4:
                    return ApexConstants.Sphigmo1name;
                case 5:
                    return ApexConstants.Sphigmo2name;
                case 6:
                    return ApexConstants.Apexname;
            }
            return null;
        }

        private double[] GetArray(int n)
        {
            switch (n)
            {
                case 0:
                    return DataA.AverECG1;
                case 1:
                    return DataA.AverECG2;
                case 2:
                    return DataA.AverReo1;
                case 3:
                    return DataA.AverReo2;
                case 4:
                    return DataA.AverSphigmo1;
                case 5:
                    return DataA.AverSphigmo2;
                case 6:
                    return DataA.AverApex;
            }
            return null;
        }

        private double[] GetSecDerivArray(int n)
        {
            switch (n)
            {
                case 0:
                    return null;
                case 1:
                    return null;
                case 2:
                    return DataA.SecDerivReo1;
                case 3:
                    return DataA.SecDerivReo2;
                case 4:
                    return DataA.SecDerivSphigmo1;
                case 5:
                    return DataA.SecDerivSphigmo2;
                case 6:
                    return DataA.SecDerivApex;
            }
            return null;
        }

        private double[] GetFirstDerivArray(int n)
        {
            switch (n)
            {
                case 0:
                    return null;
                case 1:
                    return null;
                case 2:
                    return DataA.FirstDerivReo1;
                case 3:
                    return DataA.FirstDerivReo2;
                case 4:
                    return DataA.FirstDerivSphigmo1;
                case 5:
                    return DataA.FirstDerivSphigmo2;
                case 6:
                    return DataA.FirstDerivApex;
            }
            return null;
        }

        private void butAnalyze_Click(object sender, EventArgs e)
        {
            {
                CountAllAver();
                List<SingleData> DataList;
                DataList = new List<SingleData>();
                for (int i = 0; i < GInfoArr.Length; i++)
                {
                    if (GInfoArr[i].Visible)
                    {
                        SingleData SD;
                        SD.MainData = GetArray(i);
                        SD.FirstDerivData = GetFirstDerivArray(i);
                        SD.SecDerivData = GetSecDerivArray(i);
                        SD.FirstDerivNorm = DataProcessing.Norm(SD.FirstDerivData);
                        SD.SecDerivNorm = DataProcessing.Norm(SD.SecDerivData);
                        SD.Name = GetName(i);
                        DataList.Add(SD);
                    }
                }
                var fem = new FormEditMarkers(DataList);
                fem.ShowDialog();
                fem.Dispose();

                V.NumOfClick = 2;
                V.UpdateVisirs(0, ViewShift);
                butAnalyze.Enabled = false;
                butReset.Enabled = true;
            }

        }

        private void butReset_Click(object sender, EventArgs e)
        {
            butReset.Enabled = false;
        }

    }
}

