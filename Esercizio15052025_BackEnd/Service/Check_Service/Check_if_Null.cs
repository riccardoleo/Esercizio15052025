namespace Esercizio15052025.Service.Check_Service
{
    public class Check_if_Null
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public static void CheckString(String name)
        {

            if (string.IsNullOrWhiteSpace(name))
            {
                Logger.Error(name);
                throw new InvalidOperationException("Il campo è obbligatorio.");
            }

        }

        public static void CheckInt(int id)
        {
            if (id == 0 || id == null)
            {
                Logger.Error(id);
                throw new InvalidOperationException("Il campo è obbligatorio.");
            }

        }
    }
}
