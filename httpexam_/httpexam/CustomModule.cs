using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.Utilities;
using Swan.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace httpexam
{
    public class CustomModule : IWebModule
    {
        private RouteMatcher _routeMatcher;
        protected string LogSource { get; }
        public string BaseRoute { get; }
        public CustomModule(string baseRoute)
        {
            BaseRoute = Validate.Route(nameof(baseRoute), baseRoute, true);
            _routeMatcher = RouteMatcher.Parse(baseRoute, true);
            LogSource = GetType().Name;
        }

        public bool IsFinalHandler => throw new NotImplementedException();

        public ExceptionHandlerCallback OnUnhandledException { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public HttpExceptionHandlerCallback OnHttpException { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Task HandleRequestAsync(IHttpContext context)
        {
            var request = context.Request;

            switch (request.HttpVerb)
            {
                case HttpVerbs.Get:
                    return GetValue(request.Url.Query);
                case HttpVerbs.Post:
                    return CreateValue(context);
                case HttpVerbs.Put:
                    return EditValue(context);
                case HttpVerbs.Head:
                    return CheckIfValueExist(context);
                case HttpVerbs.Options:
                    return CurrencyDescriptions(context);
            }

            throw HttpException.BadRequest();
        }

        public RouteMatch MatchUrlPath(string urlPath) => _routeMatcher.Match(urlPath);

        public void Start(CancellationToken cancellationToken)
        {
            
        }

        public async Task<object> GetValue(string code)
        {
            var currencies = await JsonClient.Get<Exchangerates>("https://api.exchangeratesapi.io/latest?base=USD");
            
            return new { CurrencyCode = code, Value = currencies.rates.TryGetValue(code, out var o) };
        }

        private List<Currency> CurrencyByDates = new List<Currency>();
        public async Task<object> CreateValue(IHttpContext context)
        {
            var currency = await context.GetRequestDataAsync<Currency>();

            var currencyExist = CurrencyByDates.Any(c => c.CurrencyCode == currency.CurrencyCode && c.Date == currency.Date);

            if (currencyExist)
            {
                return new { Status = 500, Message = "Error currency already exist" };
            }

            CurrencyByDates.Add(currency);

            return new { Success = true };

        }

        public async Task<object> EditValue(IHttpContext context)
        {
            var currency = await context.GetRequestDataAsync<Currency>();

            var currencyExist = CurrencyByDates.Any(c => c.CurrencyCode == currency.CurrencyCode && c.Date == currency.Date);

            if (!currencyExist)
            {
                return new { Status = 500, Message = "Error currency doesn't exist" };
            }

            var current = CurrencyByDates.FirstOrDefault(c => c.CurrencyCode == currency.CurrencyCode && c.Date == currency.Date);
            current.Value = currency.Value;

            return new { Success = true };

        }

        public async Task<object> DeleteValue(IHttpContext context)
        {
            var currency = await context.GetRequestDataAsync<Currency>();

            var currencyExist = CurrencyByDates.Any(c => c.CurrencyCode == currency.CurrencyCode && c.Date == currency.Date);

            if (!currencyExist)
            {
                return new { Status = 500, Message = "Error currency doesn't exist" };
            }

            var current = CurrencyByDates.Remove(currency);

            return new { Success = true };

        }

        public async Task<object> CheckIfValueExist(IHttpContext context)
        {
            var code = context.Request.Url.Query;

            var currencies = await JsonClient.Get<Exchangerates>("https://api.exchangeratesapi.io/latest?base=USD");

            if (currencies.rates.ContainsKey(code))
            {
                context.Response.ContentLength64 = 1;
                return context.Response;
            }
            else
            {
                context.Response.ContentLength64 = 0;
                return context.Response;
            }
        }

        public async Task<object> CurrencyDescriptions(IHttpContext context)
        {

            var currencies = await JsonClient.Get<Exchangerates>("https://api.exchangeratesapi.io/latest?base=USD");
            var keys = currencies.rates.Keys.ToList();
            context.Response.Headers.Add("X-Currencies", string.Join(",", keys));

            return context.Response;
        }
    }    
}
