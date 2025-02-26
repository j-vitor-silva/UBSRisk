using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UBSRisk.Useful;

namespace UBSRisk.Interfaces
{
    public interface ITrade
    {
        public double Value { get; set; }
        public TypeClientSector ClientSector { get; set; }
        public DateTime NextPaymentDate { get; set; }
    }
}
