﻿using PasswordVaildationTools.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Client
{
    public class PasswordValidationService : IPasswordValidationService
    {
        private IHashingProvider _hashingProvider;
        private ISaltStrategy _saltStrategy;
        private IPasswordRule _passwordRule;
        

        public PasswordValidationService(
            IHashingProvider hashingProvider,
            ISaltStrategy saltStrategy,
            IPasswordRule passwordRule)
        {
            _hashingProvider = hashingProvider;
            _passwordRule = passwordRule;
            _saltStrategy = saltStrategy;
        }

        public bool Validate(string password)
        {
            return _passwordRule.Validate(password);
        }

        public bool VaildatePassword(string pwd, byte[] pwdCheck, byte[] salt)
        {
            var formattedPwdCheck =_saltStrategy.Format(pwdCheck, salt);
            var hashedPwd = _hashingProvider.ComputeHash(formattedPwdCheck);
            var userPwdData= Encoding.UTF8.GetString(hashedPwd);

            if (pwd.Length != userPwdData.Length)
                return false;

            for(var i = 0; i < pwd.Length; i++)
            {
                if (pwd[i] != userPwdData[i])
                    return false;
            }

            return true;
        }

        public string GeneratePassword()
        {
            return _passwordRule.Generate();
        }

        public byte[] HashPassword(byte[] pwd,byte[] salt)
        {
            var formattedPwd = _saltStrategy.Format(pwd, salt);
            return _hashingProvider.ComputeHash(formattedPwd);
        }
    }
}
