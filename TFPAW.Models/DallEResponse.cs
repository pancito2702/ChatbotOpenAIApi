using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFPAW.Models
{
    public class DallEResponse
    {

            public int created { get; set; }
            public List<Data> data { get; set; }
    

    }


    public class Data
    {
        public string revised_prompt { get; set; }
        public string url { get; set; }
    }
}
