using ViewAnalysis.Enums;

namespace ViewAnalysis.Models.Targets
{
    internal sealed class AccessorModel : BaseModel
    {
        public AccessorModel()
        {
        }

        public AccessorModel(MemberModel memberModel)
        {
            MemberModel = memberModel;
        }

        public Kinds Kind { get; set; }

        public bool Static { get; set; }

        public Accessibilities Accessibility { get; set; }

        public bool ExternallyVisible { get; set; }

        public MemberModel MemberModel { get; }

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}";
        }
    }
}