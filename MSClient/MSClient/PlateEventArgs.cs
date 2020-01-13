using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSClient
{
    public class PlateEventArgs : EventArgs
    {
        public int PlateRow { get; set; }
        public int PlateColumn { get; set; }

        public PlateEventArgs(int row, int col)
        {
            this.PlateRow = row;
            this.PlateColumn = col;
        }
    }
}
