# Unity Analytics for GameTuner

<!--[![early-release]][tracker-classificiation]
[![Build][github-image]][github-action]
[![Release][release-image]][releases]
[![License][license-image]][license]
-->

## Overview

Add analytics to your Unity games and apps with the **[GameTuner][GameTuner]** event tracker for **[Unity][unity]**.

With this tracker you can collect event data from your Unity-based applications, games or frameworks.

Tracker is adopted to Unity engine. Read the wrapper section to see how to use plugin.

## Requirements

* GametunerUnityTracker requires Unity 2018.1+ and .NET Standard 2.0 Api Compatibility Level.
* GametunerUnityTracker requires Newtonsoft.Json. This is [bundled with Unity 2020.1+][unity-newtonsoftjson] or [Newtonsoft.Json-for-Unity][newtonsoftjson-for-unity] can be used for earlier versions.
* GametunerUnityTracker supports both Mono and IL2CPP scripting backends.

## Wrapper

First you need to initialize plugin:

```csharp
string analyticsAppID = "game_name";        // 'wokawoka' for Woka Woka; 'violasquest' for Viola's Quest
string apiKey = "secret-key";               // API key specific to every game
string store = "store_name";                // GooglePlay, AppleStore, Amazon...
string userID = "user_id";                  // User ID set by game developer
bool isSandboxEnabled = false;              // Is debug build
GametunerUnityTracker.Init(analyticsAppID, apiKey, isSandboxEnabled, userID, store);
```

Fields ```userID``` and ```store``` are optional. 

Since our games are offline, you may not have userId on Init, for that reason we left userID as optional field. Also, you can find method to set userID once you got validated id from server. 

Use this method after initialization of plugin.

```csharp
GametunerUnityTracker.SetUserId(userID);
```

Plugin will handle caching of userID and triggered events.

To trigger analytics event use:

```csharp
string eventName = "name_of_event";      // level_player, transaction...
string schemaVersion = "1-0-0";          // version of event schema
Dictionary<string, object> parameters;   // parameters of event. It can be null object
int priority = 0;                        // Priority of event. Higher the number, higher is prority

GametunerUnityTracker.LogEvent(eventName, schschemaVersionema, parameters, priority);
```
**NOTE**:If you make a call of ```LogEvent``` method before Init, event will be discarded, so make sure to call ```Init``` before any event log.

There are help methods for logging common events. You can use them to trigger common events or log them as regular event with latest schema version (contact GameTuner support to find out latest version).


```csharp
public static void LogEventAdStarted(string groupId, string adPlacement, string adPlacementGroup, string adProvider, string adType, int limit, int limitCounter, int durationSeconds, string crosspromo);
public static void LogEventAdWatched(string groupId, string ad_placement, string ad_placement_group, string adProvider, string adType, bool rewardClaimed, int limit, int limitCounter, int durationSeconds, int secondsWatched, string crosspromo);
public static void LogEventCurrencyChange(string groupId, string currency, int stashUpdated, int amountChange, int currencyLimit, int amountWasted, string reason, string gameMode, string screen);
public static void LogEventPurchase(string transactionId, string paymentProvider, string payload, string packageName, string packageContents, int premiumCurrency, double price, string priceCurrency, double priceUsd, double paidAmount, string paidCurrency, double paidUsd, string gameMode, string shopPlacement, string screen, string transactionCountryCode, string group_id, Dictionary<string, int> packageItems);
public static void LogEventPurchaseInitiated(string paymentProvider, string packageName, string packageContents, int premiumCurrency, double price, string priceCurrency, float priceUSD, string shopPlacement, string gameMode, string screen, string groupId);
```

### Logging 

Logging is disabled by default.
You can setup logging for debugging purposes before init or any time after init:

```csharp
LoggingLevel loggingLevel = LoggingLevel.Debug  // enum: Error, Debug, Verbose
GametunerUnityTracker.SetLoggingLevel(loggingLevel);
GametunerUnityTracker.EnableLogging();
```

To disable logging use method

```csharp
GametunerUnityTracker.DisableLogging();
```

### Tracker data

There are some data that are set and saved in tracker, which you can pull. Data is persistent between two session.

```csharp
GametunerUnityTracker.GetFirstOpenTime();   // gets first_open time in unix timestamp format (long)
GametunerUnityTracker.GetUserID();          // gets user Id set by developer
GametunerUnityTracker.GetInstallationID();  // gets installation ID. Installation ID is set on first_open
```

