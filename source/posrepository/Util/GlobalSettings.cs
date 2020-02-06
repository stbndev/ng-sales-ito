using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace posrepository
{
    public enum CSTATUS : int
    {
        ACTIVO = 1,
        INACTIVO = 2,
        ELIMINADO = 3,
    }

    public class ResponseModel
    {
        public dynamic result { get; set; }
        public bool response { get; set; }
        public string message { get; set; }
        public string href { get; set; }
        public string function { get; set; }

        public ResponseModel()
        {
            this.response = false;
            this.message = "unexpected error";
        }
        public void SetResponse(bool r, string m = "")
        {
            this.response = r;
            this.message = m;

            if (!r && m == "") this.message = "unexpected error";
        }
    }

    public static class PosUtil
    {
        public static long ConvertToTimestamp(DateTime value)
        {
            long epoch = (value.Ticks - 621355968000000000) / 10000000;
            // epoch epoca
            return epoch;
        }
    }

    public class GlobalSettings { }
}
