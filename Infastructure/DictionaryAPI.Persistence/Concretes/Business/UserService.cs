using DictionaryAPI.Application.Abstracts.Business;
using DictionaryAPI.Application.DTO.DTOs;
using DictionaryAPI.Application.DTO.DTOValidators;
using DictionaryAPI.Application.Utils.Result;
using DictionaryAPI.Domain.Entities;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryAPI.Persistence.Concretes.Business
{
    public class UserService : IUserService
    {
        public Result SignUp(SignUpDto signUpDto)
        {
            SignUpDtoValidator validator = new(); //Creating an instance from DTO Validator class
            ValidationResult result = validator.Validate(signUpDto); //Validate DTO

            if (result.IsValid == false)
            {
                return new ErrorDataResult<List<ValidationFailure>>(result.Errors); //If DTO is not valid, return errors to client
            }
            
            //Unique check will be added.

            //Password hash will be added.

            //User instance will be created.

            return new SuccessResult();

        }
    }
}
