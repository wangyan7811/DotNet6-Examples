namespace ReadWriteAppSettingsJson.Helpers
{
    public interface ICanReadWriteJson
    {
        public T Read<T>();

        public bool Write<T>(T  entity);

    }
}
