using System;
using System.Collections.Generic;

namespace Learning_hub.Api
{
    public class ApiResponse
    {
        public int id { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Role { get; set; }
        public string TinhTrang { get; set; }
        public object Data { get; set; }
    }

}
