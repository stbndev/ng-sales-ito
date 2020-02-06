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
        public static DropboxClient dbx { get; set; }
        public static byte[] bytes { get; set; }
        public static HttpPostedFile MyProperty { get; set; }

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
                        //var filePath = HttpContext.Current.Server.MapPath("~/images/" + tmpname);
                        //postedFile.SaveAs(filePath);

                        string fname = Path.GetFileName(postedFile.FileName);
                        string filePath = HttpContext.Current.Server.MapPath(Path.Combine("~/images", fname));
                        postedFile.SaveAs(filePath);
                        docfiles.Add(filePath);



                        // start 
                        
                        MyProperty = postedFile;

                        var tmp =  Task.Run((Func<Task>)_dbxRun);
                        tmp.Wait();
                        

                        using (var stream = new MemoryStream())
                        {
                            postedFile.InputStream.CopyTo(stream);
                            bytes = stream.ToArray();
                        }
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

            using (dbx = new DropboxClient("piuQiwTZUlwAAAAAAAC5sxZkD1aF50J-4l0SuJ5GwdmvKrKOTkrQKDVmhRXW0xmu"))
            {

                var full = await dbx.Users.GetCurrentAccountAsync();
                try
                {
                    var list = await dbx.Files.ListFolderAsync(string.Empty);
                    var checkFolderExist = list.Entries.Where(x => x.Name == "products").FirstOrDefault() == null;
                    if (checkFolderExist)
                        await dbx.Files.CreateFolderAsync("/products", false);


                    //using (var mem = new MemoryStream(Encoding.UTF8.GetBytes("content")))
                    //{
                    //    var updated = await dbx.Files.UploadAsync(
                    //        "/products" + "/" + "file.txt",
                    //        WriteMode.Overwrite.Instance,
                    //        body: mem);
                    //    Console.WriteLine("Saved {0}/{1} rev {2}", "products", "file.txt", updated.Rev);
                    //}


                    
                    var _tmp = await dbx.Files.UploadAsync("/products/dbxtmp.png", WriteMode.Overwrite.Instance, body: MyProperty.InputStream);

                    //using (var stream = new MemoryStream())
                    //{
                    //    MyProperty.InputStream.CopyTo(stream);
                    //    bytes = stream.ToArray();
                    //}

                    //using (var mem = new MemoryStream(bytes))
                    //{
                    //    var updated = await dbx.Files.UploadAsync(
                    //        "/products" + "/" + "file2.txt",
                    //        WriteMode.Overwrite.Instance,
                    //        body: MyProperty.InputStream);
                    //}





                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }




        }
        async Task ListRootFolder()
        {
            var list = await dbx.Files.ListFolderAsync(string.Empty);

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
