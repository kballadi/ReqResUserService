using ReqRes.Core.Interfaces;
using ReqRes.Core.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Collections.Generic;

namespace ReqRes.Core.Services;

public class ExternalUserService : IExternalUserService
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;
    private readonly ILogger<ExternalUserService> _logger;

    public ExternalUserService(HttpClient httpClient, IMemoryCache cache, ILogger<ExternalUserService> logger)
    {
        _httpClient = httpClient;
        _cache = cache;
        _logger = logger;
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _cache.GetOrCreateAsync($"user_{id}", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
            try
            {
                var response = await _httpClient.GetAsync($"users/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("User {Id} not found", id);
                    return null;
                }

                //var result = await response.Content.ReadFromJsonAsync<UserResponse>();
                var result = await response.Content.ReadFromJsonAsync<UserDetailResponse>();
                return result?.Data;
                //return result?.Data.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching user by ID");
                throw;
            }
        });
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        var allUsers = new List<User>();
        int page = 1;
        while (true)
        {
            var response = await _httpClient.GetAsync($"users?page={page}");
            if (!response.IsSuccessStatusCode)
                break;

            var result = await response.Content.ReadFromJsonAsync<UserListResponse>();
            if (result?.Data?.Count == 0)
                break;

            allUsers.AddRange(result.Data);
            page++;
        }
        return allUsers;
    }
}
