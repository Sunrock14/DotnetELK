using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Elastic.Transport;
using Microsoft.Extensions.Options;

namespace ELK.App.Services;

public class ElasticService<T> : IElasticService<T> where T : class
{
    private readonly ElasticsearchClient _client;
    private readonly IOptionsSnapshot<ElasticsearchSettings> _search;

    public ElasticService(string url, string defaultIndex, string username = null, string password = null,
        IOptionsSnapshot<ElasticsearchSettings> search = null)
    {
        var settings = new ElasticsearchClientSettings(new Uri(url))
            .DefaultIndex(defaultIndex);

        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
        {
            settings.Authentication(new BasicAuthentication(username, password));
        }

        _client = new ElasticsearchClient(settings);
        _search = search;
    }

    public async Task<bool> CreateIndexAsync(string indexName)
    {
        var response = await _client.Indices.CreateAsync(indexName, c => c
            .Settings(s => s
                .NumberOfShards(1)
                .NumberOfReplicas(1)
            ));

        return response.IsValidResponse;
    }

    public async Task<bool> IndexDocumentAsync(T document, string id, string indexName)
    {
        var response = await _client.IndexAsync(document, i => i.Index(indexName).Id(id));
        return response.IsValidResponse;
    }

    public async Task<bool> BulkIndexAsync(IEnumerable<T> documents, string indexName)
    {
        var bulkAll = await _client.BulkAsync(b => b
            .Index(indexName)
            .IndexMany(documents));

        return bulkAll.IsValidResponse;
    }

    public async Task<T> GetDocumentAsync(string id, string indexName)
    {
        var response = await _client.GetAsync<T>(id, g => g.Index(indexName));
        return response.IsValidResponse ? response.Source : null;
    }

    public async Task<bool> UpdateDocumentAsync(string id, T document, string indexName)
    {
        var response = await _client.UpdateAsync<T, T>(indexName, id, u => u
            .Doc(document));
        return response.IsValidResponse;
    }

    public async Task<bool> DeleteDocumentAsync(string id, string indexName)
    {
        var response = await _client.DeleteAsync<T>(indexName, id);
        return response.IsValidResponse;
    }

    public async Task<List<T>> SearchAsync(string searchText, string field, string indexName, int take = 10)
    {
        var response = await _client.SearchAsync<T>(s => s
            .Index(indexName)
            .From(0)
            .Size(take)
            .Query(q => q
                .Match(m => m
                    .Field(field)
                    .Query(searchText)
                )
            ));

        return response.IsValidResponse ? response.Documents.ToList() : new List<T>();
    }

    public async Task<List<T>> SearchMultipleFieldsAsync(string searchText, string[] fields, string indexName, int take = 10)
    {
        var response = await _client.SearchAsync<T>(s => s
            .Index(indexName)
            .From(0)
            .Size(take)
            .Query(q => q
                .MultiMatch(mm => mm
                    .Fields(fields)
                    .Query(searchText)
                )
            ));

        return response.IsValidResponse ? response.Documents.ToList() : new List<T>();
    }

    public async Task<List<T>> FilterAsync(Func<QueryDescriptor<T>, Query> query, string indexName, int take = 10)
    {
        var response = await _client.SearchAsync<T>(s => s
            .Index(indexName)
            .From(0)
            .Size(take)
            .Query(q => query(q))
        );

        return response.IsValidResponse ? response.Documents.ToList() : new List<T>();
    }
}