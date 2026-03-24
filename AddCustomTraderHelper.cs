using System.Diagnostics.CodeAnalysis;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Spt.Config;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Services;
using TraderChecker;

namespace CustomTraderHelper
{


    /// <summary>
    /// We inject this class into 'AddTraderWithDynamicAssorts' to help us with adding the new trader into the server
    /// </summary>
    [Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 1)]
    [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract")]
    public class AddCustomTraderHelper(
        ISptLogger<AddCustomTraderHelper> logger,
        DatabaseService databaseService,
        TraderChecker.TraderChecker traderChecker
        )

    {
        /// <summary>
        /// Add the traders update time for when their offers refresh
        /// </summary>  
        /// <param name="traderConfig">trader config to add our trader to</param>
        /// <param name="baseJson">json file for trader (db/base.json)</param>
        /// <param name="refreshTimeSecondsMin">How many seconds between trader stock refresh min time</param>
        /// <param name="refreshTimeSecondsMax">How many seconds between trader stock refresh max time</param>
        public void SetTraderUpdateTime(TraderConfig traderConfig, TraderBase baseJson, int refreshTimeSecondsMin, int refreshTimeSecondsMax)
        {
            // Add refresh time in seconds to config
            var traderRefreshRecord = new UpdateTime
            {
                TraderId = baseJson.Id,
                Seconds = new MinMax<int>(refreshTimeSecondsMin, refreshTimeSecondsMax)
            };

            traderConfig.UpdateTime.Add(traderRefreshRecord);
        }
        /// <summary>
        /// Merge the desired trader's assorts with the ones provided (new entries replace or are added to existing)
        /// </summary>
        /// <param name="traderId">Trader to update assorts of</param>
        /// <param name="newAssorts">new assorts we want to merge in</param>
        public void OverwriteTraderAssort(string traderId, TraderAssort newAssorts)
        {
            if (!databaseService.GetTables().Traders.TryGetValue(traderId, out var traderToEdit))
            {
                logger.Warning($"Unable to update assorts for trader: {traderId}, they couldn't be found on the server");
                return;
            }

            // If the trader has no assort yet, just set the incoming one
            if (traderToEdit.Assort == null)
            {
                logger.Warning($"Trader {traderId} has no existing assorts, setting new assorts directly");
                traderToEdit.Assort = newAssorts;
                return;
            }

            var existing = traderToEdit.Assort;

            // Merge Items (append new items)
            if (newAssorts.Items != null)
            {
                if (existing.Items == null)
                    // ReSharper disable once UseCollectionExpression
                    existing.Items = new List<Item>(newAssorts.Items);
                else
                    traderChecker.TraderAssortAdder(existing, newAssorts, "Items", traderId);
            }

            // Merge BarterScheme (overwrite or add keys)
            if (newAssorts.BarterScheme != null)
            {
                if (existing.BarterScheme == null)
                    existing.BarterScheme = new Dictionary<MongoId, List<List<BarterScheme>>>(newAssorts.BarterScheme);
                else
                    traderChecker.TraderAssortAdder(existing, newAssorts, "BarterScheme", traderId);
            }

            // Merge LoyalLevelItems (overwrite or add keys)
            if (existing.LoyalLevelItems == null)
                existing.LoyalLevelItems = new Dictionary<MongoId, int>(newAssorts.LoyalLevelItems);
            else                                           
                traderChecker.TraderAssortAdder(existing, newAssorts, "LoyalLevel", traderId);

            // Prefer incoming NextResupply if present
            if (newAssorts.NextResupply.HasValue)
                existing.NextResupply = newAssorts.NextResupply;

            // assign back (existing is reference, but keep assignment for clarity)
            traderToEdit.Assort = existing;
        }
    }
}
