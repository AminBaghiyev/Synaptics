using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Synaptics.Application.DTOs;
using Synaptics.Application.Exceptions.Base;
using Synaptics.Application.Interfaces;
using Synaptics.Application.Interfaces.Repositories;
using Synaptics.Domain.Entities;

namespace Synaptics.Persistence.Services;

public class PostService : IPostService
{
    readonly UserManager<AppUser> _userManager;
    readonly IPostRepository _repository;
    readonly IMapper _mapper;

    public PostService(IPostRepository repository, IMapper mapper, UserManager<AppUser> userManager)
    {
        _repository = repository;
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<ICollection<PostItemDTO>> GetAllAsync(string username, int count = 10, int page = 0)
    {
        AppUser user = await _userManager.FindByNameAsync(username) ?? throw new ExternalException("User not found!");

        return _mapper.Map<ICollection<PostItemDTO>>(await _repository.GetAllAsync(e => e.UserId == user.Id && !e.IsDeleted, page, count, false, "UpdatedAt"));
    }

    public async Task<ICollection<PostItemOfCurrentUserDTO>> GetAllOfCurrentUserAsync(string username, int count = 10, int page = 0)
    {
        AppUser user = await _userManager.FindByNameAsync(username) ?? throw new ExternalException("User not found!");

        return _mapper.Map<ICollection<PostItemOfCurrentUserDTO>>(await _repository.GetAllAsync(e => e.UserId == user.Id && !e.IsDeleted, page, count, false, "UpdatedAt"));
    }

    public async Task<UpdatePostDTO> GetByIdOfCurrentUserForUpdateAsync(long id, string username)
    {
        AppUser user = await _userManager.FindByNameAsync(username) ?? throw new ExternalException("User not found!");
        
        return _mapper.Map<UpdatePostDTO>(await _repository.GetOneAsync(e => e.Id == id && !e.IsDeleted));
    }

    public async Task<PostItemDTO> GetByIdAsync(long id, string username)
    {
        AppUser user = await _userManager.FindByNameAsync(username) ?? throw new ExternalException("User not found!");

        return _mapper.Map<PostItemDTO>(await _repository.GetOneAsync(e => e.Id == id && !e.IsDeleted) ?? throw new ExternalException("Post not found!"));
    }

    public async Task<PostItemOfCurrentUserDTO> GetByIdOfCurrentUserAsync(long id, string username)
    {
        AppUser user = await _userManager.FindByNameAsync(username) ?? throw new ExternalException("User not found!");

        return _mapper.Map<PostItemOfCurrentUserDTO>(await _repository.GetOneAsync(e => e.Id == id) ?? throw new ExternalException("Post not found!"));
    }

    public async Task CreateAsync(CreatePostDTO dto, string username)
    {
        AppUser user = await _userManager.FindByNameAsync(username) ?? throw new ExternalException("User not found!");
        Post post = _mapper.Map<Post>(dto);
        post.User = user;

        await _repository.CreateAsync(post);
    }

    public async Task UpdateAsync(UpdatePostDTO dto, string username)
    {
        AppUser user = await _userManager.FindByNameAsync(username) ?? throw new ExternalException("User not found!");
        Post post = await _repository.GetOneAsync(e => e.Id == dto.Id && !e.IsDeleted) ?? throw new ExternalException("Post not found!");
        Post editedPost = _mapper.Map(dto, post);

        _repository.Update(editedPost);
    }    

    public async Task RecoverAsync(long id, string username)
    {
        AppUser user = await _userManager.FindByNameAsync(username) ?? throw new ExternalException("User not found!");
        Post post = await _repository.GetOneAsync(e => e.Id == id && e.IsDeleted) ?? throw new ExternalException("Post not found!");
        post.IsDeleted = false;

        _repository.Update(post);
    }

    public async Task SoftDeleteAsync(long id, string username)
    {
        AppUser user = await _userManager.FindByNameAsync(username) ?? throw new ExternalException("User not found!");
        Post post = await _repository.GetOneAsync(e => e.Id == id && !e.IsDeleted) ?? throw new ExternalException("Post not found!");

        _repository.SoftDelete(post);
    }

    public async Task HardDeleteAsync(long id, string username)
    {
        AppUser user = await _userManager.FindByNameAsync(username) ?? throw new ExternalException("User not found!");
        Post post = await _repository.GetOneAsync(e => e.Id == id && e.IsDeleted) ?? throw new ExternalException("Post not found!");

        _repository.HardDelete(post);
    }

    public async Task<int> SaveChangesAsync() => await _repository.SaveChangesAsync();
}
