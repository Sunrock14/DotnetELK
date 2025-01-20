using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Microsoft.Extensions.Options;

namespace WebApplication1.Services
{
    public class SearchHelper<T> : ISearchHelper<T> where T : class
    {
        private readonly ElasticsearchClient _client;
        private readonly string _defaultIndex;
        private readonly IOptionsSnapshot<ElasticsearchSettings> _search;

        public SearchHelper(string url, string defaultIndex, string username = null, string password = null,
            IOptionsSnapshot<ElasticsearchSettings> search = null)
        {
            _defaultIndex = defaultIndex;

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

        public async Task<bool> IndexDocumentAsync(T document, string id)
        {
            var response = await _client.IndexAsync(document, i => i.Index(_defaultIndex).Id(id));
            return response.IsValidResponse;
        }

        public async Task<bool> BulkIndexAsync(IEnumerable<T> documents)
        {
            var bulkAll = await _client.BulkAsync(b => b
                .Index(_defaultIndex)
                .IndexMany(documents));

            return bulkAll.IsValidResponse;
        }

        public async Task<T> GetDocumentAsync(string id)
        {
            var response = await _client.GetAsync<T>(id, g => g.Index(_defaultIndex));
            return response.IsValidResponse ? response.Source : null;
        }

        public async Task<bool> UpdateDocumentAsync(string id, T document)
        {
            var response = await _client.UpdateAsync<T, T>(_defaultIndex, id, u => u
                .Doc(document));
            return response.IsValidResponse;
        }

        public async Task<bool> DeleteDocumentAsync(string id)
        {
            var response = await _client.DeleteAsync<T>(_defaultIndex, id);
            return response.IsValidResponse;
        }

        public async Task<List<T>> SearchAsync(string searchText, string field, int take = 10)
        {
            var response = await _client.SearchAsync<T>(s => s
                .Index(_defaultIndex)
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

        public async Task<List<T>> SearchMultipleFieldsAsync(string searchText, string[] fields, int take = 10)
        {
            var response = await _client.SearchAsync<T>(s => s
                .Index(_defaultIndex)
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

        public async Task<List<T>> FilterAsync(Func<QueryDescriptor<T>, Query> query, int take = 10)
        {
            var response = await _client.SearchAsync<T>(s => s
                .Index(_defaultIndex)
                .From(0)
                .Size(take)
                .Query(q => query(q))
            );

            return response.IsValidResponse ? response.Documents.ToList() : new List<T>();
        }
    }
}
