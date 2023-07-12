using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace DotForum.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum VoteStatusEnum
{
    Downvote = -1,
    Novote = 0,
    Upvote = 1
}