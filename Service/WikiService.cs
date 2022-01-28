using ADO.Tool.DTOs;
using ADO.Tool.DTOs.Constants;
using ADO.Tool.DTOs.Domain;
using ADO.Tool.Extension;
using Microsoft.TeamFoundation.Wiki.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.TeamFoundation.Wiki.WebApi.Contracts;

using Newtonsoft.Json;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ADO.Tool.Service
{
    public class WikiService
    {
        public static string KeyWord { get; set; }
        public static async Task<IList<WikiPage>> Search(string orgName = "", string keyword = "")
        {
            KeyWord = keyword;

            IList<WikiPage> filteredWikiPages = new List<WikiPage>();

            var orgUri = new System.Uri("https://clericalsoftware.visualstudio.com");
            WikiHttpClient wikiClient = new WikiHttpClient(orgUri, new VssBasicCredential(string.Empty, KeyValues.personalAccessToken));

            var pages = await wikiClient.GetPagesBatchAsync(null, "clericalsoft", "clericalsoft.wiki");

            foreach (var pageDetail in pages)
            {
                var page = await wikiClient.GetPageAsync("clericalsoft", "clericalsoft.wiki", pageDetail.Path,
                VersionControlRecursionType.Full, null, true);


            }

            return filteredWikiPages;
            #region RESTFUL IMPLEMENTATION
            //dynamic filters = new ExpandoObject();
            //filters.Project = new string[] { "clericalsoft" };

            //var searchPayload = new WikiSearchPayload()
            //{
            //    searchText = keyword,
            //    filters = filters,
            //    includeFacets = true
            //};
            //string jsonResponse = await HttpRequestExtension.SendRequest<WikiSearchPayload>(HttpRequestTypes.POST, searchPayload,
            //  KeyValues.personalAccessToken,
            //  Endpoints.almSearchADOUrl,
            //  KeyValues.clericalsoftFromOrg,
            //  Endpoints.apiWikiSearchResultSuffixUrl
            //  .Replace("{project}", KeyValues.clericalsoftFromProject)
            //  .Replace("{wikiIdentifier}", $"{KeyValues.clericalsoftFromProject}.wiki"));

            //string jsonResponse = await HttpRequestExtension.SendRequest<WikiSearchPayload>(HttpRequestTypes.POST, searchPayload,
            //  KeyValues.personalAccessToken,
            //  Endpoints.almOrgSearchADOUrl.Replace("{organization}", KeyValues.clericalsoftFromOrg),
            //  string.Empty,
            //  Endpoints.apiWikiSearchResultSuffixUrl
            //  .Replace("{project}", "4607afbc-1c7b-4238-acc8-8deaa26ba1fb")
            //  .Replace("{wikiIdentifier}", $"{KeyValues.clericalsoftFromProject}.wiki"));


            //var wikiSearchResult = JsonConvert.DeserializeObject<WikiPagesDto>(jsonResponse);
            #endregion
        }

        private static IList<WikiPage> SearchPages(IList<WikiPage> subPages)
        {
            IList<WikiPage> filteredWikiPages = new List<WikiPage>();
            foreach (var subPage in subPages)
            {
                if (subPage.SubPages.Count > 0)
                {
                    SearchPages(subPage.SubPages);
                }
            }
            return filteredWikiPages;
        }
    }
}
