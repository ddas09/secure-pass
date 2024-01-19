namespace SecurePass.Common.Models;

public class RecordModel
{
    public List<VaultRecordModel> VaultRecords { get; set; } = new List<VaultRecordModel>();

    public List<SharedRecordModel> SharedRecords { get; set; } = new List<SharedRecordModel>();
}

public class VaultRecordModel
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Login { get; set; }

    public string Password { get; set; }

    public string WebsiteUrl { get; set; }

    public string Note { get; set; }

    public string RecordKey { get; set; }

    public string OwnerName { get; set; }

    public bool IsRecordShared { get; set; } = false;
}

public class SharedRecordModel
{
    public int Id { get; set; }

    public string EncryptionKey { get; set; }

    public VaultRecordModel VaultRecord { get; set; }
}


