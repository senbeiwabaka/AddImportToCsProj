using ViewAnalysis.Enums;

namespace ViewAnalysis.Models.Targets
{
    internal sealed class MemberModel : BaseModel
    {
        public MemberModel()
        {
        }

        public MemberModel(TypeModel typeModel)
        {
            TypeModel = typeModel;
        }

        public Kinds Kind { get; set; }

        public bool Static { get; set; }

        public Accessibilities Accessibility { get; set; }

        public bool ExternallyVisible { get; set; }

        public TypeModel TypeModel { get; }

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}";
        }
    }
}