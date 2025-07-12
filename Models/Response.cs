using Models.Enums;
using System;

namespace Models
{
    public class Response
    {
         public static Response Error(string message, StatusCodes statusCode, object model = null)
         {
             var res =  new Response { ErrorMessage = message, ErrorCode = statusCode.ToInt(), Model = model };
             if (model != null) res.Model = model;
             return res;
         }
         public static Response Error(string message, int statusCode = 0, object model = null)
         {
            var res = new Response { ErrorMessage = message, ErrorCode = statusCode == 0? StatusCodes.Error_Occured.ToInt() : statusCode};
            if (model != null) res.Model = model;
            return res;
        }
        public static Response Message(string message, StatusCodes statusCode, object model = null)
         {
             var res = new Response { ResponseMessage = message, ResponseCode = statusCode.ToInt() };
             if (model != null) res.Model = model;
             return res;
        }
        public static Response Message(string message, int statusCode = 0, object model = null)
        {
            var res = new Response { ResponseMessage = message, ResponseCode = statusCode == 0? StatusCodes.OK.ToInt() : statusCode };
            if (model != null) res.Model = model;
            return res;
        }

        public int ResponseCode { get; set; }

        // ReSharper disable once InconsistentNaming
        private string responseName = "";
        public string ResponseName
        {
            get => responseName.ToLower();
            set => responseName = value;
        }

        public bool ErrorOccured => !string.IsNullOrEmpty(ErrorMessage) | ErrorCode != 0;
        public int ErrorCode { get; set; }
        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage ?? Enum.GetName(typeof(StatusCodes), ErrorCode);
            set => _errorMessage = value;
        }

        public void SetError(string errorMessage, int errorCode = 0, object model = null)
        {
            _errorMessage = errorMessage;
            ErrorCode = errorCode==0? StatusCodes.Error_Occured.ToInt():errorCode;
            if (model != null) Model = model;
        }
        public void SetError(string errorMessage, StatusCodes errorCode, object model = null)
        {
            _errorMessage = errorMessage;
            ErrorCode = errorCode.ToInt();
            if (model != null) Model = model;
        }
        public void SetMessage(string responseMessage, int responseCode = 0, object model = null)
        {
            _responseMessage = responseMessage;
            ResponseCode = responseCode == 0 ? StatusCodes.OK.ToInt() : responseCode;
            if (model != null) Model = model;
        }
        public void SetMessage(string responseMessage, StatusCodes responseCode, object model = null)
        {
            _responseMessage = responseMessage;
            ResponseCode = responseCode.ToInt();
            if (model != null) Model = model;
        }

        private string _responseMessage;
        public string ResponseMessage
        {
            get => _responseMessage ?? Enum.GetName(typeof(StatusCodes), ErrorCode);
            set => _responseMessage = value;
        }

        public object Model { get; set; }
    }
    public static class ObjectExtensions
    {
        public static string String(this object o) => o != null ? o.ToString() : "";
    }
}