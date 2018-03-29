namespace ViewAnalysis.Models
{
    internal abstract class BaseModel
    {
        public virtual string Name { get; set; }

        public string XmlFile { get; set; }
    }
}