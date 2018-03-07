﻿using System.Collections.Generic;
using System.Linq;

namespace MotiNet
{
    public class GenericResult
    {
        private static readonly GenericResult _success = new GenericResult { Succeeded = true };
        private List<GenericError> _errors = new List<GenericError>();

        public bool Succeeded { get; protected set; }

        public IEnumerable<GenericError> Errors => _errors;

        public static GenericResult Success => _success;

        public static GenericResult Failed(params GenericError[] errors)
        {
            var result = new GenericResult { Succeeded = false };
            if (errors != null)
            {
                result._errors.AddRange(errors);
            }
            return result;
        }

        public static GenericResult GetResult(IEnumerable<GenericError> errors)
        {
            return errors.Count() > 0 ? Failed(errors.ToArray()) : Success;
        }

        public override string ToString()
        {
            return Succeeded ?
                   "Succeeded" :
                   $"Failed: {string.Join(", ", Errors.Select(x => x.Code).ToList())}";
        }
    }
}
