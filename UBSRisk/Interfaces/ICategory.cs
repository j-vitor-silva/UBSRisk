using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UBSRisk.Interfaces
{
    public interface ICategory
    {
        public string Category { get; }
        public bool CheckCategory(ITrade trade, DateTime referenceDate);
    }
}
