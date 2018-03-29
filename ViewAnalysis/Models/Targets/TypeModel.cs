using System.Collections.Generic;
using ViewAnalysis.Enums;

namespace ViewAnalysis.Models.Targets
{
    internal sealed class TypeModel : BaseModel
    {
        public TypeModel() : this(null)
        {
        }

        public TypeModel(NamespaceModel namespaceModel)
        {
            NamespaceModel = namespaceModel;
        }

        public Kinds Kind { get; set; }

        public Accessibilities Accessibility { get; set; }

        public bool ExternallyVisible { get; set; }


        public List<MessageModel> Messages = new List<MessageModel>();


        public List<MemberModel> Members = new List<MemberModel>();

        public NamespaceModel NamespaceModel { get; }

        public override string ToString()
        {
            return $"Count of {nameof(Messages)}: {Messages.Count} ;; Count of {nameof(Members)}: {Members.Count} ;; {nameof(Name)}: {Name}";
        }
    }
}