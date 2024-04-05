using AutoMapper;
using SecurePass.Data.Entities;

namespace SecurePass.Common.Models.MappingProfiles;

public class VaultRecordMappingProfile : Profile
{
    public VaultRecordMappingProfile()
    {
        CreateMap<VaultRecord, RecordModel>();
        
        CreateMap<CreateRecordRequestModel, VaultRecord>();

        CreateMap<UpdateRecordRequestModel, VaultRecord>()
            .ForMember(dest => dest.EncryptionKey, opt => opt.Ignore());

        CreateMap<SharedRecord, RecordModel>()
            .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => $"{src.Owner.FirstName} {src.Owner.LastName}"))
            .ForMember(dest => dest.IsSharedRecord, opt => opt.MapFrom(_ => true))
            .IncludeMembers(src => src.VaultRecord);

        CreateMap<ShareRecordRequestModel, SharedRecord>();
    }
}

