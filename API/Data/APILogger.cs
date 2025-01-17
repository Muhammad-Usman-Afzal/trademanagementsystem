namespace API.Data;

public static class APILogger
{
    public static void WriteLog(Exception ex)
    {
        string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
        message += Environment.NewLine;
        message += "-----------------------------------------------------------";
        message += Environment.NewLine;
        message += string.Format("Message: {0}", ex.Message);
        message += Environment.NewLine;
        message += string.Format("InnerException: {0}", ex.InnerException);
        message += Environment.NewLine;
        message += string.Format("StackTrace: {0}", ex.StackTrace);
        message += Environment.NewLine;
        message += string.Format("Source: {0}", ex.Source);
        message += Environment.NewLine;
        message += string.Format("TargetSite: {0}", ex.TargetSite.ToString());
        message += Environment.NewLine;
        message += "-----------------------------------------------------------";
        message += Environment.NewLine;
        string pathatakiah = Environment.CurrentDirectory;
        string path = Environment.CurrentDirectory + "/APILogs/APILog.txt";
        using (StreamWriter writer = new StreamWriter(path, true))
        {
            writer.WriteLine(message);
            writer.Close();
        }
    }

    public static void WriteLog(string ex)
    {
        string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
        message += Environment.NewLine;
        message += "-----------------------------------------------------------";
        message += Environment.NewLine;
        message += string.Format("Message: {0}", ex);
        //message += Environment.NewLine;
        //message += string.Format("InnerException: {0}", ex.InnerException);
        //message += Environment.NewLine;
        //message += string.Format("StackTrace: {0}", ex.StackTrace);
        //message += Environment.NewLine;
        //message += string.Format("Source: {0}", ex.Source);
        //message += Environment.NewLine;
        //message += string.Format("TargetSite: {0}", ex.TargetSite.ToString());
        message += Environment.NewLine;
        message += "-----------------------------------------------------------";
        message += Environment.NewLine;
        string pathatakiah = Environment.CurrentDirectory;
        string path = Environment.CurrentDirectory + "/APILogs/APILog.txt";
        using (StreamWriter writer = new StreamWriter(path, true))
        {
            writer.WriteLine(message);
            writer.Close();
        }
    }

}
