using web.apis.Models;

namespace web.apis
{
    public interface IDashboardRepository
	{
		IEnumerable<ApplicationUser> TotalUsers(int? learningInstitutionId);
        IEnumerable<CategoriesAndNoOfIdeas> CategoriesAndNoOfIdeas();
    }
}

