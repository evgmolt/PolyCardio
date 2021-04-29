using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApexCardio
{
    public class Visir
    {
        public int X;
        public bool Visible;

        public Visir()
        {
            X = 0;
            Visible = false;
        }
    }

    public class ApexVisirs
    {
        public Visir Visir1;
        public Visir Visir2;
        public int NumOfClick;

        public ApexVisirs()
        {
            Visir1 = new Visir();
            Visir2 = new Visir();
            NumOfClick = 0;
        }

        public void ResetVisirs()
        {
            Visir1.Visible = false;
            Visir2.Visible = false;
            NumOfClick = 0;
        }

        public void UpdateVisirs(int x, int shift)
        {
            NumOfClick++;
            if (NumOfClick > 2) NumOfClick = 0;
            switch (NumOfClick)
            {
                case 0:
                    Visir1.Visible = false;
                    Visir2.Visible = false;
                    Visir1.X = 0;
                    Visir2.X = 0;
                    break;
                case 1:
                    Visir1.Visible = true;
                    Visir1.X = x + shift;
                    break;
                case 2:
                    Visir2.Visible = true;
                    Visir2.X = x + shift;
                    if (Visir1.X > Visir2.X)
                    {
                        Visir2.X = Visir1.X;
                        Visir1.X = x + shift;
                    }
                    break;                    
            }
        }
    }
}
