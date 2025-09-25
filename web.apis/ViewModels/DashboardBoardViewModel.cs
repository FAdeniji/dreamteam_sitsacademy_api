namespace web.apis
{
    public class DashboardBoardViewModel
	{
		public int TotalUsers { get; set; }
        public int TotalActiveUsers { get; set; }
        public int TotalInactiveUsers { get; set; }
        public int TotalIdeas { get; set; }
        public int TotalInvestors { get; set; }
        public int TotalCategories { get; set; }
        public List<CategoriesAndNoOfIdeas> CategoryAndNoOfIdeas { get; set; }
    }

    public class CategoriesAndNoOfIdeas
    {
        public string CategoryName { get; set; }

        public string Colour { get; set; }

        public int NoOfIdeas { get; set; }
    }
}

