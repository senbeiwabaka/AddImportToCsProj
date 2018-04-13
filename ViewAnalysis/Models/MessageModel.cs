using ViewAnalysis.Enums;
using ViewAnalysis.Models.Rules;

namespace ViewAnalysis.Models
{
    internal sealed class MessageModel : BaseModel
    {
        public MessageModel()
        {
        }

        public MessageModel(BaseModel model)
        {
            Model = model;
        }

        public string Category { get; set; }

        public string CheckId { get; set; }

        public FixCategories FixCategory { get; set; }

        public string Id { get; set; }

        public IssueModel Issue { get; set; }

        public Statuses Status { get; set; }

        public string TypeName { get; set; }

        public RuleModel Rule { get; set; }

        public BaseModel Model { get; }

        public override string Name => ToString();

        /// <inheritdoc />
        public override string ToString()
        {
            return base.ToString();
        }
    }
}