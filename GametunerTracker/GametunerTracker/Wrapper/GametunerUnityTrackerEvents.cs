using System.Collections.Generic;
using SnowplowTracker.Logging;
using SnowplowTracker;

namespace GametunerTracker
{
    
    public static partial class GametunerUnityTracker
    {
        public static void LogEventAdStarted(string groupId, string placement, string adProvider, bool rewarded, string placementGroup, int limit, int limitCounter)
        {
            if (!isInitialized) { 
                Log.Error("Tracker is not initialized");
                return;
            }

            Dictionary<string, object> eventData = new Dictionary<string, object>();
            eventData.Add("group_id", groupId);
            eventData.Add("placement", placement);
            eventData.Add("ad_provider", adProvider);
            eventData.Add("rewarded", rewarded);
            eventData.Add("placement_group", placementGroup);
            eventData.Add("limit", limit);
            eventData.Add("limit_counter", limitCounter);

            LogEvent(EventNames.EVENT_AD_STARTED, Constants.EVENT_AD_STARTED_SCHEMA, eventData, null, 0);
        }

        public static void LogEventAdWatched(string groupId, string placement, string adProvider, bool rewarded, string placementGroup, int limit, int limitCounter, int duration, int secondsWatched)
        {
            if (!isInitialized) { 
                Log.Error("Tracker is not initialized");
                return;
            }

            Dictionary<string, object> eventData = new Dictionary<string, object>();
            eventData.Add("group_id", groupId);
            eventData.Add("placement", placement);
            eventData.Add("ad_provider", adProvider);
            eventData.Add("rewarded", rewarded);
            eventData.Add("placement_group", placementGroup);
            eventData.Add("limit", limit);
            eventData.Add("limit_counter", limitCounter);
            eventData.Add("duration", duration);
            eventData.Add("seconds_watched", secondsWatched);

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

        public static void LogEventPurchaseInitiated(string transactionType, string paymentProvider, string packageName, string packageContents, int premiumCurrency, double price, string priceCurrency, string shopPlacement, string gameMode, string placement)
        {
            if (!isInitialized) { 
                Log.Error("Tracker is not initialized");
                return;
            }

            Dictionary<string, object> eventData = new Dictionary<string, object>();
            eventData.Add("transaction_type", transactionType);
            eventData.Add("payment_provider", paymentProvider);
            eventData.Add("package_name", packageName);
            eventData.Add("package_contents", packageContents);
            eventData.Add("premium_currency", premiumCurrency);
            eventData.Add("price", price);
            eventData.Add("price_currency", priceCurrency);
            eventData.Add("shop_placement", shopPlacement);
            eventData.Add("game_mode", gameMode);
            eventData.Add("placement", placement);

            LogEvent(EventNames.EVENT_PURCHASE_INITIATED, Constants.EVENT_PURCHASE_INITIATED_SCHEMA, eventData, null, 0);
        }

        //Create method to log event Purchase with parameters
        public static void LogEventPurchase(string transactionType, string transactionId, string paymentProvider, string payload, string packageName, string packageContents, int premiumCurrency, double price, string priceCurrency, double priceUsd, double paidAmount, string paidCurrency, double paidUsd, string gameMode, string shopPlacement, string screen, string transactionCountryCode)
        {
            if (!isInitialized) { 
                Log.Error("Tracker is not initialized");
                return;
            }

            Dictionary<string, object> eventData = new Dictionary<string, object>();
            eventData.Add("transaction_type", transactionType);
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

            LogEvent(EventNames.EVENT_PURCHASE, Constants.EVENT_PURCHASE_SCHEMA, eventData, null, 0);
        }
        
    }
}
