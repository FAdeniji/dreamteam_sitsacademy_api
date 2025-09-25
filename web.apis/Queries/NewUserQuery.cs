using MediatR;
using web.apis.Models;

namespace web.apis.Queries
{
    public class NewUserQuery : IRequest<UserDetails>
	{
		public UserDetails _model { get; set; }
        public bool IsInvestor { get; set; }
        public string LoggedInUserId { get; set; }
        public NewUserQuery(UserDetails model, bool isInvestor, string loggedInUserId)
        {
            _model = model;
            IsInvestor = isInvestor;
            LoggedInUserId = loggedInUserId;
        }
    }
}

