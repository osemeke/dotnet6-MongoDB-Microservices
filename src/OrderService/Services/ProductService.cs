using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
//using OrderService.Data;
using ReportingService.Entities;
using ReportingService.Options;

namespace ReportingService.Services
{
    public class ProductService
    {
        private readonly IMongoCollection<Product> _collection;

        public ProductService(IOptions<AppSettings> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            _collection = client.GetDatabase(options.Value.Db)
                .GetCollection<Product>(options.Value.Collection);
        }

        public async Task Add(Product product)
        {
            await _collection.InsertOneAsync(product);
        }

        public async Task Create(Product product) =>
            await _collection.InsertOneAsync(product);

        public async Task Update(string id, Product product) =>
            await _collection.ReplaceOneAsync(x => x.Id == id, product);

        public async Task<Product> GetById(string id) =>
            await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<IEnumerable<Product>> GetAll() =>
            await _collection.Find(_=>true).ToListAsync();

        public async Task Delete(string id) =>
            await _collection.DeleteOneAsync(x => x.Id == id);

    }
}
