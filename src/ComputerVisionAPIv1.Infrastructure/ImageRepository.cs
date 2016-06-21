using ComputerVisionAPIv1.Infrastructure.Contracts;
using ComputerVisionAPIv1.Infrastructure.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerVisionAPIv1.Infrastructure
{
    public class ImageRepository : IImageRepository
    {
        private string endpoint;
        private string authKey;
        private string databaseId;
        private string collectionId;

        private DocumentClient client;
        private Database database;
        private DocumentCollection collection;

        public ImageRepository(string endpoint, string authKey, string databaseId, string collectionId)
        {
            this.endpoint = endpoint;
            this.authKey = authKey;
            this.databaseId = databaseId;
            this.collectionId = collectionId;
        }

        /// <summary>
        /// Represent Azure DocumentDB client service
        /// </summary>
        public DocumentClient Client
        {
            get
            {
                if (client == null)
                {
                    Uri endpointUri = new Uri(this.endpoint);
                    client = new DocumentClient(endpointUri, this.authKey);
                }

                return client;
            }
        }

        /// <summary>
        /// Represent context Database
        /// </summary>
        public Database Database
        {
            get
            {
                if (database == null)
                {
                    database = ReadOrCreateDatabase();
                }

                return database;
            }
        }

        /// <summary>
        /// Represent context collection
        /// </summary>
        public DocumentCollection Collection
        {
            get
            {
                if (collection == null)
                {
                    collection = ReadOrCreateCollection(Database.SelfLink);
                }

                return collection;
            }
        }

        /// <summary>
        /// Read or Create context database Id
        /// </summary>
        /// <returns></returns>
        private Database ReadOrCreateDatabase()
        {
            var database = this.Client.CreateDatabaseQuery()
                            .Where(d => d.Id == this.databaseId)
                            .AsEnumerable()
                            .FirstOrDefault();

            if (database == null)
            {
                database = this.Client.CreateDatabaseAsync(new Database { Id = this.databaseId }).Result;
            }

            return database;
        }

        /// <summary>
        /// Read or Create given collection Id
        /// </summary>
        /// <param name="databaseLink">Database self-link</param>
        /// <returns></returns>
        private DocumentCollection ReadOrCreateCollection(string databaseLink)
        {
            var collection = this.Client.CreateDocumentCollectionQuery(databaseLink)
                              .Where(c => c.Id == this.collectionId)
                              .AsEnumerable()
                              .FirstOrDefault();

            if (collection == null)
            {
                collection = this.Client.CreateDocumentCollectionAsync(databaseLink, new DocumentCollection { Id = this.collectionId }).Result;
            }

            return collection;
        }

        /// <summary>
        /// Creates Document in context collection
        /// </summary>
        /// <param name="image">Image Document</param>
        /// <returns></returns>
        public async Task<Document> CreateAsync(Image image)
        {
            if (string.IsNullOrEmpty(image.Id))
            {
                image.Id = GenerateImageId();
            }

            return await this.Client.CreateDocumentAsync(this.Collection.SelfLink, image);
        }

        /// <summary>
        /// Generates unique identifier for the document
        /// </summary>
        /// <returns>Unique string Identifier</returns>
        private string GenerateImageId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
