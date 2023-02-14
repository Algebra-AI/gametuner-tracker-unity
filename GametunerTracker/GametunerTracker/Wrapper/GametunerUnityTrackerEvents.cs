using System.Collections.Generic;
using SnowplowTracker.Logging;
using SnowplowTracker;

namespace GametunerTracker
{
    
    public static partial class GametunerUnityTracker
    {
        public static void LogEventAdStarted(string groupId, string adPlacement, string adPlacementGroup, string adProvider, string adType, int limit, int limitCounter, int durationSeconds, string crosspromo)
        {
            if (!isInitialized) { 
                Log.Error("Tracker is not initialized");
                return;
            }

            Dictionary<string, object> eventData = new Dictionary<string, object>();
            eventData.Add("group_id", groupId);
            eventData.Add("ad_placement", adPlacement);
            eventData.Add("ad_placement_group", adPlacementGroup);
            eventData.Add("ad_provider", adProvider);
            eventData.Add("ad_type", adType);
            eventData.Add("limit", limit);
            eventData.Add("limit_counter", limitCounter);
            eventData.Add("duration_seconds", durationSeconds);
            eventData.Add("crosspromo", crosspromo);

            LogEvent(EventNames.EVENT_AD_STARTED, Constants.EVENT_AD_STARTED_SCHEMA, eventData, null, 0);
        }

        public static void LogEventAdWatched(string groupId, string ad_placement, string ad_placement_group, string adProvider, string adType, bool rewardClaimed, int limit, int limitCounter, int durationSeconds, int secondsWatched, string crosspromo)
        {
            if (!isInitialized) { 
                Log.Error("Tracker is not initialized");
                return;
            }

            Dictionary<string, object> eventData = new Dictionary<string, object>();
            eventData.Add("group_id", groupId);
            eventData.Add("ad_placement", ad_placement);
            eventData.Add("ad_placement_group", ad_placement_group);
            eventData.Add("ad_provider", adProvider);
            eventData.Add("ad_type", adType);
            eventData.Add("reward_claimed", rewardClaimed);
            eventData.Add("limit", limit);
            eventData.Add("limit_counter", limitCounter);
            eventData.Add("duration_seconds", durationSeconds);
            eventData.Add("seconds_watched", secondsWatched);
            eventData.Add("crosspromo", crosspromo);
            

            LogEvent(EventNames.EVENT_AD_WATCHED, Constants.EVENT_AD_WATCHED_SCHEMA, eventData, null, 0);
        }

        public static void LogEventCurrencyChange(string groupId, string currency, int stashUpdated, int amountChange, int currencyLimit, int amountWasted, string reason, string gameMode, string screen)
        {
            if (!isInitialized) { 
                Log.Error("Tracker is not initialized");
                return;
            }

            Dictionary<string, object> eventData = new Dictionary<string, object>();
            eventData.Add("group_id", groupId);
            eventData.Add("currency", currency);
            eventData.Add("stash_updated", stashUpdated);
            eventData.Add("amount_change", amountChange);
            eventData.Add("currency_limit", currencyLimit);
            eventData.Add("amount_wasted", amountWasted);
            eventData.Add("reason", reason);
            eventData.Add("game_mode", gameMode);
            eventData.Add("screen", screen);

            LogEvent(EventNames.EVENT_CURRENCY_CHANGE, Constants.EVENT_CURRENCY_CHANGE_SCHEMA, eventData, null, 0);
        }

        public static void LogEventPurchaseInitiated(string paymentProvider, string packageName, string packageContents, int premiumCurrency, double price, string priceCurrency, float priceUSD, string shopPlacement, string gameMode, string screen, string groupId)
        {
            if (!isInitialized) { 
                Log.Error("Tracker is not initialized");
                return;
            }

            Dictionary<string, object> eventData = new Dictionary<string, object>();
            eventData.Add("payment_provider", paymentProvider);
            eventData.Add("package_name", packageName);
            eventData.Add("package_contents", packageContents);
            eventData.Add("premium_currency", premiumCurrency);
            eventData.Add("price", price);
            eventData.Add("price_currency", priceCurrency);
            eventData.Add("price_usd", priceUSD);            
            eventData.Add("game_mode", gameMode);
            eventData.Add("shop_placement", shopPlacement);
            eventData.Add("screen", shopPlacement);
            eventData.Add("group_id", groupId);

            LogEvent(EventNames.EVENT_PURCHASE_INITIATED, Constants.EVENT_PURCHASE_INITIATED_SCHEMA, eventData, null, 0);
        }

        //Create method to log event Purchase with parameters
        public static void LogEventPurchase(string transactionId, string paymentProvider, string payload, string packageName, string packageContents, int premiumCurrency, double price, string priceCurrency, double priceUsd, double paidAmount, string paidCurrency, double paidUsd, string gameMode, string shopPlacement, string screen, string transactionCountryCode, string group_id)
        {
            if (!isInitialized) { 
                Log.Error("Tracker is not initialized");
                return;
            }

            Dictionary<string, object> eventData = new Dictionary<string, object>();
            eventData.Add("transaction_id", transactionId);
            eventData.Add("payment_provider", paymentProvider);
            eventData.Add("payload", payload);
            eventData.Add("package_name", packageName);
            eventData.Add("package_contents", packageContents);
            eventData.Add("premium_currency", premiumCurrency);
            eventData.Add("price", price);
            eventData.Add("price_currency", priceCurrency);
            eventData.Add("price_usd", priceUsd);
            eventData.Add("paid_amount", paidAmount);
            eventData.Add("paid_currency", paidCurrency);
            eventData.Add("paid_usd", paidUsd);
            eventData.Add("game_mode", gameMode);
            eventData.Add("shop_placement", shopPlacement);
            eventData.Add("screen", screen);
            eventData.Add("transaction_country_code", transactionCountryCode);
            eventData.Add("group_id", group_id);

            LogEvent(EventNames.EVENT_PURCHASE, Constants.EVENT_PURCHASE_SCHEMA, eventData, null, 0);
        }
        
    }
}
