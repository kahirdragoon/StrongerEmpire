using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrongerEmpire;

public static class BaseGen_GlobalSettingsEmpire
{
    public static bool EmpireSettlementResolved;

    public static void Clear()
    {
        EmpireSettlementResolved = false;
    }
}
