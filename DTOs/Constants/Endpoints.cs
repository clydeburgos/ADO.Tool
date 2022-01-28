namespace ADO.Tool.DTOs.Constants
{
    public static class Endpoints
    {
        public const string baseADOUrl = "https://dev.azure.com";
        public const string almSearchADOUrl = "https://almsearch.dev.azure.com";
        public const string almOrgSearchADOUrl = "https://{organization}.almsearch.visualstudio.com";

        public const string apiProjectsSuffixUrl = "_apis/projects?api-version=5.0";
        public const string apiReposSuffixUrl = "_apis/git/repositories?api-version=5.0";
        public const string apiWikiSuffixUrl = "_apis/wiki/wikis?api-version=5.0";
        public const string apiWikiPagesSuffixUrl = @"{project}/_apis/wiki/wikis/{wikiIdentifier}/pages?path=/&recursionLevel=120&includeContent=True&api-version=5.0";
        public const string apiWikiSearchSuffixUrl = "_apis/search/wikisearchresults?api-version=6.0-preview.1";
        public const string apiWikiSearchResultSuffixUrl = "{project}/_apis/search/wikiSearchResults?api-version=6.0-preview.1";
       

        public const string apiServiceEndpointSuffixUrl = "_apis/serviceendpoint/endpoints?api-version=5.0";
        public const string apiWiki = "_apis/serviceendpoint/endpoints?api-version=5.0";
    }
}
