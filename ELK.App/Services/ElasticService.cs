using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Elastic.Transport;
using Microsoft.Extensions.Options;

namespace ELK.App.Services;

public class ElasticService : IElasticService
{
    private readonly ElasticsearchClient _client;
    private readonly ElasticsearchSettings _settings;

    public ElasticService(IOptions<ElasticsearchSettings> settings)
    {
        _settings = settings.Value;

        var clientSettings = new ElasticsearchClientSettings(new Uri(_settings.Url))
            .DefaultIndex(_settings.DefaultIndex);

        if (!string.IsNullOrEmpty(_settings.Username) && !string.IsNullOrEmpty(_settings.Password))
        {
            clientSettings.Authentication(new BasicAuthentication(_settings.Username, _settings.Password));
        }

        _client = new ElasticsearchClient(clientSettings);
    }

    public async Task<bool> CreateIndexAsync(
        string indexName)
    {
        var response = await _client.Indices.CreateAsync(indexName, c => c
            .Settings(s => s
                .NumberOfShards(1)
                .NumberOfReplicas(1)
            ));

        return response.IsValidResponse;
    }

    public async Task<bool> IndexDocumentAsync<T>(
        T document, 
        string id, 
        string indexName) where T : class
    {
        var response = await _client.IndexAsync(document, i => i.Index(indexName).Id(id));
        return response.IsValidResponse;
    }

    public async Task<bool> BulkIndexAsync<T>(
        IEnumerable<T> documents, 
        string indexName) where T : class
    {
        var bulkAll = await _client.BulkAsync(b => b
            .Index(indexName)
            .IndexMany(documents));

        return bulkAll.IsValidResponse;
    }

    public async Task<T> GetDocumentAsync<T>(
        string indexName, 
        string id) where T : class
    {
        var response = await _client.GetAsync<T>(id, g => g.Index(indexName));
        return response.IsValidResponse ? response.Source : null;
    }

    public async Task<bool> UpdateDocumentAsync<T>(
        string indexName, 
        string id, 
        T document) where T : class
    {
        var response = await _client.UpdateAsync<T, T>(indexName, id, u => u.Doc(document));
        return response.IsValidResponse;
    }

    public async Task<bool> DeleteDocumentAsync<T>(
        string indexName, 
        string id) where T : class
    {
        var response = await _client.DeleteAsync<T>(indexName, id);
        return response.IsValidResponse;
    }

    public async Task<SearchResult<T>> SearchAsync<T>(
        string searchText, 
        string field, 
        string indexName, 
        int page = 1, 
        int pageSize = 10) where T : class
    {
        var response = await _client.SearchAsync<T>(s => s
            .Index(indexName)
            .From((page - 1) * pageSize)
            .Size(pageSize)
            .Query(q => q
                .Match(m => m
                    .Field(field)
                    .Query(searchText)
                )
            ));

        return new SearchResult<T>
        {
            Items = response.IsValidResponse ? response.Documents.ToList() : new List<T>(),
            Total = response.Total,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<SearchResult<T>> SearchMultipleFieldsAsync<T>(
        string searchText, 
        string[] fields, 
        string indexName, 
        int page = 1, 
        int pageSize = 10) where T : class
    {
        var response = await _client.SearchAsync<T>(s => s
            .Index(indexName)
            .From((page - 1) * pageSize)
            .Size(pageSize)
            .Query(q => q
                .MultiMatch(mm => mm
                    .Fields(fields)
                    .Query(searchText)
                )
            ));

        return new SearchResult<T>
        {
            Items = response.IsValidResponse ? response.Documents.ToList() : new List<T>(),
            Total = response.Total,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<SearchResult<T>> FilterAsync<T>(
        Func<QueryDescriptor<T>, QueryDescriptor<T>> query,
        string indexName,
        int page = 1,
        int pageSize = 10) where T : class
    {
        var response = await _client.SearchAsync<T>(s => s
            .Index(indexName)
            .From((page - 1) * pageSize)
            .Size(pageSize)
            .Query(q => query(q))
        );

        return new SearchResult<T>
        {
            Items = response.IsValidResponse ? response.Documents.ToList() : new List<T>(),
            Total = response.Total,
            Page = page,
            PageSize = pageSize
        };
    }
}