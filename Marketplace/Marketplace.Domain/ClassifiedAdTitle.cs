using System;
using System.Text.RegularExpressions;
using Marketplace.Framework;

namespace Marketplace.Domain
{
    public class ClassifiedAdTitle : Value<ClassifiedAdTitle>
    {
        #region factories

        public static ClassifiedAdTitle FromString(string title)
        {
            CheckValidity(title);
            return new ClassifiedAdTitle(title);
        }

        public static ClassifiedAdTitle FromHtml(string htmlTitle)
        {
            var supportedTagsReplaced = htmlTitle
                .Replace("<i>", "*")
                .Replace("</i>", "*")
                .Replace("<b>", "**")
                .Replace("</b>", "**");

            var value = new ClassifiedAdTitle(
                Regex.Replace(
                    supportedTagsReplaced, 
                    "<.*?>", 
                    string.Empty));
            CheckValidity(value);
            return new ClassifiedAdTitle(value);
        }
        #endregion
        
        public string Value { get; }
        internal ClassifiedAdTitle(string value) => Value = value;

        private static void CheckValidity(string value)
        {
            if (value.Length > 100)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(value), 
                    "Title cannot be longer than 100 characters");
            }
        }

        public static implicit operator string(ClassifiedAdTitle self) => self.Value;
    }
}