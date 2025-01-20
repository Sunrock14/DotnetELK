using ELK.FakeData.Models;
using ELK.FakeData.Utils;
using static ELK.FakeData.Utils.DataGenerator;

namespace ELK.FakeData.Fakers;

public class ProductFaker : TrDataGenerator<Product>
{
    public ProductFaker()
    {
        
        RuleFor(p => p.Id, f => f.IndexFaker + 1);
        RuleFor(p => p.Name, f => f.Commerce.ProductName());
        RuleFor(p => p.Description, f => f.Commerce.ProductDescription());
        RuleFor(p => p.Price, f => decimal.Parse(f.Commerce.Price()));
        RuleFor(p => p.Stock, f => f.Random.Number(1, 1000));
        RuleFor(p => p.CreatedAt, f => f.Date.Past(1));
        RuleFor(p => p.Category, f => DataGenerator.GenerateSingle<Category, CategoryFaker>());
    }
}
