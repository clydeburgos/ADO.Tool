using ADO.Tool.DTOs;
using ADO.Tool.DTOs.Constants;
using ADO.Tool.DTOs.Domain;
using ADO.Tool.Extension;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ADO.Tool.Service
{
    public static class ProjectRepoService
    {
        public static async Task InitImportRepository()
        {
            var projects = await GetProjectList(KeyValues.clericalsoftFromOrg);
            Console.WriteLine($"ORGANIZTION : {KeyValues.clericalsoftFromOrg}");
            foreach (var project in projects.value)
            {
                Console.WriteLine($"PROJECT : {project.name} - {project.url}");

                var repos = await GetRepositoryList(project.name);
                foreach (var repo in repos.value)
                {
                    Console.WriteLine($"REPO : {repo.webUrl}");
                    if (ValidateImportRepo(repo))
                    {
                        await ImportRepository(repo);
                    }
                }
                Console.WriteLine($"------------------------------------");
            }
        }

        public static async Task<ProjectResponseDto> GetProjectList(string orgName = "")
        {
            string jsonResponse = await HttpRequestExtension.SendRequest<ProjectResponseDto>(HttpRequestTypes.GET, null,
                KeyValues.personalAccessToken,
                Endpoints.baseADOUrl, orgName,
                Endpoints.apiProjectsSuffixUrl);
            var listOfProjects = JsonConvert.DeserializeObject<ProjectResponseDto>(jsonResponse);
            return listOfProjects;
        }

        private static bool ValidateImportRepo(RepositoryDto dto)
        {
            return dto != null & dto.size > 0 && !string.IsNullOrEmpty(dto.defaultBranch);
        }

        static async Task<RepositoryResponseDto> GetRepositoryList(string projectName = "")
        {
            string jsonResponse = await HttpRequestExtension.SendRequest<RepositoryResponseDto>(HttpRequestTypes.GET, null,
                KeyValues.personalAccessToken, Endpoints.baseADOUrl,
                KeyValues.clericalsoftFromOrg + "/" +
                projectName,
                Endpoints.apiReposSuffixUrl);
            var listOfRepositories = JsonConvert.DeserializeObject<RepositoryResponseDto>(jsonResponse);
            //apply filter
            listOfRepositories.value = listOfRepositories.value.Where(a => ValidateImportRepo(a)).ToList();

            return listOfRepositories;
        }

        static async Task<RepositoryDto> CreateRepository(string newRepoName, string destinationProject)
        {

            var repoPayload = new RepositoryReqestDto();
            repoPayload.name = newRepoName;

            string jsonResponse = await HttpRequestExtension.SendRequest<RepositoryReqestDto>(HttpRequestTypes.POST, repoPayload,
                KeyValues.personalAccessToken, Endpoints.baseADOUrl,
                KeyValues.clericalsoftToOrg + "/" +
                destinationProject,
                Endpoints.apiReposSuffixUrl);
            var newlyCreatedRepo = JsonConvert.DeserializeObject<RepositoryDto>(jsonResponse);
            Console.WriteLine("NEWLY CREATED REPO : ");
            Console.WriteLine(jsonResponse);
            return newlyCreatedRepo;
        }

        static async Task<ServiceEndpointResponseDto> HandleServiceConnectionAuth(string newlyCreatedRepoWebUrl)
        {
            string destinationProject = KeyValues.clericalsoftToProject;

            var authPayload = new ServiceEndpointRequestDto();
            authPayload.type = "git";
            authPayload.name = Guid.NewGuid().ToString("N").Substring(0, 5);
            authPayload.url = newlyCreatedRepoWebUrl;

            authPayload.authorization.scheme = "UsernamePassword";
            authPayload.authorization.parameters = new ServiceEndpointAuthParamRequestDto()
            {
                username = KeyValues.username,
                password = KeyValues.personalAccessToken
            };

            string jsonResponse = await HttpRequestExtension.SendRequest<ServiceEndpointRequestDto>(HttpRequestTypes.POST, authPayload,
              KeyValues.personalAccessToken, Endpoints.baseADOUrl,
              KeyValues.clericalsoftToOrg + "/" +
              destinationProject,
              Endpoints.apiServiceEndpointSuffixUrl);
            var serviceEndpoint = JsonConvert.DeserializeObject<ServiceEndpointResponseDto>(jsonResponse);

            return serviceEndpoint;
        }

        static async Task<RepositoryDto> ImportRepositoryCore(RepositoryDto sourceRepo, string newName, ServiceEndpointResponseDto newlyCreatedAuth)
        {

            string importApiReposSuffixUrl = "_apis/git/repositories/{newrepositoryname}/importRequests?api-version=5.0";
            var repoPayload = new ImportRepositoryRequestDto();

            repoPayload.parameters.deleteServiceEndpointAfterImportIsDone = true;
            repoPayload.parameters.gitSource = new GitImportGitSourceDto()
            {
                overwrite = false,
                url = sourceRepo.remoteUrl
            };
            repoPayload.parameters.serviceEndpointId = newlyCreatedAuth.id;

            importApiReposSuffixUrl = importApiReposSuffixUrl.Replace("{newrepositoryname}", newName);

            string jsonResponse = await HttpRequestExtension.SendRequest<ImportRepositoryRequestDto>(HttpRequestTypes.POST, repoPayload,
                KeyValues.personalAccessToken, Endpoints.baseADOUrl,
                KeyValues.clericalsoftToOrg + "/" +
                KeyValues.clericalsoftToProject,
                importApiReposSuffixUrl);
            var newlyImportedRepo = JsonConvert.DeserializeObject<RepositoryDto>(jsonResponse);
            Console.WriteLine("NEWLY IMPORTED REPO : ");
            Console.WriteLine(jsonResponse);

            return newlyImportedRepo;
        }

        static async Task<RepositoryDto> ImportRepository(RepositoryDto sourceRepo)
        {
            string newName = sourceRepo.name;
            string destinationProject = KeyValues.clericalsoftToProject;
            //create repo first

            var newlyCreatedRepo = await CreateRepository(newName, destinationProject);

            //handle serviceEndpoint
            var newlyCreatedAuth = await HandleServiceConnectionAuth(newlyCreatedRepo.webUrl);

            //actual import
            return await ImportRepositoryCore(sourceRepo, newName, newlyCreatedAuth);
        }
    }
}
