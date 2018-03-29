using System.Collections.Generic;

namespace ViewAnalysis.Models.Targets
{
    internal sealed class TargetModel : BaseModel
    {
        public List<ModuleModel> Modules { get; set; } = new List<ModuleModel>();

        public override string ToString()
        {
            return $"Count of {nameof(Modules)}: {Modules.Count} ;; {nameof(Name)}: {Name}";
        }
    }
}