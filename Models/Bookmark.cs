﻿using WEBDEV.Models;

public class Bookmark
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    public int BookId { get; set; }
    public Book Book { get; set; }
}
