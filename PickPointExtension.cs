using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoes
{
    public partial class PickPoint
    {
        public string FullAddress => $"{City},{Street},{Building}";
    }
}
