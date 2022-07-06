using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace BinsarAPI.Models
{
    public class User
    {
        public Meta meta { get; set; }
        public List<Data> data { get; set; }
    }
    public class Data
    {
        public int id { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string email { get; set; }
        [Required]
        public string gender { get; set; }
        public string status { get; set; }

        public string field { get; set; }
        public string message { get; set; }
    }

    public class Links
    {
        public string? previous { get; set; }
        public string current { get; set; }
        public string next { get; set; }
    }

    public class Meta
    {
        public Pagination pagination { get; set; }
    }

    public class Pagination
    {
        public int total { get; set; }
        public int pages { get; set; }
        public int page { get; set; }
        public int limit { get; set; }
        public Links links { get; set; }
    }


}
