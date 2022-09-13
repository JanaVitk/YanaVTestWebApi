using System;

namespace Test.Domain.Models
{
    public class QueryData : EntryData
    {
        const int MAX_PERCENT = 100;
        const int MIN_PERCENT = 0;
        public enum StatusQuery { Running, Finish, Stopping };
        public Guid ID { set; get; }
        public DateTime Star { set; get; }
        public DateTime? End { set; get; }
        public virtual ResultData Result { get; set; }
        public long MaxTimeMS { get; set; }
        public int Percent =>
            Status == StatusQuery.Finish ? MAX_PERCENT :
            Status == StatusQuery.Stopping && End != null ? getPercent(Star, End.Value) :
            getPercent(Star, DateTime.Now);

        public StatusQuery Status{set; get;}

        public QueryData() 
        {
            Status = StatusQuery.Running;
            End = null;
        }
        public QueryData(EntryData data) 
            :base()
        {
            IdUser = data.IdUser;
            From = data.From;
            To = data.To;
        }

        int getPercent(DateTime start, DateTime end) 
        {
            int res = (int)(((end - start).TotalMilliseconds / MaxTimeMS) * MAX_PERCENT);
            return res > MAX_PERCENT ? MAX_PERCENT :
                   res < MIN_PERCENT ? MIN_PERCENT : 
                   res;
        }
    }
}
