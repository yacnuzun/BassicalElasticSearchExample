using ElasticSearchHelper;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchController : ControllerBase
    {
        

        private readonly ILogger<SearchController> _logger;
        private readonly ElasticSearchImplamentation<Product> _elasticSearchInterface = new ElasticSearchImplamentation<Product>("http://localhost:9200", "products");

        public SearchController(ILogger<SearchController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "getalldocuments")]
        public async Task<List<Product>> Documents()
        {
            var n = await _elasticSearchInterface.GetAllAsync();

            return n.ToList();
        }

        //[HttpGet(Name = "getstring")]
        //public async Task<List<Product>> Search(string x)
        //{
        //    var n = await _elasticSearchInterface.SearchAsync(x);

        //    return n;
        //}

        [HttpPost(Name = "create")]
        public async Task<bool> Create(Product product)
        {
            var response = await _elasticSearchInterface.CreateIndexwithIdAsync(product);

            return response;
        }

        [HttpPut(Name = "update")]
        public async Task<bool> Update(UpdateProductDTO updateProductDTO)
        {

            return await _elasticSearchInterface.UpdateDocumentAsync(updateProductDTO);
        }

        [HttpDelete(Name = "delete")]
        public async Task<int> Delete(string Id)
        {
            var response = _elasticSearchInterface.DeleteDocumentAsync(Id);
            return response.Id;
        }
    }
}