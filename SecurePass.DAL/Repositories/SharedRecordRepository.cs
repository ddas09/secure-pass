using SecurePass.Data;
using SecurePass.DAL.Contracts;
using SecurePass.Data.Entities;

namespace SecurePass.DAL.Repositories;

public class SharedRecordRepository : CrudBaseRepository<SharedRecord>, ISharedRecordRepository
{
    public SharedRecordRepository(SecurePassDBContext dbContext) : base(dbContext)
    {

    }
}


