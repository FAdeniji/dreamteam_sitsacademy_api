namespace web.apis.ViewModels
{
    public class UserTypeViewModel
	{
		public UserTypeViewModel(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}

