using SecurePass.Common.Models;

namespace SecurePass.Services.Contracts;

public interface IRecordService
{
    Task<RecordModel> AddRecord(CreateRecordRequestModel createRecordRequest);

    Task<VaultRecordContainerModel> GetUserRecords(int userId);

    Task<bool> UpdateRecord(UpdateRecordRequestModel updateRecordRequest);

    Task<bool> DeleteRecord(int userId, int recordId);

    Task<int> ShareRecord(ShareRecordRequestModel shareRecordRequest);

    Task<bool> UnshareRecord(int recipientId, int sharedRecordId);

    Task<List<UserModel>> GetRecordRecipients(int userId, int recordId);

    Task<bool> RevokeRecordAccess(int ownerId, int recordId, int recipientId);
}

