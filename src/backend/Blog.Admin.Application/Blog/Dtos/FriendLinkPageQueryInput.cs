﻿namespace Blog.Admin.Application.Blog.Dtos;

public class FriendLinkPageQueryInput : Pagination
{
    /// <summary>
    /// 站点名称
    /// </summary>
    public string SiteName { get; set; }
}