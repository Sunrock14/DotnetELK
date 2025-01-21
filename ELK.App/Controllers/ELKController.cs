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
        bool isIndexCreated = await searchHelper.CreateIndexAsync(indexName);
        return Ok(isIndexCreated);
    }
    [HttpGet("[action]")]
    public async Task<IActionResult> IndexDocumentAsync([FromBody] Product product)
    {
        var searchHelper = new ElasticService("http://localhost:9200", "products");
        bool isIndexed = await searchHelper.IndexDocumentAsync(product, "1", "products");
        return Ok(isIndexed);
    }
    [HttpPost("[action]")]
    public async Task<IActionResult> BulkIndex(int quantity)
    {
        var products = dataService.ProductCreate(quantity);
        var searchHelper = new ElasticService(elasticsearchUrl, defaultIndex);
        var create = await searchHelper.CreateIndexAsync(defaultIndex);
        var a = await searchHelper.BulkIndexAsync(products, defaultIndex);
        return Ok(a);
    }
    [HttpGet("[action]")]
    public async Task<IActionResult> GetData(string id)
    {
        var searchHelper = new ElasticService(elasticsearchUrl, defaultIndex);
        var product = await searchHelper.GetDocumentAsync<Product>("products", id.ToString());
        return Ok(product);
    }
    [HttpDelete("[action]")]
    public async Task<IActionResult> DeleteData(string id)
    {
        var searchHelper = new ElasticService(elasticsearchUrl, defaultIndex);
        var a = await searchHelper.DeleteDocumentAsync<Product>(id, defaultIndex);
        return Ok(a);
    }
    [HttpPut("[action]")]
    public async Task<IActionResult> UpdateData([FromBody] Product product)
    {
        var searchHelper = new ElasticService(elasticsearchUrl, defaultIndex);
        bool isUpdated = await searchHelper.UpdateDocumentAsync("product", product.Id.ToString(), product);
        return Ok(isUpdated);
    }
    [HttpGet("[action]")]
    public async Task<IActionResult> SearchData(string term)
    {
        var searchHelper = new ElasticService(elasticsearchUrl, defaultIndex);
        var results = await searchHelper.SearchAsync<Product>(term, "name", defaultIndex);
        return Ok(results);
    }
    [HttpGet("[action]")]
    public async Task<IActionResult> SearchMultipleFields(string term)
    {
        var fields = new[] { "name", "description" };
        var searchHelper = new ElasticService(elasticsearchUrl, defaultIndex);
        var results = await searchHelper.SearchMultipleFieldsAsync<Product>(term, fields, "products");

        return Ok(results);
    }
    [HttpGet("[action]")]
    public async Task<IActionResult> Filter(double greater, double less)
    {
        var searchHelper = new ElasticService(elasticsearchUrl, defaultIndex);
        var result = await searchHelper.FilterAsync<Product>(q => q
            .Range(r => r
                .NumberRange(f => f
                    .Field(p => p.Price)
                    .Gte(greater)
                    .Lte(less)
                )
            ),
            indexName: "products",
            page: 1,
            pageSize: 10
        );
        return Ok(result);
    }


}
