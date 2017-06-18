using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionnaireSpecGenerator
{
    /// <summary>
    /// Deserializes a json questionnaire file.
    /// </summary>
    class JsonHandler
    {
        public static Questionnaire DeserializeJsonFromFile(string filepath)
        {
            Questionnaire qre;
            using (StreamReader file = File.OpenText(filepath))
            {
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    DefaultValueHandling = DefaultValueHandling.Populate,
                    NullValueHandling = NullValueHandling.Ignore
                };
                JsonSerializer serializer = JsonSerializer.Create(settings);                
                qre = (Questionnaire)serializer.Deserialize(file, typeof(Questionnaire));
            }

            //Console.WriteLine(qre.Country);

            return qre;
        }
    }
}
