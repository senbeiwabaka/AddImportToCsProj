using System.Collections.Generic;

namespace ViewAnalysis.Models.Targets
{
    internal sealed class ModuleModel : BaseModel
    {
        public ModuleModel()
        {
        }

        public ModuleModel(TargetModel targetModel) : this()
        {
            TargetModel = targetModel;
        }

        public ModuleModel(TypeModel typeModel) : this()
        {
            TypeModel = typeModel;
        }

        public List<MessageModel> Messages { get; set; } = new List<MessageModel>();

        public List<NamespaceModel> Namespaces { get; set; } = new List<NamespaceModel>();

        public TargetModel TargetModel { get; }

        public TypeModel TypeModel { get; }
    }
}