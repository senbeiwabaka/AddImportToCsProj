using ViewAnalysis.Enums;
using ViewAnalysis.Models.Rules;
using ViewAnalysis.Models.Targets;

namespace ViewAnalysis.Models
{
    internal sealed class MessageModel : BaseModel
    {
        public MessageModel()
        {
        }
        
        public MessageModel(BaseModel baseModel)
        {
            NamespaceModel = baseModel as NamespaceModel;
            ModuleModel = baseModel as ModuleModel;
            MemberModel = baseModel as MemberModel;
            TypeModel = baseModel as TypeModel;
            AccessorModel = baseModel as AccessorModel;
        }

        public string Category { get; set; }

        public string CheckId { get; set; }

        public FixCategories FixCategory { get; set; }

        public string Id { get; set; }

        public IssueModel Issue { get; set; }

        public Statuses Status { get; set; }

        public string TypeName { get; set; }

        public RuleModel Rule { get; set; }

        public MemberModel MemberModel { get; }

        public ModuleModel ModuleModel { get; }
        
        public TypeModel TypeModel { get; }
        
        public NamespaceModel NamespaceModel { get; }

        public AccessorModel AccessorModel { get; }

        public override string Name
        {
            get
            {
                return ToString();
            }
        }
        
        public override string ToString()
        {
            return Issue == null ? $"{nameof(MessageModel)}" : !string.IsNullOrWhiteSpace(Issue.Name) ? $"Issue Name: {Issue.Name}" : $"{nameof(MessageModel)}";
        }
    }
}