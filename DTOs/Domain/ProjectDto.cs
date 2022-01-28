using System;
using System.Collections.Generic;

namespace ADO.Tool.DTOs
{
    public class ProjectResponseDto { 
        public int count { get; set; }  
        public List<ProjectDetailDto> value { get; set; }
    }
    public class ProjectDetailDto
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string description { get; set; } 
        public string url { get; set; } 
        public string state { get; set; }
    }
}
