using SecurePass.Common.Models;

namespace SecurePass.Services.Contracts;

public interface IRecordService
{
    Task<RecordModel> AddRecord(CreateRecordRequestModel createRecordRequest);

    Task<RecordModel> GetUserRecords(int userId);

    Task<bool> UpdateRecord(UpdateRecordRequestModel updateRecordRequest);

    Task<bool> DeleteRecord(int recordId);

    Task<int> ShareRecord(ShareRecordRequestModel shareRecordRequest);

    Task<bool> UnshareRecord(int sharedRecordId);
}

