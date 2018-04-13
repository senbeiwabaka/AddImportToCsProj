namespace ViewAnalysis.Models
{
    internal sealed class NamespaceModel : BaseModel
    {
        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}";
        }
    }
}