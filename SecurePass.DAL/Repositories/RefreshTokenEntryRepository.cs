using SecurePass.Data;
using SecurePass.DAL.Contracts;
using SecurePass.Data.Entities;

namespace SecurePass.DAL.Repositories;

public class RefreshTokenEntryRepository : CrudBaseRepository<RefreshTokenEntry>, IRefreshTokenEntryRepository
{
    public RefreshTokenEntryRepository(SecurePassDBContext dbContext) : base(dbContext)
    {

    }
}

