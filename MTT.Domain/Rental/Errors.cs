using FluentResults;

namespace MTT.Domain.Rental
{
    public struct Errors
    {
        public static IError MotoAddedAlready => new Error(nameof(MotoAddedAlready));
        public static IError MotoNotFound => new Error(nameof(MotoNotFound));
        public static IError MotoRented => new Error(nameof(MotoRented));
        public static IError Validation => new Error(nameof(Validation));
        public static IError CNPJAddedAlready => new Error(nameof(CNPJAddedAlready));
        public static IError CNHAlreadyExist => new Error(nameof(CNHAlreadyExist));
        public static IError CNHCategoryNotAllowedForRental => new Error(nameof(CNHCategoryNotAllowedForRental));
        public static IError CNHCategoryNotAllowed => new Error(nameof(CNHCategoryNotAllowed));
        public static IError CNHUploadUnexpectedError => new Error(nameof(CNHUploadUnexpectedError));
        public static IError MotoboyNotFound => new Error(nameof(MotoboyNotFound));
        public static IError MotoUnavailable => new Error(nameof(MotoUnavailable));
        public static IError RentalNotFound => new Error(nameof(RentalNotFound));
	}
}
