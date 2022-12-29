using System;
using System.Collections.Generic;
using System.Text;

namespace PanHouse.Model
{
   public class CustomResponse
    {
        public string Message { get; set; }
        public int MessageCode { get; set; }
        public object Response { get; set; }
    }
}