### Sessions

New session is triggered automatic in SDK. Session is triggered in next cases:
- When game (app) is opened as cold start (app was not in background)
- When game is opened from background and background time was longer then 5 minutes.
- When game is in foreground and event is triggered after 20 hours of inactivity.

If you need to use new session event (login) in game, you can subscribe to ```OnSessionStarted``` delegate.

```csharp
GametunerUnityTracker.onSessionStartEvent += OnSessionStart;

private void OnSessionStart(string sessionId, int sessionIndex, string previousSessionId)
{
    Debug.Log(string.Format("New session is started. SessionId: {}, SessionIndex: {},  PreviousSessionId: {} ", sessionId, sessionIndex, previousSessionId));
}
```

### Installation

* Download the `unitypackage` from [GitHub Releases][releases]. Drop into your Unity Editor.
* If Newtonsoft.Json.dll isn't available, install [Newtonsoft.Json-for-Unity][newtonsoftjson-for-unity].


[snowplow]: https://snowplowanalytics.com
[unity]: https://unity3d.com/
[nunit]: http://www.nunit.org/
[unity-newtonsoftjson]: https://docs.unity3d.com/Packages/com.unity.nuget.newtonsoft-json@2.0
[newtonsoftjson-for-unity]: https://github.com/jilleJr/Newtonsoft.Json-for-Unity

[vagrant-install]: https://docs.vagrantup.com/v2/installation
[virtualbox-install]: https://www.virtualbox.org/wiki/Downloads

[release-image]: https://img.shields.io/github/v/release/snowplow/snowplow-unity-tracker
[releases]: https://github.com/Algebra-AI/gametuner-tracker-unity/releases

[license-image]: https://img.shields.io/github/license/snowplow/snowplow-unity-tracker
[license]: https://www.apache.org/licenses/LICENSE-2.0

[github-image]: https://github.com/snowplow/snowplow-unity-tracker/actions/workflows/build.yml/badge.svg
[github-action]: https://github.com/snowplow/snowplow-unity-tracker/actions/workflows/build.yml

[techdocs-image]: https://d3i6fms1cm1j0i.cloudfront.net/github/images/techdocs.png
[setup-image]: https://d3i6fms1cm1j0i.cloudfront.net/github/images/setup.png
[roadmap-image]: https://d3i6fms1cm1j0i.cloudfront.net/github/images/roadmap.png
[contributing-image]: https://d3i6fms1cm1j0i.cloudfront.net/github/images/contributing.png

[techdocs]: https://docs.snowplowanalytics.com/docs/collecting-data/collecting-from-own-applications/unity-tracker/
[setup]: https://docs.snowplowanalytics.com/docs/collecting-data/collecting-from-own-applications/unity-tracker/setup/
[roadmap]: https://github.com/Two-Desperados/snowplow-unity-sdk/projects

[tracker-classificiation]: https://docs.snowplowanalytics.com/docs/collecting-data/collecting-from-own-applications/tracker-maintenance-classification/
[early-release]: https://img.shields.io/static/v1?style=flat&label=Snowplow&message=Early%20Release&color=014477&labelColor=9ba0aa&logo=data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAMAAAAoLQ9TAAAAeFBMVEVMaXGXANeYANeXANZbAJmXANeUANSQAM+XANeMAMpaAJhZAJeZANiXANaXANaOAM2WANVnAKWXANZ9ALtmAKVaAJmXANZaAJlXAJZdAJxaAJlZAJdbAJlbAJmQAM+UANKZANhhAJ+EAL+BAL9oAKZnAKVjAKF1ALNBd8J1AAAAKHRSTlMAa1hWXyteBTQJIEwRgUh2JjJon21wcBgNfmc+JlOBQjwezWF2l5dXzkW3/wAAAHpJREFUeNokhQOCA1EAxTL85hi7dXv/E5YPCYBq5DeN4pcqV1XbtW/xTVMIMAZE0cBHEaZhBmIQwCFofeprPUHqjmD/+7peztd62dWQRkvrQayXkn01f/gWp2CrxfjY7rcZ5V7DEMDQgmEozFpZqLUYDsNwOqbnMLwPAJEwCopZxKttAAAAAElFTkSuQmCC 
