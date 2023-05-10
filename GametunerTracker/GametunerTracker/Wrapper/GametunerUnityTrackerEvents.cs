using System.Collections.Generic;
using GametunerTracker.Logging;

namespace GametunerTracker
{
    
    public static partial class GametunerUnityTracker
    {
        public static void LogEventAdStarted(string adPlacement,
                                            string groupId = null,
                                            string adPlacementGroup = null, 
                                            string adProvider = null, 
                                            string adType = null, 
                                            int limit = int.MinValue, 
                                            int limitCounter = int.MinValue, 
                                            int durationSeconds = int.MinValue, 
                                            string crosspromo = null)
        {
            if (!isInitialized) { 
                Log.Error("Tracker is not initialized");
                return;
            }

            Dictionary<string, object> eventData = new Dictionary<string, object>();
            eventData.Add("ad_placement", adPlacement);
            if (groupId != null)
                eventData.Add("group_id", groupId);
            if (adPlacementGroup != null) 
                eventData.Add("ad_placement_group", adPlacementGroup);
            if (adProvider != null) 
                eventData.Add("ad_provider", adProvider);
            if (adType != null)
                eventData.Add("ad_type", adType);
            if (limit != int.MinValue) 
                eventData.Add("limit", limit);
            if (limitCounter != int.MinValue) 
                eventData.Add("limit_counter", limitCounter);
            if (durationSeconds != int.MinValue) 
                eventData.Add("duration_seconds", durationSeconds);
            if (crosspromo != null) 
                eventData.Add("crosspromo", crosspromo);

            LogEvent(EventNames.EVENT_AD_STARTED, Constants.EVENT_AD_STARTED_SCHEMA, eventData, null, 0);
        }

        public static void LogEventAdWatched(string adPlacement, 
                                            bool rewardClaimed,
                                            string groupId = null,                                             
                                            string adPlacementGroup = null, 
                                            string adProvider = null, 
                                            string adType = null, 
                                            int limit = int.MinValue, 
                                            int limitCounter = int.MinValue, 
                                            int durationSeconds = int.MinValue, 
                                            int secondsWatched = int.MinValue, 
                                            string crosspromo = null)
        {
            if (!isInitialized) { 
                Log.Error("Tracker is not initialized");
                return;
            }

            Dictionary<string, object> eventData = new Dictionary<string, object>();
            eventData.Add("ad_placement", adPlacement);
            eventData.Add("reward_claimed", rewardClaimed);
            if (groupId != null)
                eventData.Add("group_id", groupId);
            if (adPlacementGroup != null)
                eventData.Add("ad_placement_group", adPlacementGroup);
            if (adProvider != null)
                eventData.Add("ad_provider", adProvider);
            if (adType != null)
                eventData.Add("ad_type", adType);
            if (limit != int.MinValue) 
                eventData.Add("limit", limit);
            if (limitCounter != int.MinValue)
                eventData.Add("limit_counter", limitCounter);
            if (durationSeconds != int.MinValue) 
                eventData.Add("duration_seconds", durationSeconds);
            if (secondsWatched != int.MinValue) 
                eventData.Add("seconds_watched", secondsWatched);
            if (crosspromo != null) 
                eventData.Add("crosspromo", crosspromo);
            

            LogEvent(EventNames.EVENT_AD_WATCHED, Constants.EVENT_AD_WATCHED_SCHEMA, eventData, null, 0);
        }

        public static void LogEventCurrencyChange(string currency, 
                                                long amountChange,       
                                                string groupId = null,
                                                long stashUpdated = int.MinValue, 
                                                long currencyLimit = int.MinValue, 
                                                long amountWasted = int.MinValue, 
                                                string reason = null, 
                                                string gameMode = null, 
                                                string screen = null)
        {
            if (!isInitialized) { 
                Log.Error("Tracker is not initialized");
                return;
            }

            Dictionary<string, object> eventData = new Dictionary<string, object>();
            eventData.Add("currency", currency);
            eventData.Add("amount_change", amountChange);
            if (groupId != null)
                eventData.Add("group_id", groupId);
            if (stashUpdated != int.MinValue)
                eventData.Add("stash_updated", stashUpdated);
            if (currencyLimit != int.MinValue)
                eventData.Add("currency_limit", currencyLimit);
            if (amountWasted != int.MinValue)
                eventData.Add("amount_wasted", amountWasted);
            if (reason != null) 
                eventData.Add("reason", reason);
            if (gameMode != null)
                eventData.Add("game_mode", gameMode);
            if (screen != null)
                eventData.Add("screen", screen);

            LogEvent(EventNames.EVENT_CURRENCY_CHANGE, Constants.EVENT_CURRENCY_CHANGE_SCHEMA, eventData, null, 0);
        }

