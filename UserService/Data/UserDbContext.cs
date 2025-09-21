﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserService.Entities;

namespace UserService.Data;

public class UserDbContext : IdentityDbContext<ApplicationUser>
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }
}