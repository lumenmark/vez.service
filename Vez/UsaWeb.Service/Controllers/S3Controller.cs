using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Mvc;
using UsaWeb.Service.Helper;
using UsaWeb.Service.ViewModels;

namespace UsaWeb.Service.Controllers
{
    public class S3Controller : Controller
    {

        [HttpPost]
        [Route("/S3/Save")]
        public async Task<ResultMessage> Post()//ICollection<IFormFile> files
        {
           var setting = HelperService.GetS3Setting();


            using (var client = new AmazonS3Client("XXX", "YYYY", RegionEndpoint.USEast1))
            {
                var files = HttpContext.Request.Form.Files;
                foreach (var file in files)
                {
                    try
                    {
                        using (var newMemoryStream = new MemoryStream())
                        {
                            file.CopyTo(newMemoryStream);
                            var uploadRequest = new TransferUtilityUploadRequest
                            {
                                InputStream = newMemoryStream,
                                Key = file.FileName,
                                BucketName = "BUCKETNAME-LJ",
                                CannedACL = S3CannedACL.PublicRead
                            };

                            var fileTransferUtility = new TransferUtility(client);
                            await fileTransferUtility.UploadAsync(uploadRequest);
                        }
                    }
                    catch (Exception ex)
                    { 
                    }
                    
                }
            }
            return new ResultMessage { Status = "ok" };
        }

        [HttpPost]
        [Route("/S3/Save2")]
        public async Task<ResultMessage> Post2()//ICollection<IFormFile> files
        {
            var setting = HelperService.GetS3Setting();
            using (var client = new AmazonS3Client(setting.accessKeyId, setting.accessKeySecret, RegionEndpoint.USWest2))
            {
                var files = HttpContext.Request.Form.Files;
                foreach (var file in files)
                {
                    try
                    {
                        using (var newMemoryStream = new MemoryStream())
                        {
                            file.CopyTo(newMemoryStream);
                            var uploadRequest = new TransferUtilityUploadRequest
                            {
                                InputStream = newMemoryStream,
                                Key = file.FileName,
                                BucketName = "vez-health",
                                CannedACL = S3CannedACL.PublicRead
                            };

                            var fileTransferUtility = new TransferUtility(client);
                            await fileTransferUtility.UploadAsync(uploadRequest);
                        }
                    }
                    catch (Exception ex)
                    {
                    }

                }
            }
            return new ResultMessage { Status = "ok" };
        }
    }
}
