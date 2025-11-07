using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Spt.Mod;
using System.Reflection;

namespace _Handy_Toolbox
{
    public record ModMetadata : AbstractModMetadata
    {
        public override string ModGuid { get; init; } = "com.colo.handy_toolbox";
        public override string Name { get; init; } = "Handy Toolbox";
        public override string Author { get; init; } = "Colobos9mm";
        public override List<string>? Contributors { get; init; } = ["Colo"];
        public override SemanticVersioning.Version Version { get; init; } = new("0.9.9");
        public override SemanticVersioning.Range SptVersion { get; init; } = new("~4.0.1");
        public override List<string>? Incompatibilities { get; init; } = ["ReadJsonConfigExample"];
        public override Dictionary<string, SemanticVersioning.Range>? ModDependencies { get; init; }
        public override string? Url { get; init; } = "http";
        public override bool? IsBundleMod { get; init; } = true;
        public override string? License { get; init; } = "MIT";
    }

    [Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 0)]

    public class HandyToolbox(
        WTTServerCommonLib.WTTServerCommonLib wttCommon) : IOnLoad
    {
        public async Task OnLoad()
        {

            Assembly assembly = Assembly.GetExecutingAssembly();
            await wttCommon.CustomItemServiceExtended.CreateCustomItems(assembly);

        }
    }
}