using System;
using System.Web.Script.Serialization;
using ICSharpCode.AvalonEdit.Document;

namespace RazorPad.Providers
{
    public class JsonModelProvider : ModelProvider
    {
        public string Json
        {
            get { return _json; }
            set
            {
                if (_json == value)
                    return;

                _json = value;
                TriggerModelChanged();
            }
        }
        private string _json;

        public JsonModelProvider(Type modelType = null, string json = null)
            : base(modelType)
        {
            Json = json;

            if (!string.IsNullOrWhiteSpace(json))
                JsonDocument = new TextDocument(json);
            else
            {
                JsonDocument = new TextDocument();
            }
        }

        protected TextDocument JsonDocument
        {
            get;
            set;
        }

        protected override dynamic RebuildModel()
        {
            var serializer = new JavaScriptSerializer();

            var json = (string.IsNullOrWhiteSpace(Json)) ? "{}" : Json;
            var modelType = ModelType ?? typeof (object);

            if (modelType == typeof(object))
                return serializer.DeserializeObject(json);
            else
                return serializer.Deserialize(json, modelType);
        }
    }
}