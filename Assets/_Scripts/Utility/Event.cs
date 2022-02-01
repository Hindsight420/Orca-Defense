using System;

namespace EventCallbacks
{
    public abstract class Event<T> where T : Event<T>
    {
        /*
         * The base Event,
         * might have some generic text
         * for doing Debug.Log?
         */
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
            if (listeners != null)
            {
                listeners(this as T);
            }
        }
    }

    public class DebugEvent : Event<DebugEvent>
    {
        public int VerbosityLevel;
    }


    // TODO: Figure out a way to do generic BuildingEvents
    //public class BuildingEvent : Event<BuildingEvent>
    //{
    //    public Building building;

    //    public void FireEvent(Building building)
    //    {
    //        this.building = building;
    //        FireEvent();
    //    }
    //}

    public class BuildingCreatedEvent : Event<BuildingCreatedEvent>
    {
        public Building building;

        public void FireEvent(Building building)
        {
            this.building = building;
            FireEvent();
        }
    }

    public class BuildingChangedEvent : Event<BuildingChangedEvent>
    {
        public Building building;

        public void FireEvent(Building building)
        {
            this.building = building;
            FireEvent();
        }
    }

    public class BuildingRemovedEvent : Event<BuildingRemovedEvent>
    {
        public Building building;

        public void FireEvent(Building building)
        {
            this.building = building;
            FireEvent();
        }
    }
}
