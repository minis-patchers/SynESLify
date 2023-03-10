using System;
using System.Linq;
using System.Threading.Tasks;

using Mutagen.Bethesda;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Synthesis;

namespace SynCOBJ
{
    internal class Program
    {
        public static async Task<int> Main(string[] args)
        {
            return await SynthesisPipeline.Instance
                .AddPatch<ISkyrimMod, ISkyrimModGetter>(RunPatch)
                .SetTypicalOpen(GameRelease.SkyrimSE, "SynCOBJ.esp")
                .Run(args);
        }
        public static void RunPatch(IPatcherState<ISkyrimMod, ISkyrimModGetter> state)
        {
            var overrides = state.PatchMod.EnumerateMajorRecords().Where(x=>x.FormKey.ModKey != state.PatchMod.ModKey).Count();
            var fidcheck = (state.PatchMod.EnumerateMajorRecords().Where(x=>x.FormKey.ModKey == state.PatchMod.ModKey).Select(x=>x.FormKey.ID).Max() <= 0x000FFFFF);
            if (state.PatchMod.ModHeader.Stats.NumRecords - overrides < 4096 && fidcheck)
            {
                Console.WriteLine($"ESLIfing patch stack with {overrides} overrides and {state.PatchMod.ModHeader.Stats.NumRecords - overrides} new records");
                state.PatchMod.ModHeader.Flags |= SkyrimModHeader.HeaderFlag.LightMaster;
            }
        }
    }
}