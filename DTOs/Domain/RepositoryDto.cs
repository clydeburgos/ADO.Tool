using System;
using System.Collections.Generic;

namespace ADO.Tool.DTOs.Domain
{
    public class RepositoryResponseDto
    {
        public int count { get; set; }
        public List<RepositoryDto> value { get; set; }
    }

    public class RepositoryDto
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string defaultBranch { get; set; }
        public string remoteUrl { get; set; }
        public string webUrl { get; set; }
        public DateTime lastUpdateTime { get; set; }
        public ProjectDto project { get; set; }
        public long? size { get; set; }
    }

    public class ProjectDto
    {
        public Guid? id { get; set; }
        public string name { get; set; }
    }

    public class RepositoryReqestDto
    {
        public string name { get; set; }
    }

    public class ImportRepositoryRequestDto
    {
        public GitImportRequestDto parameters { get; set; } = new GitImportRequestDto();
    }

    public class GitImportRequestDto
    {
        public bool deleteServiceEndpointAfterImportIsDone { get; set; } = false;
        public GitImportGitSourceDto gitSource { get; set; }
        public Guid serviceEndpointId { get; set; }
        public object tfvcSource { get; set; } = null;

    }

    public class GitImportGitSourceDto
    {
        public bool overwrite { get; set; } = false;
        public string url { get; set; }
    }


    public class GitRepositoryDto
    {
        public string name { get; set; }
        public string url { get; set; }
    }

}
