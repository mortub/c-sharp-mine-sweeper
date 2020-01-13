using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSClient
{
    public class MinesweeperException : ApplicationException
    {
        public MinesweeperException(string message)
           : base(message)
        {
        }

        public MinesweeperException(string message, Exception innerExeption)
            : base(message, innerExeption)
        {
        }
    }
}
