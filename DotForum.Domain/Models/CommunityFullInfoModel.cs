using DotForum.Domain.Entities;

namespace DotForum.Domain.Models;

public class CommunityFullInfoModel
{
    public Community Community { get; set; }
    public int TotalPosts { get; set; }
}