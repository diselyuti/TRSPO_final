using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRSPO_final
{
    [Serializable()]
    internal class MatrixRequestContainer
    {
        public int[,] matrix;
        public bool ok;
        public string error;

        public MatrixRequestContainer(int[,] matrix, bool ok, string error = "")
        {
            this.matrix = matrix;
            this.ok = ok;
            this.error = error;
        }
    }
}
