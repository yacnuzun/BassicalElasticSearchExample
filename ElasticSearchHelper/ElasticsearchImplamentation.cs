using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;

namespace ElasticSearchHelper
{
    public class ElasticSearchImplamentation<T> where T : class
    {
        private readonly ElasticsearchClient _client;
        private readonly string _indexName;

        // Constructor to initialize the Elasticsearch client and index name
        public ElasticSearchImplamentation(string url, string indexName)
        {
            ElasticsearchClientSettings settings = new(new Uri(url));
            settings.DefaultIndex("products");
            _client = new ElasticsearchClient(settings);
            _indexName = indexName;
        }

        //// Index a document (create or update)
        //public async Task IndexDocumentAsync(T document, string id)
        //{
        //    var response = await _client.IndexDocumentAsync(document);
        //    if (!response.IsValid)
        //    {
        //        throw new Exception($"Failed to index document: {response.OriginalException.Message}");
        //    }
        //}

        // Get a document by its ID
        public async Task<T> GetDocumentAsync(string id)
        {
            var response = await _client.GetAsync<T>(id);
            if (!response.Found)
            {
                throw new Exception($"Document with ID {id} not found");
            }
            return response.Source;
        }

        // Search documents based on a query
        public async Task<IReadOnlyCollection<T>> SearchAsync(string searchTerm)
        {
            SearchRequest searchRequest = new("products")
            {
                Size= 100,
                Sort = new List<SortOptions>
                { SortOptions.Field(new Field("name.keyword"), new FieldSort(){Order=SortOrder.Asc}) },
                Query = new FuzzyQuery(new Field("name")) 
                {
                    Value= searchTerm
                }
            };
            var response = await _client.SearchAsync<T>(searchRequest);

            return response.Documents;
        }



        //// Update a document by ID
        //public async Task UpdateDocumentAsync(string id, T document)
        //{
        //    var response = await _client.UpdateAsync<T>(id, u => u.Doc(document));
            
        //}

        // Delete a document by ID
        public async Task DeleteDocumentAsync(string id)
        {
            var response = await _client.DeleteAsync<T>(id);
        }

        // Check if an index exists
        public async Task<bool> IndexExistsAsync()
        {
            var response = await _client.Indices.ExistsAsync(_indexName);
            return response.Exists;
        }

        public async Task CreateIndexwithIdAsync(T product)
        {
            var responseCreated = await _client.CreateAsync<T>(product);
            
            if (!responseCreated.IsSuccess())
            {
                throw new Exception($"Failed to create index:");
            }

        }
    }
}
