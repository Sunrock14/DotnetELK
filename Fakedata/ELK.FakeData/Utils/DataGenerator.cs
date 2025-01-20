using Bogus;

namespace ELK.FakeData.Utils;

public class DataGenerator
{
    private readonly Faker _faker;

    public DataGenerator()
    {
        // Türkçe veri üretmek için
        // Veya Appsettingsten çekebilirsin
        _faker = new Faker("tr");
    }
    public class TrDataGenerator<T> : Faker<T> where T : class
    {
        public TrDataGenerator() : base("tr")
        {
        }
    }

    public static List<T> GenerateByCount<T, F>(int count) where T : class, new() where F : Faker<T>, new()
    {
        return new F().Generate(count);
    }

    public static T GenerateSingle<T, F>() where T : class, new() where F : Faker<T>, new()
    {
        return new F().Generate(1).Single();
    }
}