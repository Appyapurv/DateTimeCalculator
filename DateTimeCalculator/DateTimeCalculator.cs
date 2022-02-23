using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DateTimeCalculator
{
    public static class DateTimeCalculator
    {
        [FunctionName("DateTimeCalculator")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            try
            {
                var startTime = DateTime.UtcNow;
                log.LogInformation($"Request has been Started: {startTime}");
                string output = string.Empty;

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<DateTimeModel>(requestBody);
                var result = data?.dateandtime - startTime;

                var resultModel = new TimeFormat();
                if (result != null)
                {
                    resultModel.Days = result?.Days;
                    resultModel.Hours = result?.Hours;
                    resultModel.Minutes = result?.Minutes;
                    resultModel.Seconds = result?.Seconds;

                    output = $"Days Left - {resultModel.Days} , Hours Left - {resultModel.Hours} , Minutes Left - {resultModel.Minutes} , Second Left - {resultModel.Seconds}";
                }

                if (!string.IsNullOrWhiteSpace(output))
                { return new OkObjectResult(output); }
                return new OkObjectResult("You have not set nay date and Time");

            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }
        }
    }
}
