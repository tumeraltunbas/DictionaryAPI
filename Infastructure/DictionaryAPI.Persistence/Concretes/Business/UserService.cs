using DictionaryAPI.Application.Abstracts.Business;
using DictionaryAPI.Application.Abstracts.DAL;
using DictionaryAPI.Application.Abstracts.Security.Hash;
using DictionaryAPI.Application.DTO.DTOs;
using DictionaryAPI.Application.DTO.DTOValidators;
using DictionaryAPI.Application.Utils.Constants;
using DictionaryAPI.Application.Utils.Result;
using DictionaryAPI.Domain.Entities;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryAPI.Persistence.Concretes.Business
{
    public class UserService : IUserService
    {
        IHashHelper _hashHelper;
        IUserDal _userDal;
        public UserService(IHashHelper hashHelper, IUserDal userDal)
        {
            _hashHelper = hashHelper;
            _userDal = userDal;
        }

        public Result SignUp(SignUpDto signUpDto)
        {
            SignUpDtoValidator validator = new(); //Creating an instance from DTO Validator class
            ValidationResult result = validator.Validate(signUpDto); //Validate DTO

            if (result.IsValid == false)
            {
                return new ErrorDataResult<List<ValidationFailure>>(result.Errors); //If DTO is not valid, return errors to client
            }

            //Unique check will be added.

            Tuple<byte[], byte[]> hashResult = _hashHelper.GenerateHash(signUpDto.Password);

            User user = new(
                username: signUpDto.Username,
                email: signUpDto.Email,
                birthDate: signUpDto.BirthDate,
                gender: signUpDto.Gender,
                passwordSalt: hashResult.Item1,
                passwordHash: hashResult.Item2
                );

            _userDal.Add(user);

            return new SuccessResult(Message.UserCreated);

        }
    }
}
