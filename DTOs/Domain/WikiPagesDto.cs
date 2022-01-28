using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.Tool.DTOs.Domain
{
    public class WikiSearchPayload {
        public string searchText { get; set; }
        public object filters { get; set; }
        public bool includeFacets { get; set; }
    }
    public class WikiPagesDto
    {
        public string path { get; set; }
        public string url { get; set; }
        public IEnumerable<WikiSubPagesDto> subPages { get; set; }
    }

    public class WikiSubPagesDto
    {
        public string path { get; set; }
        public string url { get; set; }
        public IEnumerable<WikiSubPagesDto> subPages { get; set; }
    }
}
