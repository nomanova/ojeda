using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace NomaNova.Ojeda.Api.Utils
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class DirectoryAttribute : ValidationAttribute
    {
        public DirectoryAttribute() : base("Directory path does not exist")
        {
        }

        public override bool IsValid(object value)
        {
            if (!(value is string directory))
            {
                return true;
            }

            return string.IsNullOrEmpty(directory) || Directory.Exists(directory);
        }
    }
}