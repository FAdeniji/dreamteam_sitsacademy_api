namespace web.apis.Models
{
    public class UserDetails
	{
        public string UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public string MobileNumber { get; set; }

        public int SubscriptionId { get; set; }
        
        public int LearningInstitutionId { get; set; }

        public int CourseId { get; set; }
    }
}

