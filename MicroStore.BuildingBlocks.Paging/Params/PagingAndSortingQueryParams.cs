﻿
namespace MicroStore.BuildingBlocks.Paging.Params
{
    public class PagingAndSortingQueryParams : PagingQueryParams
    {
        public string? SortBy { get; set; }

        public bool Desc { get; set; }

    }
}