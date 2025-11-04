using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace StrongerEmpire.Biotech
{
    public class Genepool
    {
        public string ModId;
        public List<GeneDef> ProCombatGenes = new List<GeneDef>();
        public List<GeneDef> MetaIncreasingGenes = new List<GeneDef>();
    }
}
