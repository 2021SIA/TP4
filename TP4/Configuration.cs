using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace TP4
{
    public class Configuration
    {
        public string Network { get; set; }
        public string Patterns { get; set; }
        public int PatternRows { get; set; } = 5;
        public int PatternColumns { get; set; } = 5;
        public int TestPattern { get; set; } = 0;
        public double Noise { get; set; } = 0.2;

        public static Configuration FromYamlFile(string path)
        {
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(UnderscoredNamingConvention.Instance)
                .Build();
            return deserializer.Deserialize<Configuration>(File.OpenText(path));
        }
        public Configuration() { }
    }
}
