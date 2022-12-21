/*
 * Constants.cs
 * SnowplowTracker
 * 
 * Copyright (c) 2015 Snowplow Analytics Ltd. All rights reserved.
 *
 * This program is licensed to you under the Apache License Version 2.0,
 * and you may not use this file except in compliance with the Apache License Version 2.0.
 * You may obtain a copy of the Apache License Version 2.0 at http://www.apache.org/licenses/LICENSE-2.0.
 *
 * Unless required by applicable law or agreed to in writing,
 * software distributed under the Apache License Version 2.0 is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the Apache License Version 2.0 for the specific language governing permissions and limitations there under.
 * 
 * Authors: Joshua Beemster, Paul Boocock
 * Copyright: Copyright (c) 2015-2019 Snowplow Analytics Ltd
 * License: Apache License Version 2.0
 */

using System;

namespace SnowplowTracker {
	internal class Constants {

		// Schemas
		public readonly static string SCHEMA_PAYLOAD_DATA    = "iglu:com.algebraai.gametuner.common/payload_data/jsonschema/1-0-0";
		public readonly static string SCHEMA_CONTEXTS        = "iglu:com.snowplowanalytics.snowplow/contexts/jsonschema/1-0-1";
		public readonly static string SCHEMA_UNSTRUCT_EVENT  = "iglu:com.snowplowanalytics.snowplow/unstruct_event/jsonschema/1-0-0";
		public readonly static string SCHEMA_SCREEN_VIEW     = "iglu:com.snowplowanalytics.snowplow/screen_view/jsonschema/1-0-0";
		public readonly static string SCHEMA_USER_TIMINGS    = "iglu:com.snowplowanalytics.snowplow/timing/jsonschema/1-0-0";
		public readonly static string SCHEMA_SESSION         = "iglu:com.snowplowanalytics.snowplow/client_session/jsonschema/1-0-1";
		public readonly static string SCHEMA_DESKTOP         = "iglu:com.snowplowanalytics.snowplow/desktop_context/jsonschema/1-0-0";
		public readonly static string SCHEMA_MOBILE          = "iglu:com.snowplowanalytics.snowplow/mobile_context/jsonschema/1-0-1";
		public readonly static string SCHEMA_GEO_LOCATION    = "iglu:com.snowplowanalytics.snowplow/geolocation_context/jsonschema/1-1-0";
		public readonly static string SCHEMA_DEVICE_CONTEXT  = "iglu:com.algebraai.gametuner.embedded_context/device_context/jsonschema/1-0-0";
		public readonly static string SCHEMA_SESSION_CONTEXT = "iglu:com.algebraai.gametuner.embedded_context/session_context/jsonschema/1-0-0";
		public readonly static string SCHEMA_EVENT_CONTEXT   = "iglu:com.algebraai.gametuner.context/event_context/jsonschema/1-0-0";

		// Event Types
		public readonly static string EVENT_PAGE_VIEW       = "pv";
		public readonly static string EVENT_STRUCTURED      = "se";
		public readonly static string EVENT_UNSTRUCTURED    = "ue";
		public readonly static string EVENT_ECOMM           = "tr";
		public readonly static string EVENT_ECOMM_ITEM      = "ti";

		// Emitter
		public readonly static string GET_URI_SUFFIX        = "/i";
		public readonly static string POST_URI_SUFFIX       = "/com.snowplowanalytics.snowplow/tp2";
		public readonly static string POST_CONTENT_TYPE     = "application/json";

		// Session
		public readonly static string SESSION_ID            = "session_id";
		public readonly static string SESSION_INDEX         = "session_index";
		public readonly static string SESSION_TIME      	= "session_time";

		// Platform Generic
		public readonly static string PLAT_OS_TYPE          = "osType";
		public readonly static string PLAT_OS_VERSION       = "osVersion";
		public readonly static string PLAT_DEVICE_MANU      = "deviceManufacturer";
		public readonly static string PLAT_DEVICE_MODEL     = "deviceModel";

		// Desktop Context
		public readonly static string DESKTOP_SERVICE_PACK  = "osServicePack";
		public readonly static string DESKTOP_IS_64_BIT     = "osIs64Bit";
		public readonly static string DESKTOP_PROC_COUNT    = "deviceProcessorCount";
		
		// Mobile Context
		public readonly static string MOBILE_CARRIER        = "carrier";
		public readonly static string MOBILE_OPEN_IDFA      = "openIdfa";
		public readonly static string MOBILE_APPLE_IDFA     = "appleIdfa";
		public readonly static string MOBILE_APPLE_IDFV     = "appleIdfv";
		public readonly static string MOBILE_ANDROID_IDFA   = "androidIdfa";
		public readonly static string MOBILE_NET_TYPE       = "networkType";
		public readonly static string MOBILE_NET_TECH       = "networkTechnology";

