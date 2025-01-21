using ELK.App.Services;
using ELK.FakeData.Models;
using ELK.FakeData.Services;
using Microsoft.AspNetCore.Mvc;

namespace ELK.App.Controllers;

[ApiController]
[Route("[controller]")]
public class ELKController(IDataService dataService) : ControllerBase
{
    private string elasticsearchUrl = "http://localhost:9200/"; 
    private string defaultIndex = "products";

    [HttpGet("[action]")]
    public async Task<IActionResult> CreateIndex(string indexName)
    {
        var searchHelper = new ElasticService("http://localhost:9200", defaultIndex);
        bool isIndexCreated = await searchHelper.CreateIndexAsync("products");
        return Ok(isIndexCreated);
    }
    
    [HttpGet("[action]")]
    public async Task<IActionResult> IndexDocumentAsync(string indexName)
    {
        var searchHelper = new ElasticService("http://localhost:9200", "products");

        var product = new Product
        {
            Id = 101,
            Name = "Laptop",
            Description = "High performance laptop",
            Price = 1500
        };

        bool isIndexed = await searchHelper.IndexDocumentAsync(product, "101", "products");
        return Ok();
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> SearchData(string term)
    {
        var searchHelper = new ElasticService(elasticsearchUrl, defaultIndex);
        var results = await searchHelper.SearchAsync<Product>(term, "name", defaultIndex);
        return Ok(results);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> BulkIndex()
    {
        var products = dataService.ProductCreate();
        var searchHelper = new ElasticService(elasticsearchUrl, defaultIndex);
        var create = await searchHelper.CreateIndexAsync(defaultIndex);
        var a = await searchHelper.BulkIndexAsync(products, defaultIndex);
        return Ok(a);
    }
    [HttpDelete("[action]")]
    public async Task<IActionResult> DeleteData(string id)
    {
        var products = dataService.ProductCreate();
        var searchHelper = new ElasticService(elasticsearchUrl, defaultIndex);
        var a = await searchHelper.DeleteDocumentAsync<Product>(id, defaultIndex);
        return Ok(a);
    }

}
