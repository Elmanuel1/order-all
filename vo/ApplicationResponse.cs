﻿using System.Net;

namespace SwaggerApp.vo
{
    public class ApplicationResponse<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public T Result { get; set; }
    }
}