		// Device Context
		public readonly static string DEVICE_CATEGORY       = "device_category";
		public readonly static string DEVICE_MANUFACTURER   = "device_manufacturer";
		public readonly static string DEVICE_MODEL    	 	= "model";
		public readonly static string DEVICE_OS_VERSION     = "os_version";
		public readonly static string DEVICE_CPU_TYPE		= "cpu_type";
		public readonly static string DEVICE_GPU	        = "gpu";
		public readonly static string DEVICE_RAM_SIZE       = "ram_size";
		public readonly static string DEVICE_SCREEN_RES     = "screen_resolution";
		public readonly static string DEVICE_LANGUAGE       = "device_language";
		public readonly static string DEVICE_TIMEZONE       = "device_timezone";
		public readonly static string DEVICE_SOURCE         = "source";
		public readonly static string DEVICE_MEDIUM         = "medium";
		public readonly static string DEVICE_CAMPAIGN       = "campaign";
		public readonly static string DEVICE_BUILD_VERSION  = "build_version";
		public readonly static string DEVICE_DEVICE_ID      = "device_id";
		public readonly static string DEVICE_ADVERTISING_ID = "advertising_id";
		public readonly static string DEVICE_IDFA		    = "idfa";
		public readonly static string DEVICE_IDFV		    = "idfv";
		public readonly static string DEVICE_IS_HACKED	    = "is_hacked";

		//Event Context
		public readonly static string EVENT_INDEX	 	    = "event_index"; 
		public readonly static string EVENT_PREVIOUS_EVENT  = "previous_event"; 
		public readonly static string EVENT_SENDBOX_MODE    = "sandbox_mode";
        public readonly static string EVENT_TRANSACTION_ID  = "event_bundle_id";
		public readonly static string EVENT_IS_ONLINE  		= "is_online";

        // Geo-Location Context
        public readonly static string GEO_LAT               = "latitude";
		public readonly static string GEO_LONG              = "longitude";
		public readonly static string GEO_LAT_LONG_ACC      = "latitudeLongitudeAccuracy";
		public readonly static string GEO_ALT               = "altitude";
		public readonly static string GEO_ALT_ACC           = "altitudeAccuracy";
		public readonly static string GEO_BEARING           = "bearing";
		public readonly static string GEO_SPEED             = "speed";
		public readonly static string GEO_TIMESTAMP         = "timestamp";

		// Commmon Payload Keys
		public readonly static string SCHEMA                = "schema";
		public readonly static string DATA                  = "data";
		public readonly static string EVENT                 = "e";
		public readonly static string EID                   = "eid";
		public readonly static string TIMESTAMP             = "dtm";
		public readonly static string SENT_TIMESTAMP        = "stm";
		public readonly static string TRACKER_VERSION       = "tv";
		public readonly static string APP_ID                = "aid";
		public readonly static string NAMESPACE             = "tna";
		public readonly static string UID                   = "uid";
		public readonly static string INSTALLATION_ID       = "iid";
		public readonly static string CONTEXT               = "co";
		public readonly static string CONTEXT_ENCODED       = "cx";
		public readonly static string UNSTRUCTURED          = "ue_pr";
		public readonly static string UNSTRUCTURED_ENCODED  = "ue_px";
		public readonly static string API_KEY               = "ak";
		
		// Subject Keys
		public readonly static string PLATFORM              = "p";
		public readonly static string RESOLUTION            = "res";
		public readonly static string VIEWPORT              = "vp";
		public readonly static string COLOR_DEPTH           = "cd";
		public readonly static string TIMEZONE              = "tz";
		public readonly static string LANGUAGE              = "lang";
		public readonly static string IP_ADDRESS            = "ip";
		public readonly static string USERAGENT             = "ua";
		public readonly static string DOMAIN_UID            = "duid";
		public readonly static string NETWORK_UID           = "tnuid";
		
		// Page View
		public readonly static string PAGE_URL              = "url";
		public readonly static string PAGE_TITLE            = "page";
		public readonly static string PAGE_REFR             = "refr";
		
		// Structured Event
		public readonly static string SE_CATEGORY           = "se_ca";
		public readonly static string SE_ACTION             = "se_ac";
		public readonly static string SE_LABEL              = "se_la";
		public readonly static string SE_PROPERTY           = "se_pr";
		public readonly static string SE_VALUE              = "se_va";
		
		// Ecomm Transaction
		public readonly static string TR_ID                 = "tr_id";
		public readonly static string TR_TOTAL              = "tr_tt";
		public readonly static string TR_AFFILIATION        = "tr_af";
		public readonly static string TR_TAX                = "tr_tx";
		public readonly static string TR_SHIPPING           = "tr_sh";
		public readonly static string TR_CITY               = "tr_ci";
		public readonly static string TR_STATE              = "tr_st";
		public readonly static string TR_COUNTRY            = "tr_co";
		public readonly static string TR_CURRENCY           = "tr_cu";
		
		// Transaction Item
		public readonly static string TI_ITEM_ID            = "ti_id";
		public readonly static string TI_ITEM_SKU           = "ti_sk";
		public readonly static string TI_ITEM_NAME          = "ti_nm";
		public readonly static string TI_ITEM_CATEGORY      = "ti_ca";
		public readonly static string TI_ITEM_PRICE         = "ti_pr";
		public readonly static string TI_ITEM_QUANTITY      = "ti_qu";
		public readonly static string TI_ITEM_CURRENCY      = "ti_cu";
		
		// Screen View
		public readonly static string SV_ID                 = "id";
		public readonly static string SV_NAME               = "name";
		
		// User Timing
		public readonly static string UT_CATEGORY           = "category";
		public readonly static string UT_VARIABLE           = "variable";
		public readonly static string UT_TIMING             = "timing";
		public readonly static string UT_LABEL              = "label";
	}
}
