using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.Tool.DTOs.Domain
{
    public class WikiResponseDto
    {
        public IEnumerable<WikiDto> value { get; set; }
    }

    public class WikiDto {
        public string url { get; set; }
        public string remoteUrl { get; set; }
        public string name { get; set; }
    }
}
