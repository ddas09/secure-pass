using AutoMapper;
using System.Transactions;
using SecurePass.Common.Models;
using SecurePass.DAL.Contracts;
using SecurePass.Data.Entities;
using SecurePass.Common.Constants;
using SecurePass.Common.Exceptions;
using SecurePass.Services.Contracts;

namespace SecurePass.Services;

public class RecordService(
    IMapper mapper,
    IVaultRecordRepository vaultRecordRepository,
    ISharedRecordRepository sharedRecordRepository
    ) : IRecordService
{
    private readonly IMapper _mapper = mapper;
    private readonly IVaultRecordRepository _vaultRecordRepository = vaultRecordRepository;
    private readonly ISharedRecordRepository _sharedRecordRepository = sharedRecordRepository;

    public async Task<RecordModel> AddRecord(CreateRecordRequestModel createRecordRequest)
    {
        var newRecord = _mapper.Map<VaultRecord>(createRecordRequest);
        await _vaultRecordRepository.Add(newRecord);

        return _mapper.Map<RecordModel>(newRecord);
    }

    public async Task<VaultRecordContainerModel> GetUserRecords(int userId)
    {
        VaultRecordContainerModel recordContainer = new();

        // Get all the vault records
        var vaultRecords = await this._vaultRecordRepository
            .GetList(predicate: vr => vr.OwnerId == userId);

        var mappedVaultRecords = this._mapper.Map<List<RecordModel>>(vaultRecords);
        recordContainer.VaultRecords.AddRange(mappedVaultRecords);

        // Get all the records shared with the user
        var recievedRecords = await this._sharedRecordRepository
            .GetList
            (
                predicate: sr => sr.RecipientId == userId,
                includes:
                [
                    nameof(SharedRecord.Owner),
                    nameof(SharedRecord.VaultRecord)
                ]
            );

        var mappedRecievedRecords = this._mapper.Map<List<RecordModel>>(recievedRecords);
        recordContainer.SharedRecords.AddRange(mappedRecievedRecords);

        return recordContainer;
    }

    public async Task<bool> UpdateRecord(UpdateRecordRequestModel updateRecordRequest)
    {
        var oldRecord = await this._vaultRecordRepository.Get(vr => vr.Id == updateRecordRequest.Id)
            ?? throw new ApiException(message: "Record not found in user vault.", errorCode: AppConstants.ErrorCodeEnum.NotFound);

        if (updateRecordRequest.OwnerId != oldRecord.OwnerId)
        {
            throw new ApiException(message: "User doesn't have access to this record.", errorCode: AppConstants.ErrorCodeEnum.Unauthorized);
        }

        var updatedRecord = this._mapper.Map(updateRecordRequest, oldRecord);
        var isRecordUpdated = await this._vaultRecordRepository.Update(oldRecord);

        return isRecordUpdated;
    }

    public async Task<bool> DeleteRecord(int userId, int recordId)
    {
        var record = await this._vaultRecordRepository.Get(vr => vr.OwnerId == userId && vr.Id == recordId)
            ?? throw new ApiException(message: "Record not found in user vault.", errorCode: AppConstants.ErrorCodeEnum.NotFound);

        using TransactionScope scope = new(TransactionScopeAsyncFlowOption.Enabled);

        var isRecordDeleted = await this._vaultRecordRepository.Delete(record);

        var sharedRecords = await this._sharedRecordRepository.GetList(predicate: sr => sr.RecordId == recordId);
        if (sharedRecords.Any())
        {
            await this._sharedRecordRepository.DeleteRange(sharedRecords);
        }

        scope.Complete();

        return isRecordDeleted;
    }

    public async Task<int> ShareRecord(ShareRecordRequestModel shareRecordRequest)
    {
        var record = await this._vaultRecordRepository.Get(vr => vr.Id == shareRecordRequest.RecordId)
            ?? throw new ApiException(message: "Record with the given id doesn't exist.", errorCode: AppConstants.ErrorCodeEnum.NotFound);

        var sharedRecord = await this._sharedRecordRepository
            .Get
            (
                sr => sr.RecordId == shareRecordRequest.RecordId
                && sr.RecipientId == shareRecordRequest.RecipientId
            );
            
        if (sharedRecord != null) 
            throw new ApiException(message: "Record alreay shared with this recipient.", errorCode: AppConstants.ErrorCodeEnum.Conflict);

        var newSharedRecord = this._mapper.Map<SharedRecord>(shareRecordRequest);
        var sharedRecordId = await this._sharedRecordRepository.Add(newSharedRecord);

        return sharedRecordId;
    }

    public async Task<bool> UnshareRecord(int recipientId, int sharedRecordId)
    {
        var sharedRecord = await this._sharedRecordRepository.Get(sr => sr.RecipientId == recipientId && sr.Id == sharedRecordId)
            ?? throw new ApiException(message: "Record with the given id doesn't exist.", errorCode: AppConstants.ErrorCodeEnum.NotFound);

        var recordDeleted = await this._sharedRecordRepository.Delete(sharedRecordId);
        return recordDeleted;
    }

    public async Task<List<UserModel>> GetRecordRecipients(int userId, int recordId)
    {
        var sharedRecords = await this._sharedRecordRepository
            .GetList
            (
                predicate: sr => sr.RecordId == recordId && sr.OwnerId == userId,
                includes:
                [
                    nameof(SharedRecord.Recipient),                    
                ]
            );

        var recipients = _mapper.Map<List<UserModel>>(sharedRecords.Select(sr => sr.Recipient));

        return recipients;
    }

    public async Task<bool> RevokeRecordAccess(int ownerId, int recordId, int recipientId)
    {
        var sharedRecord = await this._sharedRecordRepository
            .Get(sr => sr.RecordId == recordId && sr.OwnerId == ownerId && sr.RecipientId == recipientId)
            ?? throw new ApiException(message: "Record doesn't exist.", errorCode: AppConstants.ErrorCodeEnum.NotFound);

        var isAccessRevoked = await this._sharedRecordRepository.Delete(sharedRecord.Id);
        return isAccessRevoked;
    }
}

