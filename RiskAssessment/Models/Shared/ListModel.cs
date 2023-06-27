using System.Collections.Generic;
using System;
using System.Linq;

namespace RiskAssessment.Models.Shared
{
    public abstract class ListModel<T>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalItem { get; internal set; }
        public int TotalPage { get; internal set; }
        public int CurrentItem { get; internal set; }
        public bool IsPrevious => Page > 1;
        public bool IsNext => Page < TotalPage;

        public int PageFrom => PageSize * (Page - 1) + 1;
        public int PageTo => PageSize * (Page - 1) + CurrentItem;

        public List<T> ToListModel(List<T> items)
        {
            TotalItem = items.Count;
            TotalPage = (int)Math.Ceiling(items.Count / (double)PageSize);

            var filteredItems = items.Skip(PageSize * (Page - 1)).Take(PageSize).ToList();
            CurrentItem = filteredItems.Count;
            return filteredItems;
        }

        public PaginationModel ToPaginationModel()
        {
            return new PaginationModel
            {
                Page = Page,
                PageSize = PageSize,
                TotalItem = TotalItem,
                TotalPage = TotalPage,
                CurrentItem = CurrentItem
            };
        }
    }
}
