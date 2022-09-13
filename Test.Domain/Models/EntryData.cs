using System;

namespace Test.Domain.Models
{
    public class EntryData
    {
        public Guid IdUser { set; get; }
        public DateTime From { set; get; }
        public DateTime To { set; get; }
    }
}
