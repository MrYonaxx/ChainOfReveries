
using VoiceActing;

namespace BehaviorDesigner.Runtime
{
    [System.Serializable]
    public class SharedAIController : SharedVariable<AIController>
    {
        public static implicit operator SharedAIController(AIController value) { return new SharedAIController { mValue = value }; }
    }
}

