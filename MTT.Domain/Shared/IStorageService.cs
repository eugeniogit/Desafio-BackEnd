using FluentResults;

namespace MTT.Domain.Shared
{
	public interface IStorageService
	{
		Task<Result> UploadAsync(string fileName, string base64String);
	}
}
