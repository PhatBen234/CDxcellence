﻿using Unilever.CDExcellent.API.Models.Entities;

public class User
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public bool IsAdmin { get; set; } = false;
    public ICollection<AreaUser> AreaUsers { get; set; } = new List<AreaUser>();
}
