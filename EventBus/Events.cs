using System.Collections.Generic;
using UnityEngine;

namespace OghiUnityTools.EventBus
{
    public struct TestEvent : IEvent { }

    public struct PlayerEvent : IEvent {
        public int health;
        public int mana;
    }
    
    public struct HealthEvent : IEvent
    {
        public int Health;
    }

    public struct LayoutsLoadingEvent : IEvent
    {
        public HashSet<AudioClip> AudioClips;
    }

}