using SecurePass.Data;
using SecurePass.DAL.Contracts;
using SecurePass.Data.Entities;

namespace SecurePass.DAL.Repositories;

public class RecoverySettingRepository : CrudBaseRepository<RecoverySetting>, IRecoverySettingRepository
{
    public RecoverySettingRepository(SecurePassDBContext dbContext) : base(dbContext)
    {

    }
}

