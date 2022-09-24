using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Gicaf.Application.GraphQL.Resolvers;
using Gicaf.Application.Interface.DTOs;
using Gicaf.Application.Interface.Services;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;


namespace Gicaf.API.Controllers
{
    internal class GraphQLQuery
    {
        public string OperationName { get; set; }
        public string NamedQuery { get; set; }
        public string Query { get; set; }
        public JObject Variables { get; set; }
    }


    public class FilePost : IFilePost
    {
        public byte[] Content { get; }
        public string ContentType { get; }
        public string ContentDisposition { get; }
        public long Length { get; }
        public string Name { get; }
        public string FileName { get; }

        public FilePost(string contentType, string contentDisposition, long length, string name, string fileName, byte[] content)
        {
            ContentType = contentType;
            ContentDisposition = contentDisposition;
            Length = length;
            Name = name;
            FileName = fileName;
            Content = content;
        }

        public FilePost(IFormFile file)
        {
            byte[] content = null;
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                content = ms.ToArray();
            }
            Content = content;
            ContentType = file.ContentType;
            ContentDisposition = file.ContentDisposition;
            Length = file.Length;
            Name = file.Name;
            FileName = file.FileName;
        }
    }

    [Route("api/gql")]
    [ApiController]
    //[AuthorizeApp("Bearer")]
    public class GraphQLController : ControllerBase
    {
        IAppServiceResolver _appServiceResolver;

        public GraphQLController(IAppServiceResolver appServiceResolver)
        {
            _appServiceResolver = appServiceResolver;
        }

        [HttpPost]
        //[Authorize("Bearer")]
        [Route("file")]
        public async Task<IActionResult> Post([FromForm]IFormCollection form)
        {
            var query = form["query"].FirstOrDefault();
            var queryObj = JObject.Parse(query);
            return await Post(queryObj);
        }

        private List<IFilePost> GetFilesFromRequest()
        {
            List<IFilePost> files = new List<IFilePost>();
            foreach (var file in Request.Form?.Files)
            {
                files.Add(new FilePost(file));
            }
            return files;
        }

        [HttpPost]
        //[Authorize("Bearer")]
        public async Task<IActionResult> Post([FromBody]JObject obj)
        {
            //CallPackage("PKG_PD_P1_FT_PESO_VALOR_COMPRA.dtsx");
            Dictionary<string, object> context = null;
            if(Request.HasFormContentType)
            {
                if (Request?.Form?.Files != null)
                {
                    var files = GetFilesFromRequest();
                    context = new Dictionary<string, object>();

                    if (files.Any())
                    {
                        context.Add("files", files);
                    }
                }
            }

            var result = _appServiceResolver.Resolve(obj, context);

            if (result.Errors?.Count > 0)
            {
                return await Task.FromResult(Ok(result.Errors.Select(x => x.Message + x?.InnerException?.Message)));
            }
            return await Task.FromResult(Ok(result));
        }

        //private void CallPackage(string packageName)
        //{
        //    // Variables
        //    string targetServerName = "localhost";
        //    string folderName = "Projects";
        //    string projectName = "Teste";
        //    //string packageName = "Package.dtsx";

        //    // Create a connection to the server
        //    string sqlConnectionString = "Data Source=" + targetServerName + ";Initial Catalog=master;Integrated Security=SSPI;";
        //    SqlConnection sqlConnection = new SqlConnection(sqlConnectionString);

        //    // Create the Integration Services object
        //    IntegrationServices integrationServices = new IntegrationServices();
        //    integrationServices.Connection = new Microsoft.SqlServer.Management.Sdk.Sfc.SqlStoreConnection(sqlConnection);

        //    //integrationServices.Connection.ServerConnection.ConnectionString = sqlConnection.ConnectionString;
        //    //integrationServices.Connection.Connect();

        //    // Get the Integration Services catalog
        //    Catalog catalog = integrationServices.Catalogs["SSISDB"];

        //    // Get the folder
        //    CatalogFolder folder = catalog.Folders[folderName];

        //    // Get the project
        //    ProjectInfo project = folder.Projects[projectName];

        //    // Get the package
        //    PackageInfo package = project.Packages[packageName];

        //    // Run the package
        //    package.Execute(false, null);
        //}
    }
}