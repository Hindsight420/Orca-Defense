using System;

namespace EventCallbacks
{
    public abstract class Event<T> where T : Event<T>
    {
        public string Description;

        private bool hasFired;
        public delegate void EventListener(T info);
        private static event EventListener listeners;

        public static void RegisterListener(EventListener listener)
        {
            listeners += listener;
        }

        public static void UnregisterListener(EventListener listener)
        {
            listeners -= listener;
        }

        public void FireEvent()
        {
            if (hasFired)
            {
                throw new Exception("This event has already fired, to prevent infinite loops you can't refire an event");
            }
            hasFired = true;

            listeners?.Invoke(this as T);
        }
    }

    public class DebugEvent : Event<DebugEvent>
    {
        public int VerbosityLevel;
    }

    public class ResourceValueChangedEvent : Event<ResourceValueChangedEvent>
    {
        public ResourceValue ResourceValue;

        public void FireEvent(ResourceValue resourceValue)
        {
            ResourceValue = resourceValue;
            FireEvent();
        }
    }

    public class BuildingEventBase<T> : Event<T> where T : Event<T>
    {
        public Building Building;

        public void FireEvent(Building building)
        {
            Building = building;
            FireEvent();
        }
    }

    public class BuildingCreatedEvent : BuildingEventBase<BuildingCreatedEvent>
    {
        
    }

    public class BuildingChangedEvent : BuildingEventBase<BuildingChangedEvent>
    {
        
    }

    public class BuildingRemovedEvent : BuildingEventBase<BuildingRemovedEvent>
    {
        
    }
}