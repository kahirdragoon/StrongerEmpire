using RimWorld;
using RimWorld.BaseGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrongerEmpire;

public class SymbolResolver_SettlementEmpire : SymbolResolver
{
    public override bool CanResolve(ResolveParams rp)
    {
        return rp.faction.def == FactionDefOf.Empire
            && !BaseGen_GlobalSettingsEmpire.EmpireSettlementResolved
            && base.CanResolve(rp);
    }

    public override void Resolve(ResolveParams rp)
    {
        BaseGen.symbolStack.Push("settlement", rp);
        BaseGen_GlobalSettingsEmpire.EmpireSettlementResolved = true;
        BaseGen.symbolStack.Push("empire_cannondefense", rp);
    }
}
