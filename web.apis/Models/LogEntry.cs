using common.data.Enums;
using data.models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;

namespace web.apis.Models
{
    public class LogEntry
    {
        public LogEntry(EntityEntry entry)
        {
            Entry = entry;
        }

        public EntityEntry Entry { get; }
        public string UserId { get; set; }
        public string TableName { get; set; }
        public Dictionary<string, object> KeyValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> OldValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> NewValues { get; } = new Dictionary<string, object>();
        public LogType LogType { get; set; }
        public List<string> ChangedColumns { get; } = new List<string>();
        public Log ToAudit()
        {
            var newLog = new Log();
            newLog.UserId = UserId;
            newLog.Type = LogType.ToString();
            newLog.TableName = TableName;
            newLog.DateTime = DateTime.Now;
            newLog.PrimaryKey = JsonConvert.SerializeObject(KeyValues);
            newLog.OldValues = OldValues.Count == 0 ? null : JsonConvert.SerializeObject(OldValues);
            newLog.NewValues = NewValues.Count == 0 ? null : JsonConvert.SerializeObject(NewValues);
            newLog.AffectedColumns = ChangedColumns.Count == 0 ? null : JsonConvert.SerializeObject(ChangedColumns);
            return newLog;
        }
    }
}