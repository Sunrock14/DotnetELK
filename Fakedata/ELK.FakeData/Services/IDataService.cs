using ELK.FakeData.Fakers;
using ELK.FakeData.Models;
using ELK.FakeData.Utils;

namespace ELK.FakeData.Services;

public interface IDataService
{
    List<Product> ProductCreate(int quantity);
}

public class DataManager : IDataService
{
    public List<Product> ProductCreate(int quantity = 10)
    {
        var result = new DataGenerator();
        var products = DataGenerator.GenerateByCount<Product, ProductFaker>(quantity);
        return products;
    }
}