using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UBSRisk.Interfaces
{
    public class MediumRiskCategory : ICategory
    {
        public string Category => "MEDIUMRISK";

        public bool CheckCategory(ITrade trade, DateTime referenceDate)
        {
            return trade.Value > 1000000 && trade.ClientSector == Useful.TypeClientSector.Public;
        }
    }
}
