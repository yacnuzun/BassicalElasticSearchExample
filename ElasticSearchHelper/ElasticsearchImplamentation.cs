using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;

namespace ElasticSearchHelper
{
    public class ElasticSearchImplamentation<T>: IElasticSearchInterface<T>
        where T : class,IBaseModel,new()
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
        public async Task<List<T>> GetAllAsync()
        {
            SearchResponse<T> searchResponse = await _client.SearchAsync<T>(s => s
            .Index(_indexName)
            //.Query(q => q.MatchAll())
            .Size(1000));  

            if (searchResponse.IsValidResponse)
            {
                return searchResponse.Documents.ToList();
            }
            else
            {
                return null;
            }
        }

        // Search documents based on a query
        public async Task<List<T>> SearchAsync(string searchTerm)
        {
            SearchRequest searchRequest = new(_indexName)
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

            return response.Documents.ToList();
        }



        // Update a document by ID
        public async Task<bool> UpdateDocumentAsync(BaseDTO request)
        {
            UpdateRequest<T, BaseDTO> updateRequest = new(_indexName, request.Id)
            {Doc=request };
            var response = await _client.UpdateAsync(updateRequest);
            return response.IsSuccess()?true:false;

        }

        // Delete a document by ID
        public async Task DeleteDocumentAsync(string id)
        {
            DeleteRequest<T> deleteRequest = new(_indexName, id);
            var response = await _client.DeleteAsync(deleteRequest);
        }

        // Check if an index exists
        public async Task<bool> IndexExistsAsync()
        {
            var response = await _client.Indices.ExistsAsync(_indexName);
            return response.Exists;
        }

        public async Task<bool> CreateIndexwithIdAsync(T product)
        {
            var responseCreated = await _client.CreateAsync<T>(product);
            
            if (!responseCreated.IsSuccess())
            {
                throw new Exception($"Failed to create index:");
            }

            else
            {
                return true;
            }

        }

        public Task UpdateDocumentAsync(UpdateProductDTO request, T document)
        {
            throw new NotImplementedException();
        }
    }
}
