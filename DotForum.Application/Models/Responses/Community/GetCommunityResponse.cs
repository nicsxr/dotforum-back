namespace DotForum.Application.Models.Responses.Community;

public class GetCommunityResponse
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string OwnerId { get; set; }
    public int TotalPosts { get; set; }
}