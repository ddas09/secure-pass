using SecurePass.Data;
using SecurePass.DAL.Contracts;
using SecurePass.Data.Entities;

namespace SecurePass.DAL.Repositories;

public class TempUserRepository : CrudBaseRepository<TempUser>, ITempUserRepository
{
    public TempUserRepository(SecurePassDBContext dbContext) : base(dbContext)
    {

    }
}

