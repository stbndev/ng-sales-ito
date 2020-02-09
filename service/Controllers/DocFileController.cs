using posrepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.IO;
using System.Web.Http;
using Dropbox.Api;
using System.Threading.Tasks;
using System.Text;
using Dropbox.Api.Files;
using System.Configuration;

namespace service.Controllers
{
    public class DocFileController : ApiController
    {
        // private static byte[] bytes;

        // private static DropboxClient dbx;

        private static DropboxClient _dbx { get; set; }
        private static byte[] _bytes { get; set; }
        private static HttpPostedFile _hpf { get; set; }
        private static string _fileName { get; set; }
        private static bool _flag { get; set; }
        private static string folderDBX { get; set; }
        private static string tokenDBX { get; set; }
        private static string linkDBX { get; set; }

        //async Task Upload(DropboxClient dbx, string folder, string file, string content)
        //{
        //    using (var mem = new MemoryStream(Encoding.UTF8.GetBytes(content)))
        //    {
        //        var updated = await dbx.Files.UploadAsync(
        //            folder + "/" + file,
        //            WriteMode.Overwrite.Instance,
        //            body: mem);
        //        Console.WriteLine("Saved {0}/{1} rev {2}", folder, file, updated.Rev);
        //    }
        //}
        [HttpPost]
        [Route("api/products/upload")]
        public string MyFileUpload()
        {
            var request = HttpContext.Current.Request;
            var filePath = "C:\\temp\\" + request.Headers["filename"];
            using (var fs = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
            {
                request.InputStream.CopyTo(fs);
            }
            return "uploaded";
        }
        //public async Task<IHttpActionResult> Uploadfile()
        //{
        //    var request = HttpContext.Current.Request;
        //    var filePath = "C:\\temp\\" + request.Headers["filename"];

        //    if (!Request.Content.IsMimeMultipartContent())
        //        throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

        //    var provider = new MultipartMemoryStreamProvider();
        //    await Request.Content.ReadAsMultipartAsync(provider);
        //    foreach (var file in provider.Contents)
        //    {
        //        var filename = file.Headers.ContentDisposition.FileName.Trim('\"');
        //        var buffer = await file.ReadAsByteArrayAsync();
        //        //Do whatever you want with filename and its binary data.
        //    }

        //    return Ok();
        //}

        public HttpResponseMessage Post()
        {
            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;
            string tmpname = PosUtil.ConvertToTimestamp(DateTime.Now).ToString();

            if (httpRequest.Files.Count > 0)
            {
                var docfiles = new List<string>();
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];

                    //var filePath = HttpContext.Current.Server.MapPath("~/images/" + postedFile.FileName);
                    tmpname = tmpname + Path.GetExtension(postedFile.FileName);
                    _fileName = tmpname;
                    //var filePath = HttpContext.Current.Server.MapPath("~/images/" + tmpname);
                    //postedFile.SaveAs(filePath);

                    // string fname = Path.GetFileName(postedFile.FileName);
                    // string filePath = HttpContext.Current.Server.MapPath(Path.Combine("~/images", fname));
                    // postedFile.SaveAs(filePath);
                    docfiles.Add(_fileName);

                    // start 

                    _hpf = postedFile;

                    var tmp = Task.Run((Func<Task>)_dbxRun);
                    tmp.Wait();
                    // end
                }
                result = Request.CreateResponse(HttpStatusCode.Created, linkDBX);
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            return result;
        }

        //public dbx = new DropboxClient

        static async Task _dbxRun()
        {
            folderDBX = ConfigurationManager.AppSettings["dbxproducts"];
            tokenDBX = ConfigurationManager.AppSettings["dbxcredentials"];

            try
            {
                using (_dbx = new DropboxClient(tokenDBX))
                {
                    string remotePath = string.Concat("/", folderDBX, "/", _fileName);
                    // var full = await _dbx.Users.GetCurrentAccountAsync();
                    var list = await _dbx.Files.ListFolderAsync(string.Empty);
                    var checkFolderExist = list.Entries.Where(x => x.Name == "products").FirstOrDefault() == null;
                    if (checkFolderExist)
                        await _dbx.Files.CreateFolderAsync("/" + folderDBX, false);

                    var _tmp = await _dbx.Files.UploadAsync(remotePath,
                                                            body: _hpf.InputStream);

                    var result = await _dbx.Sharing.CreateSharedLinkWithSettingsAsync(remotePath);
                    var url = result.Url;
                    url = url.Replace("www", "dl");
                    url = url.Replace("?dl=0", "");
                    linkDBX = url;
                    _flag = true;
                }
            }
            catch (Exception ex)
            {
                // TODO
                // put nlog config here
                Console.WriteLine(ex.Message);
            }
        }
        async Task ListRootFolder()
        {
            var list = await _dbx.Files.ListFolderAsync(string.Empty);

            // show folders then files
            foreach (var item in list.Entries.Where(i => i.IsFolder))
            {
                Console.WriteLine("D  {0}/", item.Name);
            }

            foreach (var item in list.Entries.Where(i => i.IsFile))
            {
                Console.WriteLine("F{0,8} {1}", item.AsFile.Size, item.Name);
            }
        }
    }
}
