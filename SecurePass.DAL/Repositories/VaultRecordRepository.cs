using SecurePass.Data;
using SecurePass.DAL.Contracts;
using SecurePass.Data.Entities;

namespace SecurePass.DAL.Repositories;

public class VaultRecordRepository : CrudBaseRepository<VaultRecord>, IVaultRecordRepository
{
    public VaultRecordRepository(SecurePassDBContext dbContext) : base(dbContext)
    {

    }
}

