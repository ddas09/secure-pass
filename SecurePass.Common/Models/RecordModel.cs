namespace SecurePass.Common.Models;

public class RecordModel
{
    public int Id { get; set; }

    public required string Title { get; set; }

    public required string Login { get; set; }

    public required string Password { get; set; }

    public string? WebsiteUrl { get; set; }

    public string? Notes { get; set; }

    public required string EncryptionKey { get; set; }

    public required string OwnerName { get; set; }

    public bool IsSharedRecord { get; set; } = false;
}

public class VaultRecordContainerModel 
{
    public List<RecordModel> VaultRecords { get; set; } = new List<RecordModel>();
    
    public List<RecordModel> SharedRecords { get; set; } = new List<RecordModel>();
}
