using backend.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Repositories;

public class RegistrationRequestRepository : IRegistrationRequestRepository
{
    private readonly IMongoCollection<RegistrationRequest> _collection;

    public RegistrationRequestRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<RegistrationRequest>("registrationRequests");
    }


    public async Task<IList<RegistrationRequest>> GetRegistrationRequestsByManagerName(string managerUsername)
    {
        var filter = Builders<RegistrationRequest>.Filter.Eq(r => r.ManagerUsername, managerUsername);
        return await _collection.Find(filter).ToListAsync();
    }


    public RegistrationRequest GetRegistrationRequest(string requestId)
    {
        return _collection.Find(r => r.Id == requestId).FirstOrDefault();
    }


    public RegistrationRequest AddRegistrationRequest(RegistrationRequest request)
    {
        _collection.InsertOne(request);
        return request;
    }


    public async Task<bool> DeleteRegistrationRequest(string requestId)
    {
        var result = await _collection.DeleteOneAsync(r => r.Id == requestId);
        return result.DeletedCount > 0;
    }


    public async Task<bool> ApproveRegistrationRequest(string requestId)
    {
        var filter = Builders<RegistrationRequest>.Filter.Eq(r => r.Id, requestId);
        var result = await _collection.DeleteOneAsync(filter);

        return result.DeletedCount > 0;
    }
}