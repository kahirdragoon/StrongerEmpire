using RimWorld;
using RimWorld.BaseGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace StrongerEmpire;

public class Genstep_TurrentUpgrade : GenStep
{
    public override int SeedPart => 985520570;

    public override void Generate(Map map, GenStepParams parms)
    {
        var chargeTurretDef = DefDatabase<ThingDef>.GetNamedSilentFail("VFES_Turret_ChargeTurret");
        var chargeRailgunDef = DefDatabase<ThingDef>.GetNamedSilentFail("VFES_Turret_ChargeRailgun");
        
        if (chargeTurretDef == null && chargeRailgunDef == null)
            return;
        
        MapGenerator.TryGetVar("SettlementRect", out CellRect settlementRect);
        if (settlementRect == null || settlementRect.IsEmpty)
            if (!MapGenerator.TryGetVar("RectOfInterest", out settlementRect))
                return;

        List<Thing> turretToReplace = [];
        List<Thing> autocannonToReplace = [];
        
        foreach (IntVec3 c in settlementRect)
        {
            if (!c.InBounds(map)) continue;
            foreach (Thing t in map.thingGrid.ThingsListAtFast(c))
            {
                if (chargeTurretDef != null && t.def.defName == RimWorld.ThingDefOf.Turret_MiniTurret.defName)
                    turretToReplace.Add(t);
                if(chargeRailgunDef != null && t.def.defName == EmpireThingDefOf.Turret_Autocannon.defName)
                    autocannonToReplace.Add(t);
            }
        }

        Replace(map, turretToReplace, chargeTurretDef);
        Replace(map, autocannonToReplace, chargeRailgunDef);
    }

    private static void Replace(Map map, List<Thing> buildingsToReplace, ThingDef replacementDef)
    {
        foreach (var old in buildingsToReplace)
        {
            var stuff = replacementDef.CostStuffCount > 0 ? RimWorld.ThingDefOf.Plasteel : null;
            Thing replacement = ThingMaker.MakeThing(replacementDef, stuff);
            replacement.SetFaction(old.Faction);
            replacement.HitPoints = replacement.MaxHitPoints;
            GenSpawn.Spawn(replacement, old.Position, map, old.Rotation);
        }
    }
}
