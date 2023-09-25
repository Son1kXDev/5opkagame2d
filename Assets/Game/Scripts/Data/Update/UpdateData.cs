namespace Enjine.Data
{
    [System.Serializable]
    public struct UpdateData
    {
        public string version;
        public string url;

        public UpdateData(string version, string url)
        {
            this.version = version;
            this.url = url;
        }
    }
}
