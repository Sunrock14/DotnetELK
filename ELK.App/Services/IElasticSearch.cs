using Elastic.Clients.Elasticsearch.QueryDsl;
using ELK.App.Services;

public interface IElasticService
{
    /// <summary>  
    /// Yeni bir Elasticsearch indeksi oluşturur  
    /// </summary>  
    Task<bool> CreateIndexAsync(string indexName);

    /// <summary>  
    /// Tek bir dökümanı indeksler  
    /// </summary>  
    Task<bool> IndexDocumentAsync<T>(
        T document, 
        string id, 
        string indexName) 
        where T : class;

    /// <summary>  
    /// Çoklu döküman indeksleme işlemi yapar  
    /// </summary>  
    Task<bool> BulkIndexAsync<T>(
        IEnumerable<T> documents, 
        string indexName) 
        where T : class;

    /// <summary>  
    /// ID ile döküman getirir  
    /// </summary>  
    Task<T> GetDocumentAsync<T>(
        string indexName, 
        string id) 
        where T : class;

    /// <summary>  
    /// Var olan dökümanı günceller  
    /// </summary>  
    Task<bool> UpdateDocumentAsync<T>(
        string indexName, 
        string id, 
        T document) 
        where T : class;

    /// <summary>  
    /// ID ile döküman siler  
    /// </summary>  
    Task<bool> DeleteDocumentAsync<T>(
        string indexName, 
        string id) 
        where T : class;

    /// <summary>  
    /// Tek alanda metin araması yapar  
    /// </summary>  
    Task<SearchResult<T>> SearchAsync<T>(
        string searchText, 
        string field, 
        string indexName, 
        int page = 1, 
        int pageSize = 10) 
        where T : class;

    /// <summary>  
    /// Birden fazla alanda metin araması yapar  
    /// </summary>  
    Task<SearchResult<T>> SearchMultipleFieldsAsync<T>(
        string searchText, 
        string[] fields, 
        string indexName, 
        int page = 1, 
        int pageSize = 10) 
        where T : class;

    /// <summary>  
    /// Özel sorgu ile arama yapar  
    /// </summary>  
    Task<SearchResult<T>> FilterAsync<T>(
        Func<QueryDescriptor<T>, QueryDescriptor<T>> query, 
        string indexName, 
        int page = 1, int 
        pageSize = 10) 
        where T : class;
}