// Elasticsearch URL (localhost:9200 for default)
using Elastic.Clients.Elasticsearch.Nodes;
using ElasticSearchHelper;

var elasticsearchUrl = "http://localhost:9200";
var indexName = "products";

// ElasticsearchHelper instance
var helper = new ElasticSearchImplamentation<Product>(elasticsearchUrl, indexName);

var x = await helper.IndexExistsAsync();

// Create a new product
var product = new Product
{
    Id = "5",
    Name = "Laptop",
    Description = "A powerful laptop",
    Price = 1200.99m
};

var productTwo = new Product
{
    Id = "6",
    Name = "LaptopTwo",
    Description = "A powerful laptop 2",
    Price = 11200.99m
};


await helper.CreateIndexwithIdAsync(product);
await helper.CreateIndexwithIdAsync(productTwo);


// Search for products with the term "laptop"
var searchResponse = await helper.SearchAsync("laptop");

foreach (var result in searchResponse)
{
    Console.WriteLine($"Found product: {result.Name} - {result.Price}");
}

// Get the product by ID
var retrievedProduct = await helper.GetDocumentAsync(product.Id);

Console.WriteLine($"Retrieved product: {retrievedProduct.Name}{retrievedProduct.Price}");

// Update the product
product.Price = 1100.99m;
//await helper.UpdateDocumentAsync(product.Id, product);

// Delete the product
await helper.DeleteDocumentAsync(product.Id);

Console.WriteLine("press any button");

Console.ReadKey();