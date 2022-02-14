using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        const string filePath = @"C:\TFS\develop_archivace\Csob.CU.Archive\Csob.CU.Archive.Web.Js\apps\csob-archivace\src\assets\i18n\cs.json";
        const string enFilePath = @"C:\TFS\develop_archivace\Csob.CU.Archive\Csob.CU.Archive.Web.Js\apps\csob-archivace\src\assets\i18n\en.json";
        const string newEnFilePath = @"C:\TFS\develop_archivace\Csob.CU.Archive\Csob.CU.Archive.Web.Js\apps\csob-archivace\src\assets\i18n\en2.json";
        static void Main(string[] args)
        {
            var csJObject = JObject.Parse(File.ReadAllText(filePath));
            var enJObject = JObject.Parse(File.ReadAllText(enFilePath));
            var newEnJObject = csJObject.DeepClone();

            CopyTranslations(csJObject, enJObject, newEnJObject);

            File.WriteAllText(newEnFilePath, newEnJObject.ToString());

        }

        private static void CopyTranslations(JObject csJObject, JObject enJObject, JToken newEnJObject)
        {
            foreach (KeyValuePair<string, JToken> property in csJObject)
            {
                if (property.Value.HasValues)
                {
                    var jObject = (JObject)newEnJObject[property.Key];
                    CopyTranslations((JObject)property.Value, enJObject, jObject);
                }
                else
                {
                    var value = ((JValue)enJObject.SelectToken($"{csJObject.Path}.{property.Key}"))?.Value;
                    newEnJObject[property.Key] = Modify(property.Key, value);
                }
            }
        }

        private static string Modify(string key, object value)
        {
            return value == null
                ? (key.Substring(0, 1).ToUpper() + key.Substring(1)).Replace("_", " ")
                : value.ToString();
        }
    }
}
