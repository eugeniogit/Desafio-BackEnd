using FluentResults;

namespace MTT.Domain.Rental
{
    public static class Extensions
    {
        public static T Match<T>(
            this Result result,
            Func<T> onSuccess,
            Func<IEnumerable<string>, T> onFailure)
        {
            return result.IsSuccess ? onSuccess() : onFailure(result.Errors.Select(m => m.Message));
        }

        public static T Match<T, Model>(
            this Result<Model> result,
            Func<T> onSuccess,
            Func<IEnumerable<string>, T> onFailure)
        {
            return result.IsSuccess ? onSuccess() : onFailure(result.Errors.Select(m => m.Message));
        }
    }
}
