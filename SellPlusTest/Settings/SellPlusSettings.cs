using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SellPlusTest.Settings
{
    public class CategoryType
    {
        public const string Regression = "Regression";
        public const string Smoke = "Smoke";
    }

    public enum RoleType
    {
        Manager,
        Underwriter,
        Rc
    }
    
    public class SellPlusSettings
    {
        public ConnectionStrings ConnectionStrings { get; set; }
        public List<Credentials> Credentials { get; set; }
    }

    public class ConnectionStrings
    {
        public string SelenoidUrl { get; set; }
        public string SiteUrl { get; set; }
        public string ShareFolder { get; set; }
    }

    public class Credentials
    {
        public string Login { get; set; }
        public string Password { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public RoleType Role { get; set; }
    }
    
}
