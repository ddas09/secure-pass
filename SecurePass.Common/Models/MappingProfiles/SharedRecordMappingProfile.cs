using AutoMapper;
using SecurePass.Data.Entities;

namespace SecurePass.Common.Models.MappingProfiles;

public class SharedRecordMappingProfile : Profile
{
    public SharedRecordMappingProfile()
    {
        CreateMap<SharedRecord, SharedRecordModel>();

        CreateMap<ShareRecordRequestModel, SharedRecord>();
    }
}

