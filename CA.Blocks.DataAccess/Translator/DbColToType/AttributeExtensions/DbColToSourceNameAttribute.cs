using System;

namespace CA.Blocks.DataAccess.Translator.DbColToType.AttributeExtensions
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DbColToSourceNameAttribute : Attribute
    {

        private readonly string  _sourceName;

        public string SourceName  => this._sourceName;


        public DbColToSourceNameAttribute(string sourceName)
        {
            if (string.IsNullOrWhiteSpace(sourceName))
            {
                throw new ArgumentException("The SourceName cannot be empty");
            }
            this._sourceName = sourceName;
        }
    }
}