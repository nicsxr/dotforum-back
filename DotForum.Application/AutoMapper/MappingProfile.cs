using AutoMapper;
using DotForum.Application.Models.Responses.Comment;
using DotForum.Application.Models.Responses.Community;
using DotForum.Application.Models.Responses.Post;
using DotForum.Domain.Entities;
using DotForum.Domain.Enums;
using DotForum.Domain.Models;

namespace DotForum.Application.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CommunityFullInfoModel, GetCommunityResponse>()
            .ForMember(dest => dest.Name, opt =>
                opt.MapFrom(src => src.Community.Name))
            .ForMember(dest => dest.Description, opt =>
                opt.MapFrom(src => src.Community.Description))
            .ForMember(dest => dest.OwnerId, opt =>
                opt.MapFrom(src => src.Community.UserId))
            .ForMember(dest => dest.Id, opt =>
                opt.MapFrom(src => src.Community.Id));


        CreateMap<Comment, CommentModel>();
        CreateMap<PostModel, GetPostResponse>();
        CreateMap<PostModel, VotePostResponse>();
        CreateMap<GetCommentsModel, GetCommentsByPostIdResponse>();
    }
}