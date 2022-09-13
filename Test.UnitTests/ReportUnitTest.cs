using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Test.Domain.Models;
using Test.Domain.Outputs;
using Test.WebApi.Settings;
using YanaVTestWebApi.Controllers;

namespace Test.UnitTests
{
    class ReportUnitTest
    {
        UnitTestRepository repository = new UnitTestRepository();

        private AppSettings settings;

        [SetUp]
        public void Setup()
        {
            settings = Configuration.GetSettings();
            if (settings == null)
                settings = new AppSettings();
        }
        [Test]
        public async Task TestTimeExecution()
        {
            ReportController controller = new ReportController(repository);
            var tasks = new List<Task>();
            Guid IdUser = Guid.NewGuid();

            tasks.Add(controller.PostUser_statistics(
                new EntryData
                {
                    IdUser = IdUser,
                    From = DateTime.Now.AddDays(-7),
                    To = DateTime.Now
                }));
            tasks.Add(Task.Run(async () =>
            {
                await Task.Delay(settings.MaxExecution);
                Guid idQuery = await repository.GetIDQuery(IdUser);
                QueryData q = await repository.GetItemQuery(idQuery);
                Assert.IsTrue(q.Status == QueryData.StatusQuery.Running, "Status is not Running");
            }));

            await Task.WhenAll(tasks);
        }


        [Test]
        public async Task TestPercents50()
        {
            ReportController controller = new ReportController(repository);
            var tasks = new List<Task>();
            Guid IdUser = Guid.NewGuid();

            tasks.Add(controller.PostUser_statistics(
                new EntryData
                {
                    IdUser = IdUser,
                    From = DateTime.Now.AddDays(-7),
                    To = DateTime.Now
                }));
            tasks.Add(Task.Run(async () =>
            {
                await Task.Delay(settings.MaxExecution/2);
                Guid idQuery = await repository.GetIDQuery(IdUser);
                QueryData q = await repository.GetItemQuery(idQuery);
                Assert.IsTrue(q.Percent >= 50, $"Percent {q.Percent} must be >= 50");

                var res = System.Text.Json.JsonSerializer.Deserialize(
                    (await controller.GetInfo(idQuery)).Value.ToString(),
                     new { percent = 0 }.GetType());
                var val = (int)res.GetType().GetProperty("percent").GetValue(res);
                Assert.IsTrue(val >= 50, $"Percent {q.Percent} must be >= 50");
            }));

            await Task.WhenAll(tasks);
        }

       
    }
}
