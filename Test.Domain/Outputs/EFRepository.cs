using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Domain.Interfaces;
using Test.Domain.Models;

namespace Test.Domain.Outputs
{
    public class EFRepository : IRepository
    {
        EFDbContext context = new EFDbContext();

        public async Task<QueryData> GetItemQuery(Guid idQuery)
        {
            var query = await context.Queries.FindAsync(idQuery);
            if (query == null)
            {
                return null;
            }
            await context.Entry(query).Reference(q => q.Result).LoadAsync();

            return query;
        }

        public async Task<QueryData> NewQuery(QueryData query)
        {
            try
            {
                await context.Queries.AddAsync(query);
                await context.SaveChangesAsync();
                return query;
            }
            catch (Exception ex)
            {
                throw new Exception($"Query with UserId{query.IdUser} was not created in DataBase;\n {ex.Message}");
            }
        }

        public async Task<QueryData> UpDateQuery(QueryData data)
        {
            try
            {
                await context.SaveChangesAsync();
                return data;
            }
            catch
            {
                throw new Exception($"Query {data.ID} was not updated  in DataBase");
            }
        }

        public async Task<Guid> GetIDQuery(Guid idUser)
        {
            var query = await context.Queries
                .Where(q => q.IdUser == idUser)
                .OrderByDescending(q => q.Star)
                .FirstOrDefaultAsync<QueryData>();
            return query == null ? Guid.Empty : query.ID;
        }

        public async Task<ResultData> NewResult(ResultData res)
        {
            try
            {
                await context.Results.AddAsync(res);
                await context.SaveChangesAsync();
                return await context.Results.FindAsync(res.ID);
            }
            catch (Exception ex)
            {
                throw new Exception($"Result for  Query {res.QueryID} was not created in DataBase;\n {ex.Message}");
            }
        }
    }
}
