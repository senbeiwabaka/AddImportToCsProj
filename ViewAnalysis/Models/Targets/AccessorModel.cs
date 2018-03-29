using System.Collections.Generic;
using ViewAnalysis.Enums;

namespace ViewAnalysis.Models.Targets
{
    internal sealed class AccessorModel : BaseModel
    {
        public AccessorModel() : this(null)
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

        public List<MessageModel> Messages { get; set; } = new List<MessageModel>();

        public MemberModel MemberModel { get; }
    }
}