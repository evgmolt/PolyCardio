﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ApexCardio
{
    public class BufferedPanel : Panel
    {
        public int Number;
        public BufferedPanel(int number)
        {
            Number = number;
            DoubleBuffered = true;
        }
    }
}
