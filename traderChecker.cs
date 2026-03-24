using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Utils;

namespace TraderChecker
{
    [Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 1)]
    public class TraderChecker(
        ISptLogger<TraderChecker> logger
        )
    {
        public void TraderAssortAdder(TraderAssort currentTypeList, TraderAssort newTypeList, string type, string traderId)
        {

            var closer = 0;

            if (type.Equals("Items", StringComparison.CurrentCultureIgnoreCase))
            {
                foreach (var newItem in newTypeList.Items)
                {

                    foreach (var currentItem in currentTypeList.Items)
                    {
                        if (newItem == currentItem)
                        {
                            logger.Warning($"Trader {traderId} already has the same items list reference {currentItem.Id} skipping merge to avoid duplicate");
                            closer = 1;
                            break;
                        }
                        else
                        {
                            logger.Warning($"nothing has not broke yet on item: {currentItem.Id}");
                           
                        }

                    }
                    if (closer == 1)
                    {
                        closer = 0;
                        
                    }
                    else
                    {
                        currentTypeList.Items.AddRange(newTypeList.Items);
                    }
                }
            }
            else if (type.Equals("BarterScheme", StringComparison.CurrentCultureIgnoreCase))
            {
                foreach (var newBarter in newTypeList.BarterScheme)
                {
                    foreach (var newBarterKeys in newTypeList.BarterScheme.Keys)
                    {
                        foreach (var currentBarterKeys in currentTypeList.BarterScheme.Keys)
                        {
                            if (newBarterKeys == currentBarterKeys)
                            {
                               logger.Warning($"Trader {traderId} already has the same barter list reference {currentBarterKeys} skipping merge to avoid duplicate");
                                closer = 1;
                                break;
                            }
                            else
                            { 
                                logger.Warning($"nothing has not broke yet on barter key {currentBarterKeys}");
                            }

                        }
                    }

                    if (closer == 1)
                    {
                        closer = 0;
                    }
                    else
                    {
                        currentTypeList.BarterScheme[newBarter.Key] = newBarter.Value;
                    }
                }   
            }
            else if (type.Equals("LoyalLevel", StringComparison.CurrentCultureIgnoreCase))
            {
                foreach (var newLoyalty in newTypeList.LoyalLevelItems)
                {
                    foreach (var newLoyaltyKeys in newTypeList.LoyalLevelItems.Keys)
                    {
                        foreach (var currentLoyaltyKeys in currentTypeList.LoyalLevelItems.Keys)
                        {
                            if (newLoyaltyKeys == currentLoyaltyKeys)
                            {
                                logger.Warning($"Trader {traderId} already has the same loyalty list reference {currentLoyaltyKeys} skipping merge to avoid duplicate");
                                closer = 1;
                                break;
                            }
                            else
                            {
                                logger.Warning($"nothing has not broke yet on loyalty key: {currentLoyaltyKeys}");
                            }

                        }
                    }

                    if (closer == 1)
                    {
                        closer = 0;
                    }
                    else
                    {
                        currentTypeList.LoyalLevelItems[newLoyalty.Key] = newLoyalty.Value;
                    }
                }
            }
        }
    }
}

