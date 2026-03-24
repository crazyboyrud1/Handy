using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Spt.Mod;
using System.Reflection;

namespace _Handy_Toolbox
{
    public async Task OnLoad()
    {

            Assembly assembly = Assembly.GetExecutingAssembly();

        var assort = modHelper.GetJsonDataFromFile<TraderAssort>(pathToMod, "db/assort.json");

        AddCustomTraderHelper.OverwriteTraderAssort("5a7c2eca46aef81a7ca2145d", assort);

            await wttCommon.CustomItemServiceExtended.CreateCustomItems(assembly);

        await wttCommon.CustomQuestService.CreateCustomQuests(assembly);
     
        await Task.CompletedTask;
    }
}
