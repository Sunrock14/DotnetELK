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
    public async Task<IActionResult> SearchData(string term)
    {
        var searchHelper = new ElasticService<Product>(elasticsearchUrl, defaultIndex);
        var results = await searchHelper.SearchAsync(term, "name", defaultIndex);
        return Ok(results);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> AddData()
    {
        var products = dataService.ProductCreate();
        var searchHelper = new ElasticService<Product>(elasticsearchUrl, defaultIndex);
        var create = await searchHelper.CreateIndexAsync(defaultIndex);
        var a = await searchHelper.BulkIndexAsync(products, defaultIndex);
        return Ok(a);
    }
    [HttpDelete("[action]")]
    public async Task<IActionResult> DeleteData(string id)
    {
        var products = dataService.ProductCreate();
        var searchHelper = new ElasticService<Product>(elasticsearchUrl, defaultIndex);
        var a = await searchHelper.DeleteDocumentAsync(id, defaultIndex);
        return Ok(a);
    }
}
