﻿namespace JobsityChallenge.Client.Models
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = new();
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }

}