        public static void LogEventPurchaseInitiated(string packageName, 
                                                    string paymentProvider = null, 
                                                    string packageContents = null, 
                                                    long premiumCurrency = int.MinValue,   
                                                    double price = double.MinValue, 
                                                    string priceCurrency = null,                                                   
                                                    float priceUSD = float.MinValue, 
                                                    string shopPlacement = null, 
                                                    string gameMode = null, 
                                                    string screen = null, 
                                                    string groupId = null)
        {
            if (!isInitialized) { 
                Log.Error("Tracker is not initialized");
                return;
            }

            Dictionary<string, object> eventData = new Dictionary<string, object>();
            eventData.Add("package_name", packageName);
            if (paymentProvider != null)
                eventData.Add("payment_provider", paymentProvider);
            if (packageContents != null)
                eventData.Add("package_contents", packageContents);
            if (premiumCurrency != int.MinValue)
                eventData.Add("premium_currency", premiumCurrency);
            if (price != double.MinValue)
                eventData.Add("price", price);
            if (priceCurrency != null)
                eventData.Add("price_currency", priceCurrency);
            if (priceUSD != float.MinValue)
                eventData.Add("price_usd", priceUSD);
            if (shopPlacement != null)
                eventData.Add("shop_placement", shopPlacement);
            if (gameMode != null)
                eventData.Add("game_mode", gameMode);
            if (screen != null)
                eventData.Add("screen", screen);
            if (groupId != null)
                eventData.Add("group_id", groupId);

            LogEvent(EventNames.EVENT_PURCHASE_INITIATED, Constants.EVENT_PURCHASE_INITIATED_SCHEMA, eventData, null, 0);
        }

        //Create method to log event Purchase with parameters
        public static void LogEventPurchase(string packageName, 
                                            double paidAmount, 
                                            string paidCurrency, 
                                            string transactionId = null, 
                                            string paymentProvider = null, 
                                            string payload = null,  
                                            string packageContents = null, 
                                            long premiumCurrency = int.MinValue, 
                                            double price = double.MinValue, 
                                            string priceCurrency = null, 
                                            double priceUsd = double.MinValue,                                            
                                            double paidUsd = double.MinValue, 
                                            string gameMode = null, 
                                            string shopPlacement = null, 
                                            string screen = null, 
                                            string transactionCountryCode = null, 
                                            string groupId = null, 
                                            Dictionary<string, int> packageItems = null)
        {
            if (!isInitialized) { 
                Log.Error("Tracker is not initialized");
                return;
            }

            Dictionary<string, object> eventData = new Dictionary<string, object>();
            eventData.Add("package_name", packageName);
            eventData.Add("paid_amount", paidAmount);
            eventData.Add("paid_currency", paidCurrency);
            if (transactionId != null)
                eventData.Add("transaction_id", transactionId);
            if (paymentProvider != null)
                eventData.Add("payment_provider", paymentProvider);
            if (payload != null)
                eventData.Add("payload", payload);
            if (packageContents != null)
                eventData.Add("package_contents", packageContents);
            if (premiumCurrency != int.MinValue)   
                eventData.Add("premium_currency", premiumCurrency);
            if (price != double.MinValue)
                eventData.Add("price", price);
            if (priceCurrency != null)  
                eventData.Add("price_currency", priceCurrency);
            if (priceUsd != double.MinValue)
                eventData.Add("price_usd", priceUsd);
            if (paidUsd != double.MinValue)
                eventData.Add("paid_usd", paidUsd);
            if (gameMode != null)
                eventData.Add("game_mode", gameMode);
            if (shopPlacement != null)
                eventData.Add("shop_placement", shopPlacement);
            if (screen != null)
                eventData.Add("screen", screen);
            if (transactionCountryCode != null)
                eventData.Add("transaction_country_code", transactionCountryCode);
            if (groupId != null)    
                eventData.Add("group_id", groupId);
            if (packageItems != null)
                eventData.Add("package_items", packageItems);

            LogEvent(EventNames.EVENT_PURCHASE, Constants.EVENT_PURCHASE_SCHEMA, eventData, null, 0);
        }
        
    }
}
