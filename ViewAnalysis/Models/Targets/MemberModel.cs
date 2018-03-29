using System.Collections.Generic;
using ViewAnalysis.Enums;

namespace ViewAnalysis.Models.Targets
{
    internal sealed class MemberModel : BaseModel
    {
        public MemberModel() : this(null)
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

        public List<MessageModel> Messages { get; set; } = new List<MessageModel>();

        public List<AccessorModel> Accessors { get; set; } = new List<AccessorModel>();

        public TypeModel TypeModel { get; }

        public override string ToString()
        {
            return $"Count of {nameof(Messages)}: {Messages.Count} ;; Count of {nameof(Accessors)}: {Accessors.Count} ;; {nameof(Name)}: {Name}";
        }
    }
}