using System;

namespace ADO.Tool.DTOs.Domain
{
    public class ServiceEndpointResponseDto
    {
        public Guid id { get; set; }
        public string name { get; set; }
    }

    public class ServiceEndpointRequestDto { 
        public string name { get; set; }
        public string type { get; set; } = "git";
        public string url { get; set; }
        public ServiceEndpointAuthDto authorization { get; set; } = new ServiceEndpointAuthDto();
    }

    public class ServiceEndpointAuthDto { 
        public string scheme { get; set; }
        public ServiceEndpointAuthParamRequestDto parameters { get; set; }
    }

    public class ServiceEndpointAuthParamRequestDto { 
        public string username { get; set; }
        public string password { get; set; }
    }
}
