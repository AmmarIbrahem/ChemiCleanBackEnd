using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using ChemiClean.Models;
using ChemiClean.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;


namespace ChemiClean.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ChemiCleanController : ControllerBase
    {
        IRepository repository;
        IWebHostEnvironment environment;
        private static Dictionary<string, string> MimeTypes = new Dictionary<string, string>()
        {
                {"text/plain" ,".txt"},
                {"application/pdf" ,".pdf"},
                {"application/vnd.ms-word",  ".docx"}
        };

        public ChemiCleanController(IRepository _repository, IWebHostEnvironment _environment)
        {
            repository = _repository;
            environment = _environment;
        }

        /// <summary>
        /// Get All Data sheets
        /// </summary>
        /// <returns>List of Data sheets</returns>
        // GET: /getData
        [Route("/getData")]
        public ActionResult<List<DataSheets>> GetAllDataSheets()
        {
            var ds = repository.GetAllDataSheets();
            if (ds.Count == 0)
                return NotFound();
            return Ok(ds);
        }
      
        /// <summary>
        /// Save data sheet file locally
        /// </summary>
        /// <param name="dataSheetId"></param>
        /// <param name="sheetURL"></param>
        /// <returns>Action Result indicating saving file result</returns>
        // POST: /upload/1 
        [HttpPost("/upload/{dataSheetId}")]
        public IActionResult SaveProductDataSheet(int dataSheetId, [FromForm]string sheetURL)
        {
            //Get data sheet from DB
            var dataSheet = repository.getDataSheet(dataSheetId);
            //Check url Validitiy & data sheet info existance
            if (IsUrlValid(sheetURL) && dataSheet!=null)
            {
                //Download data sheet 
                var result = Download(dataSheetId, sheetURL);
                return result;
            }
            else
            {
                //Url is not valid 
                // update file status to invalid.
                repository.UpdateURLStatus(dataSheetId, sheetURL, UrlState.Invalid);
                // TODO -- Remove old hash value & Delete file if Exist 
                return NotFound("URL doesn't exist");
            }
            //DB update trigger is made to record date 
            
        }

        /// <summary>
        /// Check url validation
        /// Send request to url & return response
        /// </summary>
        /// <param name="url"></param>
        /// <returns>
        ///     True --> Request sent successfully.
        ///     False --> Request was not sent successfully or Response with 404
        /// </returns>
        private bool IsUrlValid(string url)
        {
            HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(url);
            httpReq.AllowAutoRedirect = false;
            try
            {
                HttpWebResponse httpRes = (HttpWebResponse)httpReq.GetResponse();
                if (httpRes.StatusCode == HttpStatusCode.NotFound)
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Stream url data 
        /// Check if file already exist locally & up-to-date
        /// Download file if not Exist.
        /// </summary>
        /// <param name="dataSheetId"></param>
        /// <param name="path"></param>
        /// <returns>
        ///     Action Result indicates result of downloading file
        ///     "Saved Successfully, File is Updated "--> if file has been updated
        ///     "File already Exist"  --> if file already exist & up-to-date
        ///     "File Extension not supported" --> if file extension is not .pdf, .docx, .txt
        ///     "URL doesn't exist" --> if url not found
        /// </returns>
        public IActionResult Download(int dataSheetId, string path)
        {
            using(WebClient wc = new WebClient())
            {
                try
                {
                    //Generate Random name for the data sheet
                    string filename = Guid.NewGuid().ToString("N");
                    //Start download the data sheet
                    var result = wc.DownloadData(path);
                    //Get file extension
                    var contentType = wc.ResponseHeaders["Content-Type"];
                    //Check avilability for file extension
                    if (contentType != null && MimeTypes.ContainsKey(contentType))
                    {
                        var filePath = @$"{environment.WebRootPath}\Uploads\{filename}{MimeTypes[contentType]}";
                        using (var md5 = MD5.Create())
                        {
                            var hash = md5.ComputeHash(result);
                            //Check if file not exist locally or not up-to-date
                            if(!hash.SequenceEqual(repository.getHashValue(dataSheetId)))
                            {
                                //Write File to local destination.
                                System.IO.File.WriteAllBytes(filePath, result);
                                //Store local path & hash value
                                repository.StoreFile(dataSheetId, filePath, hash);
                                //Update status to valid
                                repository.UpdateURLStatus(dataSheetId, path, UrlState.Valid);
                                return Ok("Saved Successfully, File is Updated"); 
                            }
                        }
                        //Update status to valid
                        repository.UpdateURLStatus(dataSheetId, path,  UrlState.Valid);
                        //File already exist locally and up-to-date
                        return Ok("File already Exist");
                    }
                    //Update status to invalid url
                    repository.UpdateURLStatus(dataSheetId, path,  UrlState.Invalid);
                    //File extension is not supported
                    return BadRequest("File Extension not supported");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    //update status to invalid url
                    repository.UpdateURLStatus(dataSheetId, path,  UrlState.Invalid);
                    //delete file if exist or leave it for user
                    return NotFound("URL doesn't exist");
                }
            }
          
        }

        //-------------------------Not Mandatory--------------------------------------------

        // GET: api/suppliers
        [Route("/suppliers")]
        public ActionResult<List<Suppliers>> GetAllSuppliers()
        {
            var suppliers = repository.GetAllSuppliers();
            if (suppliers.Count == 0)
                return NotFound();
            return Ok(suppliers);
        }

        // GET: api/suppliers/1
        [Route("/suppliers/{supplierId}/products")]
        public ActionResult<List<Products>> GetSupplierProducts(int supplierId)
        {
            var products = repository.GetSupplierProducts(supplierId);
            if (products.Count == 0)
                return NotFound();
            return Ok(products);
        }

        // GET: api/suppliers/1/products/2
        [Route("/suppliers/{supplierId}/{productId}")]
        public ActionResult<string> GetProductDataSheet(int supplierId, int productId)
        {
            var datasheet = repository.GetProductDataSheet(productId);
            if (datasheet == null)
                return NotFound();
            return Ok(datasheet);
        }

    }
}
