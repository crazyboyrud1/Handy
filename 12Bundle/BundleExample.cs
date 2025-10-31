using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Helpers;
using SPTarkov.Server.Core.Models.Spt.Mod;
using SPTarkov.Server.Core.Services.Mod;
using System.Reflection;
using SPTarkov.Server.Core.Models.Utils;

namespace _Handy_Toolbox
{
    public record ModMetadata : AbstractModMetadata
    {
        public override string ModGuid { get; init; } = "com.handy_toolbox";
        public override string Name { get; init; } = "Handy Toolbox";
        public override string Author { get; init; } = "Colobos9mm";
        public override List<string>? Contributors { get; init; } = ["Colo"];
        public override SemanticVersioning.Version Version { get; init; } = new("0.9.9");
        public override SemanticVersioning.Range SptVersion { get; init; } = new("~4.0.0");
        public override List<string>? Incompatibilities { get; init; } = ["ReadJsonConfigExample"];
        public override Dictionary<string, SemanticVersioning.Range>? ModDependencies { get; init; }
        public override string? Url { get; init; } = "http";
        public override bool? IsBundleMod { get; init; } = true;
        public override string? License { get; init; } = "MIT";
    }

    [Injectable(TypePriority = OnLoadOrder.PostSptModLoader)]

    public class HandyToolbox(
        ModHelper modHelper,
        CustomItemService customItemService,
        ISptLogger<HandyToolbox> logger) : IOnLoad
    {
        public Task OnLoad()
        {
            var pathToMod = modHelper.GetAbsolutePathToModFolder(Assembly.GetExecutingAssembly());
            var itemIdLookup = modHelper.GetJsonDataFromFile<Dictionary<string, string>>(pathToMod, @"db\itemId.json");
            var itemData = modHelper.GetJsonDataFromFile<Dictionary<string, NewItemFromCloneDetails>>(pathToMod, @"db\items.json");
            foreach (var itemId in itemIdLookup.Keys)
            {
                customItemService.CreateItemFromClone(itemData[itemId]);
            }
            logger.Success("HandyToolbox Loaded!");
            return Task.CompletedTask;
        }
    }
}