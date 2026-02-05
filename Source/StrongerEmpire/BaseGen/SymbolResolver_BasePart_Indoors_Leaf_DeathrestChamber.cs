using RimWorld;
using RimWorld.BaseGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrongerEmpire;

public class SymbolResolver_BasePart_Indoors_Leaf_DeathrestChamber : SymbolResolver
{
    public override bool CanResolve(ResolveParams rp) => 
        base.CanResolve(rp) 
        && rp.faction == Faction.OfEmpire
        && BaseGen.globalSettings.basePart_throneRoomsResolved >= BaseGen.globalSettings.minThroneRooms 
        && (BaseGen.globalSettings.basePart_worshippedTerminalsResolved >= BaseGen.globalSettings.requiredWorshippedTerminalRooms || !SymbolResolver_BasePart_Indoors_Leaf_WorshippedTerminal.CanResolve("basePart_indoors_leaf", rp)) 
        && (BaseGen.globalSettings.basePart_gravcoresResolved >= BaseGen.globalSettings.requiredGravcoreRooms || !SymbolResolver_BasePart_Indoors_Leaf_Gravcore.CanResolve("basePart_indoors_leaf", rp));

    public override void Resolve(ResolveParams rp)
    {
        BaseGen.symbolStack.Push("empire_deathrestchamber", rp);
    }
}
