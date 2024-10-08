using Amazon.Runtime.Internal.Util;
using Amazon.S3;
using Amazon.S3.Model;
using FluentResults;
using Microsoft.Extensions.Logging;
using MTT.Domain.Rental;
using MTT.Domain.Shared;

namespace MTT.Infra.Storage
{
	public class S3StorageService : IStorageService
	{
		private readonly IAmazonS3 _s3Client;
		private readonly ILogger<S3StorageService> _logger;
		private const string BUCKET_NAME = "BUCKET_NAME";

		public S3StorageService(IAmazonS3 s3Client, ILogger<S3StorageService> logger)
		{
			_s3Client = s3Client;
			_logger = logger;
		}

		public async Task<Result> UploadAsync(string fileName, string base64String)
		{
            return Result.Ok();
            //try
            //{
            //	var fileBytes = Convert.FromBase64String(base64String);
            //	using (var stream = new MemoryStream(fileBytes))
            //	{
            //		var putRequest = new PutObjectRequest
            //		{
            //			BucketName = BUCKET_NAME,
            //			Key = fileName,
            //			InputStream = stream,
            //			ContentType = "application/octet-stream"
            //		};

            //		await _s3Client.PutObjectAsync(putRequest);
            //	}

            //	return Result.Ok();
            //}
            //catch (Exception ex)
            //{
            //	_logger.LogError(ex.Message);
            //	return Result.Fail(Errors.CNHUploadUnexpectedError);
            //}

        }
	}
}



