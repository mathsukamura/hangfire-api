using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3.Util;

namespace apiemail.Service.AWSService;

public interface IAwsService
{
    Task<ListBucketsResponse> ListBucketAsync();
    Task<PutBucketResponse?> CreateBucketAsync(string nameBucket);
    Task<bool> UploadFileInS3(List<IFormFile> files, string nameBucket);
    Task<bool?> DeleteFileAsync(List<string> nameFile, string nameBucket);
}
public class AwsService : IAwsService
{
    
    public async Task<ListBucketsResponse> ListBucketAsync()
    {
        var connect = ConnectionS3Async();
        
        var list = await connect.ListBucketsAsync();

        return list;
    }

    public async Task<PutBucketResponse?> CreateBucketAsync(string nameBucket)
    {
        var connect = ConnectionS3Async();
        
        if (AmazonS3Util.DoesS3BucketExistV2Async(connect, nameBucket) != null)
        {
          var newBucket =  await connect.PutBucketAsync(nameBucket);
          
          return newBucket;
        }
        
        return null;
    }

    public async Task<bool> UploadFileInS3(List<IFormFile> files, string nameBucket)
    {
        var connect = ConnectionS3Async();
        var fileTransferUtility = new TransferUtility(connect);

        if (AmazonS3Util.DoesS3BucketExistV2Async(connect, nameBucket) == null)
        {
            return false;
        }

        foreach (var file in files)
        {
            var fileName = Path.GetFileName(file.FileName);
                
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);

                stream.Position = 0;
                    
                await fileTransferUtility.UploadAsync(stream, nameBucket, fileName);
            }
        }
        
        return true;
    }

    public async Task<bool?> DeleteFileAsync(List<string> nameFile, string nameBucket )
    {
        var connection = ConnectionS3Async();
        
        if (AmazonS3Util.DoesS3BucketExistV2Async(connection, nameBucket) == null)
        {
            return false;
        }

        foreach (var file in nameFile)
        {
            var deleteObjectRequest = new DeleteObjectRequest
            {
                BucketName = nameBucket,
                Key = file
            };
            await connection.DeleteObjectAsync(deleteObjectRequest);
        }

        return true;
    }

    public async Task<bool> AlertEmail()
    {
        return true;
    }   

    private AmazonS3Client ConnectionS3Async()
    {
        var s3ClientConfig = new AmazonS3Config
        {
            ServiceURL = "http://localhost:4566",
            ForcePathStyle = true
        };

        var s3Client = new AmazonS3Client("Zu8VgBoZMU2xcmOEeS70", "xvnjYyFQyFs44iuUagi4kTHiOGvlK1PiX64LiwOy", s3ClientConfig);

        return s3Client;
    }
}