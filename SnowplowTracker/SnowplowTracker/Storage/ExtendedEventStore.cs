using System;

namespace SnowplowTracker.Storage
{
    public class ExtendedEventStore : EventStore
    {

        public class EventsMetaData { 
            public string Id { get; set; }
            public string Value { get; set; }
        }

        private const string COLLECTION_METADATA = "eventsData";
        private const string COLLECTION_METADATA_LAST_ADDED_EVENT = "lastAddedEventName";
        private const string  COLLECTION_METADATA_LAST_TRANSACTION_ID = "lastTransactionId";

        public ExtendedEventStore() : base() { 
            try
            {
                var colData = _db.GetCollection<EventsMetaData>(COLLECTION_METADATA);
                colData.EnsureIndex("Key");
            }
            catch (Exception e)
            {
                Log.Error("Event Store: Failed to create metadata table");
                Log.Error(e.ToString());
                throw;
            }
        }


        public bool UpdateLastTriggeredEvent(string eventName)
        {
            try
            {
                _dbLock.EnterWriteLock();
                // Get event collection
                var colData = _db.GetCollection<EventsMetaData>(COLLECTION_METADATA);

                var result = colData.FindOne(x => x.Id == COLLECTION_METADATA_LAST_ADDED_EVENT);
                EventsMetaData metadata = new EventsMetaData { Id = COLLECTION_METADATA_LAST_ADDED_EVENT, Value = eventName };
                
                if (!colData.Update(metadata)) {
                    colData.Insert(metadata);
                }
               
                Log.Verbose("EventStore: Last event name updated");
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

                return result.Value;
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

                return Convert.ToInt32(result.Value);
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

        public bool UpdateLastTransactionId()
        {
            try
            {
                _dbLock.EnterWriteLock();
                // Get event collection
                var colData = _db.GetCollection<EventsMetaData>(COLLECTION_METADATA);

                var result = colData.FindOne(x => x.Id == COLLECTION_METADATA_LAST_TRANSACTION_ID);
                EventsMetaData metadata = new EventsMetaData { Id = COLLECTION_METADATA_LAST_TRANSACTION_ID, Value = "0" };

                if (result == null) { 
                    colData.Insert(metadata);
                } else {
                    int transactionID = Convert.ToInt32(result.Value);
                    transactionID += 1;
                    metadata.Value = transactionID.ToString();
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
    }
}
