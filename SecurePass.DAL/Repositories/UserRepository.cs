using SecurePass.Data;
using SecurePass.DAL.Contracts;
using SecurePass.Data.Entities;

namespace SecurePass.DAL.Repositories;

public class UserRepository : CrudBaseRepository<User>, IUserRepository
{
    public UserRepository(SecurePassDBContext dbContext) : base(dbContext)
    {
        
    }
}

