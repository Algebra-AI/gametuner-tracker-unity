using System;

namespace SnowplowTracker.Storage
{
    /// <summary>
    /// Extension of the EventStore to store additional data localy.
    /// </summary>
    internal class ExtendedEventStore : EventStore
    {

        public class EventsMetaData { 
            public string Id { get; set; }
            public string ValueString { get; set; }
            public int ValueInt { get; set; }
        }

        private const string COLLECTION_METADATA                        = "eventsData";
        private const string COLLECTION_METADATA_LAST_ADDED_EVENT       = "lastAddedEventName";
        private const string COLLECTION_METADATA_LAST_TRANSACTION_ID    = "lastTransactionId";
        private const string COLLECTION_METADATA_EVENT_INDEX            = "eventIndex";
        private const string COLLECTION_METADATA_USER_ID                = "userId";

        public ExtendedEventStore() : base() { 
            try
            {
                _dbLock.EnterWriteLock();

                var colData = _db.GetCollection<EventsMetaData>(COLLECTION_METADATA);
                colData.EnsureIndex("Key");
            }
            catch (Exception e)
            {
                Log.Error("Event Store: Failed to create metadata table");
                Log.Error(e.ToString());
                throw;
            }
            finally
            {
                _dbLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Gets the last event name.
        /// </summary>
        /// <returns>Event name</returns>
        public string GetLastAddedEvent() { 
            try
            {
                _dbLock.EnterReadLock();
                // Get event collection
                var colData = _db.GetCollection<EventsMetaData>(COLLECTION_METADATA);

                var result = colData.FindOne(x => x.Id == COLLECTION_METADATA_LAST_ADDED_EVENT);
                if (result == null) {
                    return string.Empty;
                }

                return result.ValueString;
            }
            catch (Exception e)
            {
                Log.Error($"EventStore: Get last added event failed");
                Log.Error(e.ToString());
                return string.Empty;
            }
            finally
            {
                _dbLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Update name of last added event.
        /// </summary>
        /// <param name="eventName">Event name to update</param>
        /// <returns>Is event name updated</returns>
        public bool UpdateLastAddedEvent(string eventName)
        {
            try
            {
                _dbLock.EnterWriteLock();
                // Get event collection
                var colData = _db.GetCollection<EventsMetaData>(COLLECTION_METADATA);
                EventsMetaData metaDataEventName = new EventsMetaData { Id = COLLECTION_METADATA_LAST_ADDED_EVENT, ValueString = eventName };

                if (!colData.Update(metaDataEventName)) {
                    colData.Insert(metaDataEventName);
                }
               
                return true;
            }
            catch (Exception e)
            {
                Log.Error("EventStore: Last event failed to save");
                Log.Error(e.ToString());
                return false;
            }
            finally
            {
                _dbLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Gets the event indix
        /// </summary>
        /// <returns>Event index</returns>
        public int GetEventIndex() { 
            try
            {
                _dbLock.EnterReadLock();
                // Get event collection
                var colData = _db.GetCollection<EventsMetaData>(COLLECTION_METADATA);

                var result = colData.FindOne(x => x.Id == COLLECTION_METADATA_EVENT_INDEX);
                if (result == null) {
                    return 0;
                }

                return result.ValueInt;
            }
            catch (Exception e)
            {
                Log.Error($"EventStore: Get event index failed");
                Log.Error(e.ToString());
                return 0;
            }
            finally
            {
                _dbLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Update event index
        /// </summary>
        /// <returns>Is event index updated</returns>
        public bool UpdateEventIndex()
        { 
            try
            {
                _dbLock.EnterWriteLock();
                // Get event collection
                var colData = _db.GetCollection<EventsMetaData>(COLLECTION_METADATA);

                int lastEventIndex = 0;
                var result = colData.FindOne(x => x.Id == COLLECTION_METADATA_EVENT_INDEX);
                if (result != null) {
                    lastEventIndex = result.ValueInt;
                }

                lastEventIndex += 1;
                EventsMetaData metaDataIndex = new EventsMetaData { Id = COLLECTION_METADATA_EVENT_INDEX, ValueInt = lastEventIndex };

                if (!colData.Update(metaDataIndex)) {
                    colData.Insert(metaDataIndex);
                }
               
                return true;
            }
            catch (Exception e)
            {
                Log.Error("EventStore: Last event failed to save");
                Log.Error(e.ToString());
                return false;
            }
            finally
            {
                _dbLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Gets last transaction id.
        /// </summary>
        /// <returns>ID of last transaction</returns>
        public int GetLastTransactionId() { 
            try
            {
                _dbLock.EnterReadLock();
                // Get event collection
                var colData = _db.GetCollection<EventsMetaData>(COLLECTION_METADATA);

                var result = colData.FindOne(x => x.Id == COLLECTION_METADATA_LAST_TRANSACTION_ID);
                if (result == null) {
                    return 0;
                }

                return result.ValueInt;
            }
            catch (Exception e)
            {
                Log.Error($"EventStore: Get last added event failed");
                Log.Error(e.ToString());
                return 0;
            }
            finally
            {
                _dbLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Update last transaction id.
        /// </summary>
        /// <returns>Is last transaction updated</returns>
        public bool UpdateLastTransactionId()
        {
            try
            {
                _dbLock.EnterWriteLock();
                // Get event collection
                var colData = _db.GetCollection<EventsMetaData>(COLLECTION_METADATA);

                var result = colData.FindOne(x => x.Id == COLLECTION_METADATA_LAST_TRANSACTION_ID);
                EventsMetaData metadata = new EventsMetaData { Id = COLLECTION_METADATA_LAST_TRANSACTION_ID, ValueInt = 0 };

                if (result == null) { 
                    colData.Insert(metadata);
                } else {
                    int transactionID = result.ValueInt + 1;
                    metadata.ValueInt = transactionID;
                    colData.Update(metadata);
                }
               
                return true;
            }
            catch (Exception e)
            {
                Log.Error("EventStore: Last transaction failed to save");
                Log.Error(e.ToString());
                return false;
            }
            finally
            {
                _dbLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Gets user id from cache.
        /// </summary>
        /// <returns>Cached userID</returns>
        public string GetCacheUserId() { 
            try
            {
                _dbLock.EnterReadLock();
                // Get event collection
                var colData = _db.GetCollection<EventsMetaData>(COLLECTION_METADATA);

                var result = colData.FindOne(x => x.Id == COLLECTION_METADATA_USER_ID);
                if (result == null) {
                    return string.Empty;
                }

                return result.ValueString;
            }
            catch (Exception e)
            {
                Log.Error($"EventStore: Get user id failed");
                Log.Error(e.ToString());
                return string.Empty;
            }
            finally
            {
                _dbLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Update user id.
        /// </summary>
        /// <returns>Is user id updated</returns>
        public bool UpdateUserId(string userID)
        {
            try
            {
                _dbLock.EnterWriteLock();
                // Get event collection
                var colData = _db.GetCollection<EventsMetaData>(COLLECTION_METADATA);

                var result = colData.FindOne(x => x.Id == COLLECTION_METADATA_USER_ID);
                EventsMetaData metadata = new EventsMetaData { Id = COLLECTION_METADATA_USER_ID, ValueString = userID };

                if (result == null) { 
                    colData.Insert(metadata);
                } else {
                    colData.Update(metadata);
                }
               
                return true;
            }
            catch (Exception e)
            {
                Log.Error("EventStore: UserID failed to save");
                Log.Error(e.ToString());
                return false;
            }
            finally
            {
                _dbLock.ExitWriteLock();
            }
        }
    }
}
