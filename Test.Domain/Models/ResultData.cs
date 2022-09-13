using System;

namespace Test.Domain.Models
{
    public class ResultData
    {
        public Guid ID { set; get; }
        public Guid QueryID { set; get; }
        public int CountSignIn { set; get; }
        public bool IsOk { set; get; }
        public string ErrorStr { set; get; }
        public ResultData() 
        {
            IsOk = true;
        }
    }
}
