using ELK.App.Services;
using ELK.FakeData.Models;
using ELK.FakeData.Services;
using Microsoft.AspNetCore.Mvc;

namespace ELK.App.Controllers;

[ApiController]
[Route("[controller]")]
public class ELKController(
    IDataService dataService, 
    IElasticService _elasticService) : 
    ControllerBase
{

    [HttpGet("[action]")]
    public async Task<IActionResult> CreateIndex(string indexName)
    {
        bool isIndexCreated = await _elasticService.CreateIndexAsync(indexName);
        return Ok(isIndexCreated);
    }
    [HttpGet("[action]")]
    public async Task<IActionResult> IndexDocumentAsync([FromBody] Product product)
    {
        bool isIndexed = await _elasticService.IndexDocumentAsync(product, "1", "products");
        return Ok(isIndexed);
    }
    [HttpPost("[action]")]
    public async Task<IActionResult> BulkIndex(int quantity)
    {
        var products = dataService.ProductCreate(quantity);
        var a = await _elasticService.BulkIndexAsync(products, "products");
        return Ok(a);
    }
    [HttpGet("[action]")]
    public async Task<IActionResult> GetData(string id)
    {
        var product = await _elasticService.GetDocumentAsync<Product>("products", id.ToString());
        return Ok(product);
    }
    [HttpDelete("[action]")]
    public async Task<IActionResult> DeleteData(string id)
    {
        var deleted = await _elasticService.DeleteDocumentAsync<Product>(id, "products");
        return Ok(deleted);
    }
    [HttpPut("[action]")]
    public async Task<IActionResult> UpdateData([FromBody] Product product)
    {
        bool isUpdated = await _elasticService.UpdateDocumentAsync("product", product.Id.ToString(), product);
        return Ok(isUpdated);
    }
    [HttpGet("[action]")]
    public async Task<IActionResult> SearchData(string term)
    {
        var results = await _elasticService.SearchAsync<Product>(term, "name", "products");
        return Ok(results);
    }
    [HttpGet("[action]")]
    public async Task<IActionResult> SearchMultipleFields(string term)
    {
        var fields = new[] { "name", "description" };
        var results = await _elasticService.SearchMultipleFieldsAsync<Product>(term, fields, "products");
        return Ok(results);
    }
    [HttpGet("[action]")]
    public async Task<IActionResult> CustomFilter()
    {
        var result = new List<SearchResult<Product>>();

        double greater = 799;
        double less = 800;
        var queryOne = await _elasticService.FilterAsync<Product>(q => q
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

        var queryTwo = await _elasticService.FilterAsync<Product>(q => q
            .RankFeature(r => r
                .Field(p => p.Price)
                .Boost(2)
            ),
            indexName: "products",
            page: 1,
            pageSize: 10
        );

        var queryThree = await _elasticService.FilterAsync<Product>(q => q
            .Term(t => t
                .Field(p => p.Name)
                .Value("product")
            ),
            indexName: "products",
            page: 1,
            pageSize: 10
        );

        var queryFour = await _elasticService.FilterAsync<Product>(q => q
            .Match(m => m
                .Field(p => p.Name)
                .Query("product")
            ),
            indexName: "products",
            page: 1,
            pageSize: 10
        );

        result.Add(queryOne);
        result.Add(queryTwo);
        result.Add(queryThree);
        result.Add(queryFour);

        return Ok(result);
    }
}
