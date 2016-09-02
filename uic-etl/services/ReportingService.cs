using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using uic_etl.models.dtos;

namespace uic_etl.services
{
    public static class ReportingService
    {
        private const string ApplicationName = "etl";
        private const string SpreadsheetId = "1jeYvLWq7XFmDgKayO7ZFyNuqzkRnDS9Wz0ozwquTErA";
        private static readonly string[] Scopes = {SheetsService.Scope.Spreadsheets};
        private static SheetsService _service;
        private static string _worksheet;

        public static void Initalize()
        {
            GoogleCredential credential;
            using (var stream = new FileStream("secrets.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
            }

            _service = new SheetsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName
            });

            var newSheetRequest = new BatchUpdateSpreadsheetRequest
            {
                Requests = new List<Request>
                {
                    new Request
                    {
                        AddSheet = new AddSheetRequest
                        {
                            Properties = new SheetProperties
                            {
                                Title = DateTime.Now.ToString("yyyyMMdd-hhmmsstt")
                            }
                        }
                    }
                }
            };

            var response = _service.Spreadsheets.BatchUpdate(newSheetRequest, SpreadsheetId).Execute();
            _worksheet = response.Replies.Single().AddSheet.Properties.Title;
        }

        public static void Log(string message)
        {
            var vRange = new ValueRange
            {
                Range = string.Format("{0}!A1:A1", _worksheet),
                Values = new List<IList<object>>
                {
                    new List<object> {message}
                },
                MajorDimension = "ROWS"
            };

            Append(vRange);
        }

        public static void LogErrors(IList newItems)
        {
            foreach (var item in newItems)
            {
                var logModel = item as LogModel;
                if (logModel == null)
                {
                    continue;
                }

                var id = logModel.Identifier;
                var list1 = new List<object> {id};
                IList<IList<object>> list = new List<IList<object>> {list1};

                foreach (var failure in logModel.Failures)
                {
                    var condition = failure.Key;
                    list.Add(new List<object> {" ", condition});

                    foreach (var reason in failure.Value)
                    {
                        var cause = string.Format("{0} {1} is not valid.", reason.ErrorMessage, reason.AttemptedValue);
                        list.Add(new List<object> {" ", " ", cause});
                    }
                }

                var vRange = new ValueRange
                {
                    Range = string.Format("{0}!A1:C{1}", _worksheet, list.Count),
                    Values = list,
                    MajorDimension = "ROWS"
                };

                Append(vRange);
            }
        }

        private static void Append(ValueRange vRange)
        {
            var request = _service.Spreadsheets.Values.Append(vRange, SpreadsheetId, vRange.Range);
            request.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;

            var response = request.Execute(); 
        }
    }
}