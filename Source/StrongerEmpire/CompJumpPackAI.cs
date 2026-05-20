using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;

namespace StrongerEmpire
{
    public class CompProperties_JumpPackAI : CompProperties
    {
        public float meleeEngageRadius = 3f;   // skip engage jump if already this close to target
        public float rangedEscapeRadius = 3f;  // any enemy within this radius triggers a ranged escape jump

        public CompProperties_JumpPackAI() => compClass = typeof(CompJumpPackAI);
    }

    public class CompJumpPackAI : ThingComp
    {
        private static readonly List<Thing> tmpHostiles = [];
        private CompProperties_JumpPackAI Props => (CompProperties_JumpPackAI)props;

        public override void CompTick()
        {
            if (!parent.IsHashIntervalTick(120)) return;
            if (!StrongerEmpireMod.settings.enableJumpPackAI) return;

            Pawn wearer = ((Apparel)parent).Wearer;
            if (wearer == null) { Log.Message($"[StrongerEmpire] JumpPackAI: wearer is null."); return; }
            if (wearer.Dead) { Log.Message($"[StrongerEmpire] JumpPackAI: {wearer.LabelShort} is dead."); return; }
            if (wearer.IsColonist) { Log.Message($"[StrongerEmpire] JumpPackAI: {wearer.LabelShort} is colonist, skipping."); return; }
            if (!wearer.Spawned) { Log.Message($"[StrongerEmpire] JumpPackAI: {wearer.LabelShort} is not spawned."); return; }
            if (wearer.carryTracker?.CarriedThing is Pawn) { Log.Message($"[StrongerEmpire] JumpPackAI: {wearer.LabelShort} is carrying a pawn, skipping."); return; }

            CompApparelVerbOwner verbOwner = parent.TryGetComp<CompApparelVerbOwner>();
            if (verbOwner == null)
            {
                Log.Warning($"[StrongerEmpire] JumpPackAI: {wearer.LabelShort} jump pack has no CompApparelVerbOwner.");
                return;
            }
            if (!verbOwner.CanBeUsed(out _))
            {
                Log.Message($"[StrongerEmpire] JumpPackAI: {wearer.LabelShort} jump pack cannot be used (out of charges or invalid map).");
                return;
            }

            Verb jumpVerb = verbOwner.VerbTracker.PrimaryVerb;
            if (jumpVerb == null)
            {
                Log.Warning($"[StrongerEmpire] JumpPackAI: {wearer.LabelShort} jump pack has no primary verb.");
                return;
            }

            Log.Message($"[StrongerEmpire] JumpPackAI: {wearer.LabelShort} — IsFleeing={IsFleeing(wearer)}, " +
                $"RangedWeapon={wearer.equipment?.Primary?.def?.IsRangedWeapon == true}, " +
                $"CurJob={wearer.CurJob?.def?.defName ?? "none"}");

            Job job;
            if (IsFleeing(wearer))
                job = TryGetJumpAwayJob(wearer, jumpVerb);
            else if (wearer.equipment?.Primary?.def?.IsRangedWeapon == true)
                job = TryGetEscapeJob(wearer, jumpVerb);
            else
                job = TryGetEngageJob(wearer, jumpVerb);

            if (job == null)
            {
                Log.Message($"[StrongerEmpire] JumpPackAI: {wearer.LabelShort} — no valid jump destination found.");
                return;
            }

            Log.Message($"[StrongerEmpire] JumpPackAI: {wearer.LabelShort} — starting jump job to {job.targetA.Cell}.");
            wearer.jobs.StartJob(job, JobCondition.InterruptForced, null, false);
        }

        private static bool IsFleeing(Pawn pawn)
        {
            JobDef curJobDef = pawn.CurJob?.def;
            return curJobDef == JobDefOf.Flee
                || curJobDef == JobDefOf.FleeAndCower
                || curJobDef == JobDefOf.FleeAndCowerShort
                || pawn.MentalState?.def == MentalStateDefOf.PanicFlee
                || pawn.MentalState?.def == MentalStateDefOf.PanicFleeFire;
        }

