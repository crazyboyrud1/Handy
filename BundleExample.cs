using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Helpers;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Spt.Config;
using SPTarkov.Server.Core.Routers;
using SPTarkov.Server.Core.Servers;
using SPTarkov.Server.Core.Utils;
using System.Reflection;
using CustomAssortService;
namespace lilFilter;

[Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 2)]
public class LilFilter(
    WTTServerCommonLib.WTTServerCommonLib wttCommon, CustomAssortService.CustomAssortService customAssort) : IOnLoad
{
    public async Task OnLoad() 
    {
        Assembly assembly = Assembly.GetExecutingAssembly();

        await wttCommon.CustomClothingService.CreateCustomClothing(assembly);
        
        await wttCommon.CustomItemServiceExtended.CreateCustomItems(assembly);

        await wttCommon.CustomQuestService.CreateCustomQuests(assembly);

         customAssort.CreateCustomAssorts("5a7c2eca46aef81a7ca2145d");
        //    await wttCommon.CustomVoiceService.CreateCustomVoices(assembly);
        //    69a72360ba32f88711871850
        await Task.CompletedTask;
        
    }
}
//