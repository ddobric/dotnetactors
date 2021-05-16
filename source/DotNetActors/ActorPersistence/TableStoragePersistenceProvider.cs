// Copyright (c) Damir Dobric. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotnetActorClientPair.Net
{
    /// <summary>
    /// Represents the table storage persistance class
    /// </summary>
    public class TableStoragePersistenceProvider : IPersistenceProvider
    {
        const string cConnStr = "StorageConnectionString";

        private string actorSystemId;

        private CloudTableClient tableClient;

        private ILogger logger;

        private CloudTable table;

        /// <summary>
        /// Represents the initialize method
        /// </summary>
        /// <param name="actorSystemId"> Represents the Actor System ID</param>
        /// <param name="settings">Represents Settings as a dictionary</param>
        /// <param name="purgeOnStart">Represents Purge on Start value as boolen</param>
        /// <param name="logger">Represents logger parameter</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task InitializeAsync(string actorSystemId, Dictionary<string, object> settings, bool purgeOnStart = false, ILogger logger = null)
        {
            this.actorSystemId = actorSystemId;

            this.logger = logger;

            if (!settings.ContainsKey(cConnStr))
                throw new ArgumentException($"'{cConnStr}' argument must be contained in settings.");

            CloudStorageAccount storageAccount = CreateFromConnectionString(settings[cConnStr] as string);

            // Create a table client for interacting with the table service
            tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());

            string tableName = $"ActorSystemInstance{actorSystemId}";

            table = tableClient.GetTableReference(tableName);

            if (purgeOnStart == true)
                await Purge();

            if (await table.CreateIfNotExistsAsync())
            {
                this.logger?.LogTrace("Created Table : {0}", tableName);
            }
            else
            {
                this.logger?.LogTrace("Created Table : {0}", tableName);
            }
        }

        /// <summary>
        /// Represents the method to load the actor
        /// </summary>
        /// <param name="actorId"> Represents the Actor ID</param>
        /// <returns></returns>
        public async Task<ActorBase> LoadActor(ActorId actorId)
        {
            this.logger?.LogTrace("Loading actor: {0}", actorId);

            var actorInstance = await GetPersistedActorAsync(actorId);

            if (actorInstance == null)
                this.logger?.LogTrace("Actor: {0} was not found in persistence store.", actorId);
            else
                this.logger?.LogTrace("Actor: {0} loaded from persistence store.", actorId);

            return actorInstance;
        }


        /// <summary>
        /// Represents the method to Persist the Actor
        /// </summary>
        /// <param name="actorInstance">Represents the param Actor instance</param>
        /// <returns></returns>
        public async Task PersistActor(ActorBase actorInstance)
        {
            this.logger?.LogTrace("Persisting actor: {0}", actorInstance.Id);

            var serializedEntity = SerializeActor(actorInstance);

            ActorEntity actorEntity = new ActorEntity(this.actorSystemId, actorInstance.Id.IdAsString);
            actorEntity.SerializedActor = serializedEntity;

            await InsertOrMergeEntityAsync(this.table, new ActorEntity(this.actorSystemId, actorInstance.Id.IdAsString) { SerializedActor = serializedEntity });

            this.logger?.LogTrace("Persisting actor: {0}", actorInstance.Id);
        }


        /// <summary>
        /// Represents the Method to purge
        /// </summary>
        /// <returns></returns>
        public async Task Purge()
        {
            this.logger?.LogTrace("Purge started");

            await this.table.DeleteIfExistsAsync();

            this.logger?.LogTrace("Purge completed");
        }

        #region Private Methods

        private async Task<ActorBase> GetPersistedActorAsync(string actorId)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<ActorEntity>(this.actorSystemId, actorId);
            TableResult result = await this.table.ExecuteAsync(retrieveOperation);
            ActorEntity customer = result.Result as ActorEntity;

            if (customer == null)
                return null;
            else
                return DeserializeActor<ActorBase>(customer.SerializedActor);
        }

        private static async Task<ActorEntity> InsertOrMergeEntityAsync(CloudTable table, ActorEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Entity cannot be null!");
            }
            try
            {
                // Create the InsertOrReplace table operation
                TableOperation insertOrMergeOperation = TableOperation.InsertOrMerge(entity);

                // Execute the operation.
                TableResult result = await table.ExecuteAsync(insertOrMergeOperation);
                ActorEntity insertedActor = result.Result as ActorEntity;

                // Get the request units consumed by the current operation. RequestCharge of a TableResult is only applied to Azure CosmoS DB 
                if (result.RequestCharge.HasValue)
                {
                    Console.WriteLine("Request Charge of InsertOrMerge Operation: " + result.RequestCharge);
                }

                return insertedActor;
            }
            catch (StorageException ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
                throw;
            }
        }

        private static CloudStorageAccount CreateFromConnectionString(string storageConnectionString)
        {
            CloudStorageAccount storageAccount;
            try
            {
                storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the application.");
                throw;
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the sample.");
                Console.ReadLine();
                throw;
            }

            return storageAccount;
        }

        internal static string SerializeActor(ActorBase actorInstance)
        {
            JsonSerializerSettings sett = new JsonSerializerSettings();
            sett.TypeNameHandling = TypeNameHandling.All;

            var strObj = JsonConvert.SerializeObject(actorInstance, sett);

            return strObj;
        }

        internal static T DeserializeActor<T>(string serializedActor)
        {
            JsonSerializerSettings sett = new JsonSerializerSettings();

            sett.TypeNameHandling = TypeNameHandling.All;

            var strObj = JsonConvert.DeserializeObject<T>(serializedActor, sett);

            return strObj;
        }
        
        #endregion
    }
}
