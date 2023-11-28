using System;

namespace SerializableData
{
    [Serializable]
    public class WidgetsData
    {
        public WidgetData[] widgetData; // all widgets
    }
    
    [Serializable]
    public class WidgetData
    {
        public string title;
        public int x;
        public int y;

        public WidgetData() { }
        public WidgetData(WidgetData source)
        {
            title = source.title;
            x = source.x;
            y = source.y;
        }

    }

    [Serializable]
    public class NoteWidgetData : WidgetData
    {
        public string note;
        public NoteWidgetData() { }
        public NoteWidgetData(WidgetData source) : base(source) { }
    }

    [Serializable]
    public class FirefoxWidgetData : WidgetData
    {
        public string url;
        public FirefoxWidgetData() { }
        public FirefoxWidgetData(WidgetData source) : base(source) { }
    }

    [Serializable]
    public class ConsoleWidgetData : WidgetData
    {
        public string logPath;
        public ConsoleWidgetData() { }
        public ConsoleWidgetData(WidgetData source) : base(source) { }
    }

    [Serializable]
    public class AppsWidgetData : WidgetData
    {
        public AppData[] apps;
        public AppsWidgetData() { }
        public AppsWidgetData(WidgetData source) : base(source) { }
    }

    [Serializable]
    public class ClockWidgetData : WidgetData
    {
        public int format;
        public ClockWidgetData() { }
        public ClockWidgetData(WidgetData source) : base(source) { }
    }
    
    [Serializable] public class IPAddressWidgetData : WidgetData
    {
        public IPAddressWidgetData() { }
        public IPAddressWidgetData(WidgetData source) : base(source) { }
    }
    [Serializable] public class PlaybackWidgetData : WidgetData
    {
        public PlaybackWidgetData() { }
        public PlaybackWidgetData(WidgetData source) : base(source) { }
    }
}
