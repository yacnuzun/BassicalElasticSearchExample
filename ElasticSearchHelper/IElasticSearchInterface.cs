using Elastic.Clients.Elasticsearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearchHelper
{
    public interface IElasticSearchInterface<T> 
        where T : class,IBaseModel
    {
        Task<T> GetDocumentAsync(string id);
        Task<List<T>> SearchAsync(string searchTerm);
        Task UpdateDocumentAsync(UpdateProductDTO request, T document);
        Task DeleteDocumentAsync(string id);
        Task<bool> IndexExistsAsync();
        Task<bool> CreateIndexwithIdAsync(T product);
        Task<List<T>> GetAllAsync();
    }
}
