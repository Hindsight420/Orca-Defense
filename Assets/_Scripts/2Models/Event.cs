using System;

namespace EventCallbacks
{
    public abstract class Event<T> where T : Event<T>
    {
        private bool _hasFired;
        private static event EventListener Listeners;
        public delegate void EventListener(T info);

        public static void RegisterListener(EventListener listener)
        {
            Listeners += listener;
        }

        public static void UnregisterListener(EventListener listener)
        {
            Listeners -= listener;
        }

        public void FireEvent()
        {
            if (_hasFired)
            {
                throw new Exception("This event has already fired, to prevent infinite loops you can't refire an event");
            }
            _hasFired = true;

            Listeners?.Invoke(this as T);
        }
    }

    public class DebugEvent : Event<DebugEvent>
    {
        public int VerbosityLevel;
    }

    public class IncomeEvent : Event<IncomeEvent>
    {
        public ResourceList Resources;

        public void FireEvent(ResourceList resources)
        {
            Resources = resources;
            FireEvent();
        }
    }

    public class ResourcesChangedEvent : Event<ResourcesChangedEvent>
    {
        public ResourceList ResourceList;

        public void FireEvent(ResourceList resources)
        {
            ResourceList = resources;
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
