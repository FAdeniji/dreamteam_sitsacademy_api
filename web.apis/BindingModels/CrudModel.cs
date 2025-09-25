using System.ComponentModel.DataAnnotations;

namespace web.apis
{
    public class CrudModel
	{
		[Required]
		public int Id { get; set; }
	}

    public class CrudModelString
    {
        [Required]
        public string Id { get; set; }
    }

    public class CrudModelRef
    {
        [Required]
        public string Reference { get; set; }
    }

    public class CrudDateModel
    {
        [Required]
        public DateTime RefDate { get; set; }
    }

    public class PullCrudModel
    {
        [Required]
        public int InviteId { get; set; }

        [Required]
        public string Expiry { get; set; }

        [Required]
        public string Email { get; set; }
    }

    public class CrudDoubleIntModel
    {
        [Required]
        public int GroupID { get; set; }

        [Required]
        public int LearningInstitutionID { get; set; }

        [Required]
        public int CourseID { get; set; }
    }

    public class CrudIntIntModel
    {
        [Required]
        public int Value1 { get; set; }

        [Required]
        public int Value2 { get; set; }
    }

    public class CrudIntStringModel
    {
        [Required]
        public int Value1 { get; set; }

        [Required]
        public string Value2 { get; set; }
    }

    public class CrudStringListModel
    {
        [Required]
        public List<string> EmailAddresses { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        public int LearningInstitutionId { get; set; }
    }
}

