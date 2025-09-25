using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using common.data;

namespace data.models
{
    public class Log : Entity
    {
        public Log()
        {
            IsActive = true;
            DateAdded = DateTime.UtcNow;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string UserId { get; set; }

        public string Type { get; set; }

        public string TableName { get; set; }

        public DateTime DateTime { get; set; }

        public string? OldValues { get; set; }

        public string? NewValues { get; set; }

        public string? AffectedColumns { get; set; }

        public string? PrimaryKey { get; set; }

        public string? StaffName { get; set; }
    }
}
