using System;
using ViewAnalysis.Enums;

namespace ViewAnalysis.Models
{
    internal sealed class IssueModel : BaseModel
    {
        public IssueModel() : this(null)
        {
        }

        public IssueModel(MessageModel messageModel)
        {
            MessageModel = messageModel;
        }
        
        public int Certainty { get; set; }

        public Levels Level { get; set; }

        public string Text { get; set; }

        public string Path { get; set; }

        public string File { get; set; }

        public int? Line { get; set; }

        public string FixCategory
        {
            get
            {
                return MessageModel != null ? Enum.GetName(typeof(FixCategories), MessageModel.FixCategory).ToString() : string.Empty;
            }
        }

        public MessageModel MessageModel { get; }

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name} ;; {nameof(MessageModel)}: {MessageModel}";
        }
    }
}