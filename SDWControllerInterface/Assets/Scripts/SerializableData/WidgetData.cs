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
        public int x, y; // grid coords for all widgets
    }

    [Serializable]
    public class NoteWidgetData : WidgetData
    {
        public string note;
    }

    [Serializable]
    public class FirefoxWidgetData : WidgetData
    {
        public string url;
    }

    [Serializable]
    public class ConsoleWidgetData : WidgetData
    {
        public string logPath;
    }

    [Serializable]
    public class AppsWidgetData : WidgetData
    {
        public AppData[] apps;
    }
}
