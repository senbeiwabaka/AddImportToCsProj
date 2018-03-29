using System;
using System.Collections.Generic;

namespace ViewAnalysis.Models.Rules
{
    internal sealed class RuleModel : BaseModel, IEquatable<RuleModel>
    {
        public string TypeName { get; set; }

        public string Category { get; set; }

        public string CheckId { get; set; }

        public string Description { get; set; }

        public ResolutionModel Resolution { get; set; }

        public string Owner { get; set; }

        public string Url { get; set; }

        public MessageLevelModel MessageLevel { get; set; }

        public bool Equals(RuleModel other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return CheckId.Equals(other.CheckId) && Category.Equals(other.Category) && TypeName.Equals(other.TypeName);
        }

        public override string ToString()
        {
            return $"{nameof(CheckId)}: {CheckId}";
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((RuleModel)obj);
        }

        public override int GetHashCode()
        {
            var hashCode = 1863327797;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(TypeName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Category);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CheckId);
            //hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Description);
            //hashCode = hashCode * -1521134295 + EqualityComparer<ResolutionModel>.Default.GetHashCode(Resolution);
            //hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Owner);
            //hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Url);
            //hashCode = hashCode * -1521134295 + EqualityComparer<MessageLevelModel>.Default.GetHashCode(MessageLevel);
            return hashCode;
        }
    }
}