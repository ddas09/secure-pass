using AutoMapper;
using SecurePass.Data.Entities;

namespace SecurePass.Common.Models.MappingProfiles;

public class VaultRecordMappingProfile : Profile
{
    public VaultRecordMappingProfile()
    {
        CreateMap<VaultRecord, VaultRecordModel>();

        CreateMap<CreateRecordRequestModel, VaultRecord>();

        CreateMap<UpdateRecordRequestModel, VaultRecord>();
    }
}

