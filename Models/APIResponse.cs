﻿using System.Net;

namespace webapi_tutorial.Models;

public class APIResponse
{
    public HttpStatusCode statusCode { get; set; }
    public bool IsExitoso { get; set; } = true;
    public List<string> ErrorMessages { get; set;}
    public object Resultado { get; set; } // Puede ser cualquier tipo de dato
}
