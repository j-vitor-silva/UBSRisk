using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UBSRisk.Interfaces
{
    public class ExpiredCategory : ICategory
    {
        public string Category => "EXPIRED";

        public bool CheckCategory(ITrade trade, DateTime referenceDate)
        {
            return trade.NextPaymentDate < referenceDate.AddDays(-30);
        }
    }
}
