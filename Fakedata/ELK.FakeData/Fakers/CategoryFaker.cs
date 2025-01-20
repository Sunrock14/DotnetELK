using ELK.FakeData.Models;
using static ELK.FakeData.Utils.DataGenerator;

namespace ELK.FakeData.Fakers;

public class CategoryFaker : TrDataGenerator<Category>
{
    public CategoryFaker()
    {
        RuleFor(c => c.Id, f => f.IndexFaker + 1);
        RuleFor(c => c.Name, f => f.Commerce.Categories(1)[0]);
        RuleFor(c => c.Description, f => f.Lorem.Sentence());
    }
}
