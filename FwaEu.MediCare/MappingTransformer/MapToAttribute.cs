using System;

namespace FwaEu.MediCare.MappingTransformer
{
    public class MapToAttribute : Attribute
    {
        public string SourcePropertyName { get; }

        public MapToAttribute(string sourcePropertyName)
        {
            SourcePropertyName = sourcePropertyName;
        }
    }
}