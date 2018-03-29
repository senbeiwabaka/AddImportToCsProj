using System.Collections.Generic;
using ViewAnalysis.Models.Targets;

namespace ViewAnalysis.Models
{
    internal sealed class NamespaceModel : BaseModel
    {
        public NamespaceModel() : this(null)
        {
        }

        public NamespaceModel(ModuleModel moduleModel)
        {
            ModuleModel = moduleModel;
        }

        public List<MessageModel> Messages = new List<MessageModel>();

        public List<TypeModel> Types = new List<TypeModel>();

        public ModuleModel ModuleModel { get; }

        public override string ToString()
        {
            return $"Count of {nameof(Messages)}: {Messages.Count} ;; Count of {nameof(Types)}: {Types.Count} ;; {nameof(Name)}: {Name}";
        }
    }
}