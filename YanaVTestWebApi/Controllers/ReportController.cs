using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using Test.Domain.Interfaces;
using Test.Domain.Models;
using Test.WebApi.Settings;

namespace YanaVTestWebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ReportController : Controller
    {
        private IRepository repository;
        private AppSettings settings;

        public ReportController(IRepository repo)
        {
            repository = repo;
            settings = Configuration.GetSettings();
            if (settings == null)
                settings = new AppSettings();

        }

        [HttpPost("user_statistics")]
        public async Task<ActionResult<Guid>> PostUser_statistics(EntryData data)
        {
            if (data.IdUser == Guid.Empty || data.From > data.To)
            {
                return Guid.Empty;
            }

            Guid IdQuery = Guid.NewGuid();
            try
            {
                await repository.NewQuery(new QueryData(data)
                {
                    ID = IdQuery,
                    Star = DateTime.Now,
                    MaxTimeMS = settings.MaxExecution
                });

                int CountPoint = settings.MaxExecution / settings.Step + 1;
                for (int i = 0; i < CountPoint; i++)
                {
                    await Task.Delay(settings.Step);
                    QueryData q = await repository.GetItemQuery(IdQuery);
                    q.End = DateTime.Now;
                    q.Status = QueryData.StatusQuery.Running;
                    await repository.UpDateQuery(q);
                }

                QueryData query = await repository.GetItemQuery(IdQuery);
                query.Result = await repository.NewResult(new ResultData
                {
                    ID = Guid.NewGuid(),
                    QueryID = query.ID,
                    CountSignIn = (new Random()).Next(),
                    IsOk = true
                });
                query.End = DateTime.Now;
                query.Status = QueryData.StatusQuery.Finish;
                await repository.UpDateQuery(query);
                return IdQuery;
            }
            catch
            {
                return Guid.Empty;
            }

        }

        [HttpGet("info/{idQuery:Guid}")]
        public async Task<ActionResult<string>> GetInfo(Guid idQuery)
        {
            try
            {
                QueryData query = await repository.GetItemQuery(idQuery);
                if (query.Status == QueryData.StatusQuery.Running &&
                    query.Star.AddMilliseconds(query.MaxTimeMS) < DateTime.Now)
                {
                    query.Result = await repository.NewResult(new ResultData
                    {
                        ID = Guid.NewGuid(),
                        QueryID = query.ID,
                        CountSignIn = 0,
                        IsOk = false,
                        ErrorStr = "the Process was stopped"
                    });
                    if (query.End == null)
                        query.End = query.Star;
                    query.Status = QueryData.StatusQuery.Stopping;
                    await repository.UpDateQuery(query);
                }
                var options = new JsonSerializerOptions { WriteIndented = true };

                object res = query.Result == null ? "null" :
                        query.Result.IsOk ? new
                        {
                            user_id = query.IdUser,
                            count_sign_in = query.Result.CountSignIn
                        } : new { Error = query.Result.ErrorStr.ToString() };

                return JsonSerializer.Serialize(new
                {
                    query = query.ID,
                    percent = query.Percent,
                    result = res
                }, options);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpGet("get_id_query")]
        public async Task<ActionResult<Guid>> GetIDQuery(Guid idUser)
        {
            return await repository.GetIDQuery(idUser);
        }

    }
}
