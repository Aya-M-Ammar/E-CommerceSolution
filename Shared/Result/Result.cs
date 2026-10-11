using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Result
{
    // Represents the result of an operation,
    // which can be either successful or failed with associated errors.
    //Exception is Expected to be handled 
    public class Result
    {
        // The list of errors associated with the result.
        protected readonly List<Error> _errors = [];
        // Indicates whether the result is successful (no errors).
        public bool IsSuccess => _errors.Count == 0;
        // Indicates whether the result is a failure (has errors).
        public bool IsFailure => !IsSuccess;
        // Provides a read-only list of errors associated with the result.
        public IReadOnlyList<Error> Errors => _errors;
        // Constructors for creating instances of the Result class If Result is successful
        protected Result() { }
        // Constructors for creating instances of the Result class If Result is failed with multiple Errors
        protected Result(List<Error> errors)
        {
           _errors.AddRange(errors);
        }
        // Constructors for creating instances of the Result class If Result is failed one Error
        protected Result(Error error)
        {
            _errors.Add(error);
        }
        // Static factory methods for creating instances of the Result class
        public static Result Ok() => new Result();
        public static Result Fail(Error error) => new Result(error);
        public static Result Fail(List<Error> errors) => new Result(errors);
    
    }
    // Generic Result class that can hold a value of type TValue
    public class  Result<TValue>:Result
    {
        // The value associated with the result. It is only accessible if the result is successful.
        private readonly TValue _value;
        public TValue Value => IsSuccess ? _value : throw new InvalidOperationException("Cannot access the value of a failed result.");

        private Result(TValue value) : base()
        {
            _value = value;
        }
        private Result(Error error) : base(error)
        {
            _value = default!;
        }
        private Result(List<Error> errors) : base(errors)
        {
            _value = default!;
        }

        public  static Result<TValue> Ok(TValue value) => new (value);
        public new static Result<TValue> Fail(Error error) => new (error);
        public new static Result<TValue> Fail(List<Error> errors) => new (errors);
        //implicit casting
        // Implicit conversion operators for converting TValue, Error, and List<Error> to Result<TValue>
        public static implicit operator Result<TValue>(TValue value) => new (value);
        public static implicit operator Result<TValue>(Error error) => new(error);
        public static implicit operator Result<TValue>(List<Error> errors) => new(errors);





    }
}