        // Ranged: jump away if any enemy is within close range
        private Job TryGetEscapeJob(Pawn pawn, Verb jumpVerb)
        {
            bool anyNearby = false;
            foreach (Thing t in GenRadial.RadialDistinctThingsAround(
                pawn.Position, pawn.Map, Props.rangedEscapeRadius, false))
            {
                if (t is Pawn p && !p.Dead && !p.Downed && p.HostileTo(pawn))
                {
                    anyNearby = true;
                    break;
                }
            }

            if (!anyNearby)
            {
                Log.Message($"[StrongerEmpire] JumpPackAI: {pawn.LabelShort} (ranged) — no enemies within {Props.rangedEscapeRadius} cells.");
                return null;
            }

            return TryGetJumpAwayJob(pawn, jumpVerb);
        }

        // Melee / unarmed: jump toward nearest enemy to close the gap
        private Job TryGetEngageJob(Pawn pawn, Verb jumpVerb)
        {
            float range = jumpVerb.EffectiveRange;
            Log.Message($"[StrongerEmpire] JumpPackAI: {pawn.LabelShort} (melee) — jump verb range={range}.");

            Thing target = FindNearestEnemy(pawn, range);
            if (target == null) { Log.Message($"[StrongerEmpire] JumpPackAI: {pawn.LabelShort} — no enemy in range."); return null; }

            float dist = pawn.Position.DistanceTo(target.Position);
            if (dist <= Props.meleeEngageRadius)
            {
                Log.Message($"[StrongerEmpire] JumpPackAI: {pawn.LabelShort} — already close enough ({dist:F1} <= {Props.meleeEngageRadius}).");
                return null;
            }

            if (!RCellFinder.TryFindGoodAdjacentSpotToTouch(pawn, target, out IntVec3 dest))
            {
                Log.Message($"[StrongerEmpire] JumpPackAI: {pawn.LabelShort} — TryFindGoodAdjacentSpotToTouch failed.");
                return null;
            }
            if (!JumpUtility.ValidJumpTarget(pawn, pawn.Map, dest))
            {
                Log.Message($"[StrongerEmpire] JumpPackAI: {pawn.LabelShort} — ValidJumpTarget failed for {dest}.");
                return null;
            }
            if (!JumpUtility.CanHitTargetFrom(pawn, pawn.Position, dest, range))
            {
                Log.Message($"[StrongerEmpire] JumpPackAI: {pawn.LabelShort} — CanHitTargetFrom failed (pos={pawn.Position}, dest={dest}, range={range}).");
                return null;
            }

            Job job = JobMaker.MakeJob(JobDefOf.CastJump, dest);
            job.verbToUse = jumpVerb;
            return job;
        }

        // Shared: jump to a cell away from all known threats
        private static Job TryGetJumpAwayJob(Pawn pawn, Verb jumpVerb)
        {
            float range = jumpVerb.EffectiveRange;

            tmpHostiles.Clear();
            tmpHostiles.AddRange(
                pawn.Map.attackTargetsCache.GetPotentialTargetsFor(pawn)
                    .Where(a => !a.ThreatDisabled(pawn))
                    .Select(a => a.Thing));

            Log.Message($"[StrongerEmpire] JumpPackAI: {pawn.LabelShort} (away) — {tmpHostiles.Count} threats, jump range={range}.");

            IntVec3 dest = CellFinderLoose.GetFallbackDest(
                pawn, tmpHostiles, range, 5f, 5f,
                validator: c =>
                    JumpUtility.ValidJumpTarget(pawn, pawn.Map, c) &&
                    JumpUtility.CanHitTargetFrom(pawn, pawn.Position, c, range));

            tmpHostiles.Clear();

            if (!dest.IsValid)
            {
                Log.Message($"[StrongerEmpire] JumpPackAI: {pawn.LabelShort} — GetFallbackDest returned invalid cell.");
                return null;
            }

            Job job = JobMaker.MakeJob(JobDefOf.CastJump, dest);
            job.verbToUse = jumpVerb;
            return job;
        }

        private static Thing FindNearestEnemy(Pawn pawn, float maxRange)
        {
            Thing nearest = null;
            float nearestDist = float.MaxValue;

            foreach (IAttackTarget t in pawn.Map.attackTargetsCache.GetPotentialTargetsFor(pawn))
            {
                if (t.ThreatDisabled(pawn)) continue;
                float dist = pawn.Position.DistanceTo(t.Thing.Position);
                if (dist > maxRange || dist >= nearestDist) continue;
                nearestDist = dist;
                nearest = t.Thing;
            }

            return nearest;
        }
    }
}
