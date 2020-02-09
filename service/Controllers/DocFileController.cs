using Dropbox.Api;
using Dropbox.Api.Files;
using posrepository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace service.Controllers
{
    public class DBX
    {
        public DropboxClient dbx { get; set; }
        public byte[] bytes { get; set; }
        public HttpPostedFile hpf { get; set; }
        public string fileName { get; set; }
        public bool flag { get; set; }
        public string folderDBX { get; set; }
        public string tokenDBX { get; set; }
        public string linkDBX { get; set; }
        public string errorDBX { get; set; }
    }
    public class DocFileController : ApiController
    {
        //public static DropboxClient dbx { get; set; }
        public DBX dbx { get; set; }

        async Task Upload(DropboxClient dbx, string folder, string file, string content)
        {
            using (var mem = new MemoryStream(Encoding.UTF8.GetBytes(content)))
            {
                var updated = await dbx.Files.UploadAsync(
                    folder + "/" + file,
                    WriteMode.Overwrite.Instance,
                    body: mem);
                Console.WriteLine("Saved {0}/{1} rev {2}", folder, file, updated.Rev);
            }
        }
        //public HttpResponseMessage Post()
        //{
        //    HttpResponseMessage result = null;
        //    var httpRequest = HttpContext.Current.Request;
        //    string tmpname = PosUtil.ConvertToTimestamp(DateTime.Now).ToString();

        //    var task = Task.Run((Func<Task>)_dbxRun);
        //    task.Wait();

        //    var task2 = Task.Run((Func<Task>)ListRootFolder);
        //    task2.Wait();



        //    try
        //    {


        //        if (httpRequest.Files.Count > 0)
        //        {
        //            var docfiles = new List<string>();
        //            foreach (string file in httpRequest.Files)
        //            {

        //                var postedFile = httpRequest.Files[file];
        //                //var filePath = HttpContext.Current.Server.MapPath("~/images/" + postedFile.FileName);
        //                tmpname = tmpname + Path.GetExtension(postedFile.FileName);
        //                //var filePath = HttpContext.Current.Server.MapPath("~/images/" + tmpname);
        //                //postedFile.SaveAs(filePath);

        //                string fname = Path.GetFileName(postedFile.FileName);
        //                string filePath = HttpContext.Current.Server.MapPath(Path.Combine("~/images", fname));
        //                postedFile.SaveAs(filePath);
        //                docfiles.Add(filePath);

        //            }
        //            result = Request.CreateResponse(HttpStatusCode.Created, docfiles);
        //        }
        //        else
        //        {
        //            result = Request.CreateResponse(HttpStatusCode.BadRequest);
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        result = Request.CreateResponse(HttpStatusCode.Conflict, ex.Message);

        //    }
        //    return result;
        //}

        //public dbx = new DropboxClient
        public HttpResponseMessage Post()
        {
            HttpResponseMessage result = null;
            dbx = new DBX();
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
                    dbx.fileName = tmpname;
                    //var filePath = HttpContext.Current.Server.MapPath("~/images/" + tmpname);
                    //postedFile.SaveAs(filePath);

                    // string fname = Path.GetFileName(postedFile.FileName);
                    // string filePath = HttpContext.Current.Server.MapPath(Path.Combine("~/images", fname));
                    // postedFile.SaveAs(filePath);
                    docfiles.Add(dbx.fileName);

                    // start 

                    dbx.hpf = postedFile;

                    var tmp = Task.Run((Func<Task>)_dbxRun);
                    tmp.Wait();
                    // end
                }
                result = Request.CreateResponse(HttpStatusCode.Created, dbx.linkDBX);
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            return result;
        }
        async Task _dbxRun()

        {
            dbx.folderDBX = ConfigurationManager.AppSettings["dbxproducts"];
            dbx.tokenDBX = ConfigurationManager.AppSettings["dbxcredentials"];

            try
            {
                using (dbx.dbx = new DropboxClient(dbx.tokenDBX))
                {
                    string remotePath = string.Concat("/", dbx.folderDBX, "/", dbx.fileName);
                    // var full = await _dbx.Users.GetCurrentAccountAsync();
                    var list = await dbx.dbx.Files.ListFolderAsync(string.Empty);
                    var checkFolderExist = list.Entries.Where(x => x.Name == "products").FirstOrDefault() == null;
                    if (checkFolderExist)
                        await dbx.dbx.Files.CreateFolderAsync("/" + dbx.folderDBX, false);

                    var _tmp = await dbx.dbx.Files.UploadAsync(remotePath,
                                                            body: dbx.hpf.InputStream);

                    var result = await dbx.dbx.Sharing.CreateSharedLinkWithSettingsAsync(remotePath);
                    var url = result.Url;
                    url = url.Replace("www", "dl");
                    url = url.Replace("?dl=0", "");
                    dbx.linkDBX = url;
                    dbx.flag = true;
                }
            }
            catch (Exception ex)
            {
                // TODO
                // put nlog config here
                Console.WriteLine(ex.Message);
            }
        }
        //static async Task _dbxRun()
        //{

        //    using (dbx = new DropboxClient("piuQiwTZUlwAAAAAAAC5sxZkD1aF50J-4l0SuJ5GwdmvKrKOTkrQKDVmhRXW0xmu"))
        //    {

        //        var full = await dbx.Users.GetCurrentAccountAsync();
        //        try
        //        {
        //            var list = await dbx.Files.ListFolderAsync(string.Empty);
        //            var checkFolderExist = list.Entries.Where(x => x.Name == "products").FirstOrDefault() == null;
        //            if (checkFolderExist)
        //                await dbx.Files.CreateFolderAsync("/products", false);

                    
        //            using (var mem = new MemoryStream(Encoding.UTF8.GetBytes("content")))
        //            {
        //                var updated = await dbx.Files.UploadAsync(
        //                    "/products" + "/" + "file.txt",
        //                    WriteMode.Overwrite.Instance,
        //                    body: mem);
        //                Console.WriteLine("Saved {0}/{1} rev {2}", "products", "file.txt", updated.Rev);
        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.Message);
        //        }
        //    }


            

        //}
        //async Task ListRootFolder()
        //{
        //    var list = await dbx.Files.ListFolderAsync(string.Empty);

        //    // show folders then files
        //    foreach (var item in list.Entries.Where(i => i.IsFolder))
        //    {
        //        Console.WriteLine("D  {0}/", item.Name);
        //    }

        //    foreach (var item in list.Entries.Where(i => i.IsFile))
        //    {
        //        Console.WriteLine("F{0,8} {1}", item.AsFile.Size, item.Name);
        //    }
        //}
    }
}
