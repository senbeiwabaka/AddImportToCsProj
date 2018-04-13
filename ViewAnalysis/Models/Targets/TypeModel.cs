using ViewAnalysis.Enums;

namespace ViewAnalysis.Models.Targets
{
    internal sealed class TypeModel : BaseModel
    {
        public TypeModel()
        {
        }

        public TypeModel(NamespaceModel namespaceModel)
        {
            NamespaceModel = namespaceModel;
        }

        public Kinds Kind { get; set; }

        public Accessibilities Accessibility { get; set; }

        public bool ExternallyVisible { get; set; }

        public NamespaceModel NamespaceModel { get; }

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}";
        }
    }
}