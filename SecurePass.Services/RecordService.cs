using AutoMapper;
using System.Transactions;
using SecurePass.Common.Models;
using SecurePass.DAL.Contracts;
using SecurePass.Data.Entities;
using SecurePass.Common.Constants;
using SecurePass.Common.Exceptions;
using SecurePass.Services.Contracts;

namespace SecurePass.Services;

public class RecordService : IRecordService
{
    private readonly IMapper _mapper;  
    private readonly IVaultRecordRepository _vaultRecordRepository;
    private readonly ISharedRecordRepository _sharedRecordRepository;

    public RecordService(
        IMapper mapper,
        IVaultRecordRepository vaultRecordRepository,
        ISharedRecordRepository sharedRecordRepository
    )
    {
        _mapper = mapper;
        _vaultRecordRepository = vaultRecordRepository;
        _sharedRecordRepository = sharedRecordRepository;
    }

    public async Task<bool> UnshareRecord(int sharedRecordId)
    {
        var sharedRecord = await this._sharedRecordRepository.Get(sr => sr.Id == sharedRecordId)
            ?? throw new ApiException(message: "Record with the given id doesn't exist.", errorCode: AppConstants.ErrorCodeEnum.NotFound);

        var recordDeleted = await this._sharedRecordRepository.Delete(sharedRecordId);
        return recordDeleted;
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
            ) ?? throw new ApiException(message: "Record alreay shared with this recipient.", errorCode: AppConstants.ErrorCodeEnum.Conflict);

        var newSharedRecord = this._mapper.Map<SharedRecord>(shareRecordRequest);
        var sharedRecordId = await this._sharedRecordRepository.Add(newSharedRecord);

        return sharedRecordId;
    }

    public async Task<bool> DeleteRecord(int recordId)
    {
        var record = await this._vaultRecordRepository.Get(vr => vr.Id == recordId)
            ?? throw new ApiException(message: "Record with the given id doesn't exist.", errorCode: AppConstants.ErrorCodeEnum.NotFound);

        using TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        var isRecordDeleted = await this._vaultRecordRepository.Delete(record);

        var sharedRecords = await this._sharedRecordRepository.GetList(predicate: sr => sr.RecordId == recordId);
        if (sharedRecords.Any())
        {
            await this._sharedRecordRepository.DeleteRange(sharedRecords);
        }
        
        scope.Complete();

        return isRecordDeleted;
    }

    public async Task<bool> UpdateRecord(UpdateRecordRequestModel updateRecordRequest)
    {
        var oldRecord = await this._vaultRecordRepository.Get(vr => vr.Id == updateRecordRequest.RecordId) 
            ?? throw new ApiException(message: "Record with the given id doesn't exist", errorCode: AppConstants.ErrorCodeEnum.NotFound);

        if (updateRecordRequest.OwnerId !=  oldRecord.OwnerId)
        {
            throw new ApiException(message: "User doesn't have access to this record.", errorCode: AppConstants.ErrorCodeEnum.Unauthorized);
        }

        var updatedRecord = this._mapper.Map<VaultRecord>(updateRecordRequest);
        var isRecordUpdated = await this._vaultRecordRepository.Update(updatedRecord);

        return isRecordUpdated;
    }

    public async Task<RecordModel> AddRecord(CreateRecordRequestModel createRecordRequest)
    {
        var newRecord = _mapper.Map<VaultRecord>(createRecordRequest);
        await _vaultRecordRepository.Add(newRecord);

        return _mapper.Map<RecordModel>(newRecord);
    }

    public async Task<RecordModel> GetUserRecords(int userId)
    {
        var userRecords = new RecordModel();

        // Get all the vault records
        var vaultRecords = await this._vaultRecordRepository
            .GetList
            (
                predicate: vr => vr.OwnerId == userId, 
                include: $"{nameof(VaultRecord.Owner)}, {nameof(VaultRecord.SharedRecords)}"
            );
        
        var mappedVaultRecords = this._mapper.Map<List<VaultRecordModel>>(vaultRecords);
        Parallel.ForEach(mappedVaultRecords, record =>
        {
            var vaultRecord = vaultRecords.First(vr => vr.Id == record.Id);
            record.IsRecordShared = vaultRecord.SharedRecords.Any();
            record.OwnerName = $"{vaultRecord.Owner.FirstName} {vaultRecord.Owner.LastName}";
        });

        userRecords.VaultRecords.AddRange(mappedVaultRecords);

        // Get all the records shared with the user
        var recievedRecords = await this._sharedRecordRepository
            .GetList
            (
                predicate: sr => sr.RecipientId == userId,
                include: $"{nameof(SharedRecord.Owner)}, {nameof(SharedRecord.VaultRecord)}"
            );

        var mappedRecievedRecords = this._mapper.Map<List<SharedRecordModel>>(recievedRecords);
        Parallel.ForEach(mappedRecievedRecords, record =>
        {
            var recieveRecord = recievedRecords.First(sr => sr.Id == record.Id);
            record.VaultRecord.OwnerName = $"{recieveRecord.Owner.FirstName} {recieveRecord.Owner.LastName}";
        });

        userRecords.SharedRecords.AddRange(mappedRecievedRecords);

        return userRecords;
    }
}

