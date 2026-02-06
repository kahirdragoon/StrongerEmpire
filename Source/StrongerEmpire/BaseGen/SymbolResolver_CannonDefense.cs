using RimWorld.BaseGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace StrongerEmpire;

public class SymbolResolver_CannonDefense : SymbolResolver
{
    public override void Resolve(ResolveParams rp)
    {
        Map map = BaseGen.globalSettings.map;
        CellRect rect = rp.rect;

        int extraX = ExtraPerSide(rect.Width);
        int extraZ = ExtraPerSide(rect.Height);

        List<IntVec3> positions =
        [
            new IntVec3(rect.minX, 0, rect.minZ),
            new IntVec3(rect.minX, 0, rect.maxZ),
            new IntVec3(rect.maxX, 0, rect.minZ),
            new IntVec3(rect.maxX, 0, rect.maxZ),
        ];

        AddSidePositionsX(rect.minX, rect.maxX, rect.maxZ, extraX, positions);
        AddSidePositionsX(rect.minX, rect.maxX, rect.minZ, extraX, positions);
        AddSidePositionsZ(rect.minZ, rect.maxZ, rect.minX, extraZ, positions);
        AddSidePositionsZ(rect.minZ, rect.maxZ, rect.maxX, extraZ, positions);

        foreach (IntVec3 pos in positions)
        {
            if (pos.InBounds(map) && pos.Standable(map))
            {
                Thing autocannon = ThingMaker.MakeThing(EmpireDefOf.Turret_Autocannon);
                autocannon.SetFaction(rp.faction);
                GenSpawn.Spawn(autocannon, pos, map);
            }
        }

        static int ExtraPerSide(int sideLength)
        {
            if (sideLength <= 12) return 0;
            if (sideLength >= 50) return 2;
            return 1;
        }

        static void AddSidePositionsX(int minX, int maxX, int z, int extraCount, List<IntVec3> positions)
        {
            if (extraCount <= 0) return;
            if (extraCount == 1)
            {
                positions.Add(new IntVec3((minX + maxX) / 2, 0, z));
                return;
            }
            positions.Add(new IntVec3((2 * minX + maxX) / 3, 0, z));
            positions.Add(new IntVec3((minX + 2 * maxX) / 3, 0, z));
        }

        static void AddSidePositionsZ(int minZ, int maxZ, int x, int extraCount, List<IntVec3> positions)
        {
            if (extraCount <= 0) return;
            if (extraCount == 1)
            {
                positions.Add(new IntVec3(x, 0, (minZ + maxZ) / 2));
                return;
            }
            positions.Add(new IntVec3(x, 0, (2 * minZ + maxZ) / 3));
            positions.Add(new IntVec3(x, 0, (minZ + 2 * maxZ) / 3));
        }
    }
}