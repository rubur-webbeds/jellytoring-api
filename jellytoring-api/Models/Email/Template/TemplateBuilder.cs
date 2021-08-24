using System.Collections.Generic;

namespace jellytoring_api.Models.Email.Template
{
    public class TemplateBuilder
    {
        private string _name;
        private Dictionary<string, string> _options = new Dictionary<string, string>();

        public TemplateBuilder SetName(string name)
        {
            _name = name;
            return this;
        }

        public TemplateBuilder AddOption(string optionKey)
        {
            _options.Add($"[{optionKey}]", string.Empty);
            return this;
        }

        public TemplateBuilder WithValue(string optionValue)
        {
            foreach(var key in _options.Keys)
            {
                if (_options[key].Equals(string.Empty))
                {
                    _options[key] = optionValue;
                }
            }

            return this;
        }

        public Template Build() => new Template {Name = _name, Options = new TemplateOptions { Options = _options } };
    }
}
