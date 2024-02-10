using System;
using System.Security.Cryptography.X509Certificates;
using Marketplace.Domain;
using Raven.Client.Documents;

namespace Marketplace.Framework
{
// The `DocumentStoreHolder` class holds a single Document Store instance.
    public class DocumentStoreHolder
    {
        // Use Lazy<IDocumentStore> to initialize the document store lazily. 
        // This ensures that it is created only once - when first accessing the public `Store` property.
        private static Lazy<IDocumentStore> store = new(CreateStore);

        public static IDocumentStore Store => store.Value;

        public static IDocumentStore CreateStore()
        {
            IDocumentStore store = new DocumentStore()
            {
                // Define the cluster node URLs (required)
                Urls = new[] { "http://localhost:8080", 
                    /*some additional nodes of this cluster*/ },

                // Set conventions as necessary (optional)
                Conventions =
                {
                    MaxNumberOfRequestsPerSession = 10,
                    UseOptimisticConcurrency = true,
                    FindIdentityProperty = i => i.Name == "_databesId"
                },

                // Define a default database (optional)
                Database = "Marketplace",

                // Define a client certificate (optional)
                Certificate = new X509Certificate2("C:\\path_to_your_pfx_file\\cert.pfx"),
                    
                /*
                  store.Conventions.RegisterAsyncIdConvention<ClassifiedAd> (dbName, entity) =>
                    Task.FromResult("ClassifiedAd/" + entity.Id.ToString()); 
                 */
                
                // Initialize the Document Store
            }.Initialize();

            return store;
        }
    }
}