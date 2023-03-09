using System;
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
            if (state.PatchMod.ModHeader.Stats.NumRecords < 4096)
            {
                Console.WriteLine($"ESLIfing patch stack");
                state.PatchMod.ModHeader.Flags |= SkyrimModHeader.HeaderFlag.LightMaster;
            }
        }
    }
}