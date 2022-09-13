using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Domain.Interfaces;
using Test.Domain.Models;

namespace Test.Domain.Outputs
{
    public class UnitTestRepository : IRepository
    {
        List<QueryData> queries = new List<QueryData>();
        List<ResultData> results = new List<ResultData>();
        public async Task<Guid> GetIDQuery(Guid idUser)
        {
            var query = queries.LastOrDefault(q => q.IdUser == idUser);
            return query == null ? Guid.Empty : query.ID;
        }

        public async Task<QueryData> GetItemQuery(Guid idQuery)
        {
            return queries.FirstOrDefault(q=> q.ID == idQuery);
        }

        public async Task<QueryData> NewQuery(QueryData query)
        {
            queries.Add(query);
            return query;
        }

        public async Task<ResultData> NewResult(ResultData res)
        {
            results.Add(res);
            return res;
        }

        public async Task<QueryData> UpDateQuery(QueryData data)
        {
            return data;
        }
    }
}
