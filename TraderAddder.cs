using System.Reflection;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Helpers;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Services;
using CustomTraderHelper;

namespace CustomAssortService; 
[Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 1)]
public class CustomAssortService(ModHelper modHelper, CustomTraderHelper.AddCustomTraderHelper addCustomTraderHelper, ISptLogger<CustomAssortService> logger, DatabaseService databaseService)
{
    public void CreateCustomAssorts(string traderId, string? relativeDir = null)
    {
        
        TraderAssort assort;
        var pathToMod = modHelper.GetAbsolutePathToModFolder(Assembly.GetExecutingAssembly());
        const string defaultdir = "/db/assort.json";
        string finalDir;
        if (relativeDir != null)
        {
            if (relativeDir[0] != '/')
            {
                logger.Warning($"added a \"/\" to the begining of the assort path {relativeDir} this is just because there was no / in the begining");
                finalDir = pathToMod + "/" + relativeDir;
                logger.Info($"{finalDir}");
            }
            else
            {
                logger.Info($"did not replace string");
                finalDir = pathToMod + relativeDir;
            }
        }
        else
        {
            finalDir = pathToMod + defaultdir;
        }
        logger.Info($"{finalDir}");
        logger.Info($"{pathToMod}");

        
        if (File.Exists(finalDir))
        {
            assort = modHelper.GetJsonDataFromFile<TraderAssort>(pathToMod, relativeDir ?? finalDir);
        }
        else
        {
            logger.Info($"{finalDir}");

            logger.Warning($"The assort path {finalDir} does not exsist using the default path \"/db/assort.json\"");

            finalDir = pathToMod + "/db/assort.json";

            logger.Info($"{finalDir}");

            if (File.Exists(finalDir))
            {
                assort = modHelper.GetJsonDataFromFile<TraderAssort>(pathToMod, relativeDir ?? finalDir);
            }
            else
            {
                logger.Warning($"■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■");
                logger.Warning($"There is nothing in the default path either not adding any custom assorts");
                logger.Warning($"■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■");
                return;
               
              
            }
        }
        
        if (!databaseService.GetTables().Traders.TryGetValue(traderId, out _))
        {
            logger.Warning($"Unable to update assorts for trader: {traderId}, they couldn't be found on the server");
            throw new ArgumentNullException(nameof(traderId), "The Id you're trying to identify dosen't exsist");
        }
        else
        {
            addCustomTraderHelper.OverwriteTraderAssort(traderId, assort);
        }

    }
}