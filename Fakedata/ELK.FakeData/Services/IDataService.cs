using ELK.FakeData.Fakers;
using ELK.FakeData.Models;
using ELK.FakeData.Utils;

namespace ELK.FakeData.Services;

public interface IDataService
{
    List<Product> ProductCreate();
}

public class DataManager : IDataService
{
    public List<Product> ProductCreate()
    {
        var result = new DataGenerator();
        var products = DataGenerator.GenerateByCount<Product, ProductFaker>(100);
        return products;
    }
}