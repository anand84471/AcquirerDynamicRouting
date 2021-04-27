using Core.Features.ExceptionHandling.Concrete;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Core.Validation
{
   
    public class ValidationHander<TValidator, TValidatorType>
        where TValidator : AbstractValidator<TValidatorType>, new()
       
    {
        public static void DoValidate(TValidatorType ob, string ruleName)
        {
            var _validator = new TValidator();
           
            ValidationResult result = _validator.Validate(ob, ruleSet: ruleName);
            if (!result.IsValid)
            {
                 
                throw new InvalidRequestException(result.Errors.Select(X => Convert.ToInt32(X.ErrorCode)).ToList());
            }
        }
    }
}