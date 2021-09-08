using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NomaNova.Ojeda.Client.Utils
{
    internal class QueryStringBuilder
    {
        private readonly List<KeyValuePair<string, string>> _parameters = new();

        public static QueryStringBuilder New()
        {
            return new ();
        }

        public QueryStringBuilder Add(string name, string value, bool urlEncode = true)
        {
            if (urlEncode)
            {
                value = HttpUtility.UrlEncode(value);
            }
            
            _parameters.Add(new KeyValuePair<string, string>(name, value));
            
            return this;
        }
        
        public QueryStringBuilder Add(string name, bool? value)
        {
            return Add(name, value?.ToString(), false);
        }
        
        public QueryStringBuilder Add(string name, int? value)
        {
            return Add(name, value?.ToString(), false);
        }
        
        public QueryStringBuilder Add(string name, IEnumerable<string> values)
        {
            if (values == null)
            {
                return this;
            }

            foreach (var value in values)
            {
                Add(name, value);
            }

            return this;
        }
        
        public string Build()
        {
            var parameters = _parameters
                .Where(_ => !string.IsNullOrEmpty(_.Value))
                .Select(_ => $"{HttpUtility.UrlEncode(_.Key)}={_.Value}")
                .ToArray();

            return "?" + string.Join("&", parameters);
        }
    }
}