using Newtonsoft.Json;

namespace web.apis.Models
{
    public class ResponseModel
	{
		public ResponseModel(){}
		
        public ResponseModel(string status, bool hasError, object data, bool maxIdeaReached = false)
		{
			Status = status;
			HasError = hasError;
			Data = data;
			MaxIdeaReached = maxIdeaReached;
		}

        public ResponseModel(object obj)
        {
            Status = "Generated";
            HasError = false;
            Data = obj ?? JsonConvert.SerializeObject(obj);
            MaxIdeaReached = false;
        }

        public string Status { get; set; }
		public bool HasError { get; set; }
		public object Data { get; set; }
		public bool MaxIdeaReached { get; set; }
	}
}

