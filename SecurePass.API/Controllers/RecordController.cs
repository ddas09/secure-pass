using SecurePass.Services;
using SecurePass.API.Models;
using SecurePass.Common.Models;
using Microsoft.AspNetCore.Mvc;
using SecurePass.Common.Constants;
using SecurePass.Common.Exceptions;
using SecurePass.API.ActionFilters;
using SecurePass.Services.Contracts;

namespace SecurePass.API.Controllers;

/// <summary>
/// Controller responsible for managing vault records.
/// </summary>
/// <param name="recordService">The Record service for handling different record operations.</param>
[ApiController]
[Authorize]
[ValidateModelState]
[Route("api/vault/records")]
public class RecordController(IRecordService recordService) : ControllerBase
{
    private readonly CustomResponse _customResponse = new();
    private readonly IRecordService _recordService = recordService;
    private int CurrentUserId => int.Parse(JwtService.GetClaimValue(HttpContext.User, AppConstants.IdClaim));

    /// <summary>
    /// Adds a new record to the vault.
    /// </summary>
    /// <param name="recordRequest">The request containing record details.</param>
    [HttpPost]
    public async Task<IActionResult> AddRecord(CreateRecordRequestModel recordRequest)
    {
        recordRequest.OwnerId = this.CurrentUserId;
        var newRecord = await _recordService.AddRecord(recordRequest);

        var message = "Vault record added successfully.";
        return _customResponse.Success(data: newRecord, message: message);
    }

    /// <summary>
    /// Retrieves all records belonging to the authenticated user.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetRecords()
    {
        var recordContainer = await _recordService.GetUserRecords(this.CurrentUserId);
        return _customResponse.Success(data: recordContainer);
    }

    /// <summary>
    /// Updates an existing vault record.
    /// </summary>
    /// <param name="recordRequest">The request containing updated record details.</param>
    [HttpPut]
    public async Task<IActionResult> UpdateRecord(UpdateRecordRequestModel recordRequest)
    {
        recordRequest.OwnerId = this.CurrentUserId;
        var isRecordUpdated = await _recordService.UpdateRecord(recordRequest);
        if (!isRecordUpdated)
            throw new ApiException("Unable to update the record.", AppConstants.ErrorCodeEnum.BadRequest);

        var message = "Vault record updated successfully.";
        return _customResponse.Success(message: message);
    }

    /// <summary>
    /// Deletes a record from the vault.
    /// </summary>
    /// <param name="recordId">The ID of the record to delete.</param>
    [HttpDelete("{recordId}")]
    public async Task<IActionResult> DeleteRecord(int recordId)
    {
        var isRecordDeleted = await _recordService.DeleteRecord(this.CurrentUserId, recordId);
        if (!isRecordDeleted)
            throw new ApiException("Unable to delete the record.", AppConstants.ErrorCodeEnum.BadRequest);

        var message = "User record deleted successfully.";
        return _customResponse.Success(message: message);
    }

    /// <summary>
    /// Shares a vault record with another user.
    /// </summary>
    /// <param name="recordId">The ID of the record to share.</param>
    /// <param name="request">The request containing recipient details.</param>
    [HttpPost("{recordId}/share")]
    public async Task<IActionResult> ShareRecord(int recordId, ShareRecordRequestModel request)
    {
        if (recordId != request.RecordId)
            throw new ApiException("Unable to share the record.", AppConstants.ErrorCodeEnum.BadRequest);

        request.OwnerId = this.CurrentUserId;
        await _recordService.ShareRecord(request);

        var message = "Vault record shared successfully.";
        return _customResponse.Success(message: message);
    }

    /// <summary>
    /// Unshares a shared vault record.
    /// </summary>
    /// <param name="sharedRecordId">The ID of the shared record.</param>
    [HttpDelete("unshare/{sharedRecordId}")]
    public async Task<IActionResult> UnshareRecord(int sharedRecordId)
    {
        var isRecordDeleted = await _recordService.UnshareRecord(this.CurrentUserId, sharedRecordId);
        if (!isRecordDeleted)
            throw new ApiException("Unable to delete the record.", AppConstants.ErrorCodeEnum.BadRequest);

        var message = "Record deleted successfully.";
        return _customResponse.Success(message: message);
    }

    /// <summary>
    /// Retrieves recipients of a shared record.
    /// </summary>
    /// <param name="recordId">The ID of the record.</param>
    [HttpGet("{recordId}/recipients")]
    public async Task<IActionResult> GetRecordRecipients(int recordId)
    {
        var recipients = await _recordService.GetRecordRecipients(this.CurrentUserId, recordId);
        return _customResponse.Success(data: recipients);
    }

    /// <summary>
    /// Revokes access to a shared record.
    /// </summary>
    /// <param name="recordId">The ID of the record.</param>
    /// <param name="recipientId">The ID of the recipient whose access needs to be revoked.</param>
    [HttpDelete("{recordId}/recipients/{recipientId}/revoke")]
    public async Task<IActionResult> RevokeRecordAccess(int recordId, int recipientId)
    {
        var isAccessRevoked = await _recordService.RevokeRecordAccess(this.CurrentUserId, recordId, recipientId);
        if (!isAccessRevoked)
            throw new ApiException("Unable to unshare the record.", AppConstants.ErrorCodeEnum.BadRequest);

        var message = "Record unshared successfully.";
        return _customResponse.Success(message: message);
    }
}
