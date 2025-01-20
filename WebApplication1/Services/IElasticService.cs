namespace WebApplication1.Services;

public interface ISearchHelper<T> where T : class
{
    /// <summary>  
    /// Yeni bir Elasticsearch indeksi oluşturur  
    /// </summary>  
    Task<bool> CreateIndexAsync(string indexName);

    /// <summary>  
    /// Tek bir dökümanı indeksler  
    /// </summary>  
    Task<bool> IndexDocumentAsync(T document, string id);

    /// <summary>  
    /// Çoklu döküman indeksleme işlemi yapar  
    /// </summary>  
    Task<bool> BulkIndexAsync(IEnumerable<T> documents);

    /// <summary>  
    /// ID ile döküman getirir  
    /// </summary>  
    Task<T> GetDocumentAsync(string id);

    /// <summary>  
    /// Var olan dökümanı günceller  
    /// </summary>  
    Task<bool> UpdateDocumentAsync(string id, T document);

    /// <summary>  
    /// ID ile döküman siler  
    /// </summary>  
    Task<bool> DeleteDocumentAsync(string id);

    /// <summary>  
    /// Tek alanda metin araması yapar  
    /// </summary>  
    Task<List<T>> SearchAsync(string searchText, string field, int take = 10);

    /// <summary>  
    /// Birden fazla alanda metin araması yapar  
    /// </summary>  
    Task<List<T>> SearchMultipleFieldsAsync(string searchText, string[] fields, int take = 10);

    /// <summary>  
    /// Özel sorgu ile arama yapar  
    /// </summary>  
    Task<List<T>> FilterAsync(Func<QueryDescriptor<T>, Query> query, int take = 10);
}
