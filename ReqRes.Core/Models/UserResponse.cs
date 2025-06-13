using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ReqRes.Core.Models;

public class UserListResponse
{
    [JsonPropertyName("data")]
    public List<User> Data { get; set; } = [];
}

public class UserDetailResponse
{
    [JsonPropertyName("data")]
    public User Data { get; set; } = new();
}