namespace RiskAssessment.Models.Shared
{
    public class PaginationModel
    {
        public int Page { get; internal set; } = 1;
        public int PageSize { get; internal set; } = 10;
        public int TotalItem { get; internal set; }
        public int TotalPage { get; internal set; }
        public int CurrentItem { get; internal set; }
        public bool IsPrevious => Page > 1;
        public bool IsNext => Page < TotalPage;

        public int PageFrom => PageSize * (Page - 1) + 1;
        public int PageTo => PageSize * (Page - 1) + CurrentItem;
        //public bool HasPrevious => Page > 1;
        //public bool HasNext => Page < TotalPage;
    }
}
