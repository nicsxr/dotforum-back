using AutoMapper;
using DotForum.Application.Helpers;
using DotForum.Application.Models.Common;
using DotForum.Application.Models.Requests;
using DotForum.Application.Models.Requests.User;
using DotForum.Application.Models.Responses.User;
using DotForum.Application.Services.Abstractions;
using DotForum.Domain.Entities;
using DotForum.Persistence.Repositories.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace DotForum.Application.Services.Implementations;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public UserService(UserManager<ApplicationUser> userManager, IUserRepository userRepository, 
        IHttpContextAccessor httpContextAccessor, IMapper mapper)
    {
        _userManager = userManager;
        _userRepository = userRepository;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<AppResponse<EmptyResponse>> RegisterUser(RegisterUserRequest model)
    {
        var user = new ApplicationUser
        {
            UserName = model.Username,
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        
        return result.Succeeded ? ResponseHelper.Ok() : ResponseHelper.Fail();
    }
    
    public async Task<AppResponse<GetUserResponse>> GetUser()
    {
        var userId = _httpContextAccessor.HttpContext!.User.GetUserId();

        var result = await _userRepository.GetByIdAsync(userId);
        if (result != null)
        {
            return ResponseHelper.Ok(new GetUserResponse
            {
                Username = result.UserName!,
                Id = result.Id
            });
        }
        return ResponseHelper.Fail<GetUserResponse>();
    }
}