/*
 * GetRequest.cs
 * SnowplowTracker.Requests
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
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using SnowplowTracker.Collections;
using SnowplowTracker.Wrapper;
using UnityEngine;
using UnityEngine.Networking;

namespace SnowplowTracker.Requests
{
    internal class WebReadyRequest
    {
        /*
        private static readonly HttpClient client = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(10)
        };
        */
        int timeout = 10;

        private readonly HttpRequest request;
        private readonly List<Guid> rowIds;
        private readonly bool oversize;
        private readonly ConcurrentQueue<RequestResult> resultQueue;

        /// <summary>
        /// Initializes a new instance of the <see cref="SnowplowTracker.Requests.ReadyRequest"/> class.
        /// </summary>
        /// <param name="request">Request.</param>
        /// <param name="rowIds">Row identifiers.</param>
        /// <param name="oversize">If set to <c>true</c> oversize.</param>
        /// <param name="resultQueue">The queue that sent events will be added to</param>
        public WebReadyRequest(HttpRequest request, List<Guid> rowIds, bool oversize, ConcurrentQueue<RequestResult> resultQueue)
        {
            this.request = request;
            this.rowIds = rowIds;
            this.oversize = oversize;
            this.resultQueue = resultQueue;
        }

        /// <summary>
        /// Send a blocking request.
        /// </summary>
        public void Send()
        {
            //posalji request sa unity merodama i napuni queue
            try {
                Log.Debug("Send http request");
                UnityMainThreadDispatcher.Instance.Enqueue(SendUnityRequest(request.CollectorUri.ToString(), request.StringContent));
                /*
                Task<HttpResponseMessage> response = null;

                switch (request.Method)
                {
                    case Enums.HttpMethod.POST:
                        response = client.PostAsync(request.CollectorUri, request.Content);
                        break;
                    case Enums.HttpMethod.GET:
                        response = client.GetAsync(request.CollectorUri);
                        break;
                }

                AddToResultQueue(response.Result);
                */
            }
            catch (Exception e)
            {
                Log.Error("WebRequesr ex: " + e.Message);
                AddToResultQueue(false);
            }
        }

        private IEnumerator SendUnityRequest(string uri, string postData) {
            Log.Debug(uri);
            Log.Debug(postData);
            UnityWebRequest uwr = UnityWebRequest.Post(uri, postData);
            uwr.SetRequestHeader("Content-Type", "application/json");
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError)
            {
                Log.Debug("Error While Sending: " + uwr.error);
                AddToResultQueue(false);
            }
            else if(uwr.isHttpError)
            {
                Log.Debug("Error While Sending isHttpError: " + uwr.error);
                AddToResultQueue(false);
                
            } else { 
                Log.Debug("Received: " + uwr.downloadHandler.text);
                AddToResultQueue(true);                
            }
        }

        private void AddToResultQueue(bool okResponse)
        {
            var success = oversize || okResponse;
            resultQueue.Enqueue(new RequestResult(success, rowIds));
        }
    }
}
