
namespace clearpixels.Models
{
    public class IdName
    {
        public IdName(long id, string name)
        {
            this.id = id.ToString();
            this.name = name;
        }

        public IdName(string id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public IdName()
        {
            id = "";
            name = "";
        }

        public string id { get; set; }
        public string name { get; set; }
    }
}