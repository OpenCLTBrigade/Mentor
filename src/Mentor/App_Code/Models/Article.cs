using System;

/// <summary>
/// Stores content page data for website
/// </summary>
public class Article
{
    public int Id { get; private set; }
    public string Title { get; set; }
    public string Summary { get; set; }
    public string Body { get; set; }
    public DateTime Modified { get; set; }
};
