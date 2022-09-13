using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Test.Domain.Models;

namespace Test.Domain.Interfaces
{
    public interface IRepository
    {
        Task<QueryData> NewQuery(QueryData query);
        Task<ResultData> NewResult(ResultData res);
        Task<QueryData> UpDateQuery(QueryData data);
        Task<QueryData> GetItemQuery(Guid idQuery);
        Task<Guid> GetIDQuery(Guid idUser);
    }
}
