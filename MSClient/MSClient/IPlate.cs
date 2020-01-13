using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSClient
{
    public interface IPlate
    {
        // Plate must have position getters
        int RowPosition { get; }
        int ColPosition { get; }
    }
}
