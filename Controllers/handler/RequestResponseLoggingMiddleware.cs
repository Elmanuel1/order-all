using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.IO;
using Newtonsoft.Json;
using Serilog;
using SwaggerApp.Data;
using SwaggerApp.vo;

namespace SwaggerApp.Controllers.handler
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;
        private readonly Action<RequestProfilerModel> _requestResponseHandler;
        private const int ReadChunkBufferLength = 4096;
        private static readonly Encoding LocalEncoding = Encoding.UTF8;
        private ILogger<RequestResponseLoggingMiddleware> _logger;

        public RequestResponseLoggingMiddleware(RequestDelegate next, Action<RequestProfilerModel> requestResponseHandler, ILogger<RequestResponseLoggingMiddleware> logger )
        {
            _next = next;
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
            _requestResponseHandler = requestResponseHandler;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {



            string remoteIpAddress = context.Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            if (context.Request.Headers.ContainsKey("X-Forwarded-For"))
                remoteIpAddress = context.Request.Headers["X-Forwarded-For"];

            string.Join(" ", context.Request.Headers);
            var conn = string.Join("", context.Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString());
            Log.Information("Client details headers {IP}, {Headers}", remoteIpAddress,  conn);


            var model = new RequestProfilerModel
            {
                RequestTime = new DateTimeOffset(),
                Context = context,
                Request = await FormatRequest(context)
            };

            Stream originalBody = context.Response.Body;

            using (MemoryStream newResponseBody = _recyclableMemoryStreamManager.GetStream())
            {
                context.Response.Body = newResponseBody;
                await _next(context);

                newResponseBody.Seek(0, SeekOrigin.Begin);
                String response = LocalEncoding.GetString(newResponseBody.ToArray());
                string code = ResponseCodesList.Success;
                string message = "Successful";
                KoboWashResult newBody = JsonConvert.DeserializeObject<KoboWashResult>(response);
                Object data = null;
                if (newBody.ResponseCode != null)
                {
                    code = newBody.ResponseCode;
                    message = newBody.ResponseMessage;

                }
                else
                    data = JsonConvert.DeserializeObject(response);

                KoboWashResult res = new KoboWashResult(code, message)
                {
                    Data = data
                };
                String result = JsonConvert.SerializeObject(res);
                _logger.Log(LogLevel.Information, result);
                byte[] byteArray = Encoding.ASCII.GetBytes(result);
                MemoryStream stream = new MemoryStream(byteArray);
                await stream.CopyToAsync(originalBody);

                stream.Seek(0, SeekOrigin.Begin);
                model.Response = FormatResponse(context, stream);
                model.ResponseTime = new DateTimeOffset();
                _requestResponseHandler(model);
            }
    
        }

        private string FormatResponse(HttpContext context, MemoryStream newResponseBody)
        {
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;

            return $"Http Response Information: {Environment.NewLine}" +
                   $"Schema:{request.Scheme} {Environment.NewLine}" +
                   $"Host: {request.Host} {Environment.NewLine}" +
                   $"Path: {request.Path} {Environment.NewLine}" +
                   $"QueryString: {request.QueryString} {Environment.NewLine}" +
                   $"StatusCode: {response.StatusCode} {Environment.NewLine}" +
                   $"Response Body: {ReadStreamInChunks(newResponseBody)} {Environment.NewLine}"+
                   $"------------------------------------------------------------------------------------------ {Environment.NewLine}";
        }

        private async Task<string> FormatRequest(HttpContext context)
        {
            HttpRequest request = context.Request;

            return $"-------------------------------------------------------------------------------------------{Environment.NewLine}" +
                   $"Http Request Information: {Environment.NewLine}" +
                   $"Schema:{request.Scheme} {Environment.NewLine}" +
                   $"Host: {request.Host} {Environment.NewLine}" +
                   $"Path: {request.Path} {Environment.NewLine}" +
                   $"QueryString: {request.QueryString} {Environment.NewLine}" +
                   $"Request Body: {await GetRequestBody(request)}";
        }

        public async Task<string> GetRequestBody(HttpRequest request)
        {
            request.EnableBuffering();
            request.EnableRewind();
            using (var requestStream = _recyclableMemoryStreamManager.GetStream())
            {
                await request.Body.CopyToAsync(requestStream);
                request.Body.Seek(0, SeekOrigin.Begin);
                return ReadStreamInChunks(requestStream);
            }
        }

        private static string ReadStreamInChunks(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            string result;
            using (var textWriter = new StringWriter())
            using (var reader = new StreamReader(stream))
            {
                var readChunk = new char[ReadChunkBufferLength];
                int readChunkLength;
                //do while: is useful for the last iteration in case readChunkLength < chunkLength
                do
                {
                    readChunkLength = reader.ReadBlock(readChunk, 0, ReadChunkBufferLength);
                    textWriter.Write(readChunk, 0, readChunkLength);
                } while (readChunkLength > 0);

                result = textWriter.ToString();
            }

            return result;
        }

        public class RequestProfilerModel
        {
            public DateTimeOffset RequestTime { get; set; }
            public HttpContext Context { get; set; }
            public string Request { get; set; }
            public string Response { get; set; }
            public DateTimeOffset ResponseTime { get; set; }
        }
    }
}