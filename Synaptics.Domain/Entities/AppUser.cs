﻿using Microsoft.AspNetCore.Identity;
using Synaptics.Domain.Enums;

namespace Synaptics.Domain.Entities;

public class AppUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? ProfilePhotoPath { get; set; }
    public string? CoverPhotoPath { get; set; }
    public Gender Gender { get; set; }
    public string Biography { get; set; }
    public string SelfDescription { get; set; }
    public ICollection<UserRelation> Followers { get; set; }
    public ICollection<UserRelation> Following { get; set; }
    public ICollection<Post> Posts { get; set; }
    public ICollection<PostLike> PostsLike { get; set; }
    public ICollection<PostComment> PostComments { get; set; }
    public ICollection<CommentLike> CommentsLike { get; set; }
}
