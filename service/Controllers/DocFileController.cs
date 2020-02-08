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

namespace service.Controllers
{
    public class DocFileController : ApiController
    {
        private static byte[] bytes;

        // private static DropboxClient dbx;

        private static DropboxClient _dbx { get ; set ; }
        private static byte[] _bytes { get => bytes; set => bytes = value; }
        private static HttpPostedFile _hpf { get; set; }
        private static string _fileName { get; set; }
        private static bool _flag { get; set; }


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
        public HttpResponseMessage Post()
        {
            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;
            string tmpname = PosUtil.ConvertToTimestamp(DateTime.Now).ToString();

            try
            {
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
                    result = Request.CreateResponse(HttpStatusCode.Created, docfiles);
                }
                else
                {
                    result = Request.CreateResponse(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {

                result = Request.CreateResponse(HttpStatusCode.Conflict, ex.Message);

            }
            return result;
        }

        //public dbx = new DropboxClient

        static async Task _dbxRun()
        {

            using (_dbx = new DropboxClient("piuQiwTZUlwAAAAAAAC5sxZkD1aF50J-4l0SuJ5GwdmvKrKOTkrQKDVmhRXW0xmu"))
            {
                string remotePath = "/products/" + _fileName;
                var full = await _dbx.Users.GetCurrentAccountAsync();
                try
                {
                    var list = await _dbx.Files.ListFolderAsync(string.Empty);
                    var checkFolderExist = list.Entries.Where(x => x.Name == "products").FirstOrDefault() == null;
                    if (checkFolderExist)
                        await _dbx.Files.CreateFolderAsync("/products", false);

                    
                    var _tmp = await _dbx.Files.UploadAsync(remotePath,
                                      //WriteMode.Overwrite.Instance,
                                      body: _hpf.InputStream);
              
                    // var _tmp2 =_dbx.Sharing.CreateSharedLinkWithSettingsAsync("/products/" + _fileName);
                    var result = await _dbx.Sharing.CreateSharedLinkWithSettingsAsync(remotePath);
                    var url = result.Url;
                    url = url.Replace("www", "dl");
                    url = url.Replace("?dl=0", "");
                    _flag = true;
                    Console.WriteLine("test");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
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
