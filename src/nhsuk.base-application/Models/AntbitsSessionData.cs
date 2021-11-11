using System;
using System.Data;
using System.Collections.Generic;
using nhsuk.base_application.ViewModels;

namespace nhsuk.base_application.Models
{
    internal sealed class AntbitsSessionData
    {
        public AntbitsSessionData()
        {
            id = new Guid();
            currentQuestionNumber = 0;
            configData = new ConfigData();
            questions = new List<QuestionData>();
            responses = new List<ResponseData>();
        }

        public Guid id { get; set; }
        public int currentQuestionNumber { get; set; }
        public ConfigData configData { get; set; }
        public List<QuestionData> questions { get; set; }
        public List<ResponseData> responses { get; set; }
    }

    internal sealed class ConfigData
    {
        public ConfigData()
        {
            introCopy = "";
            introTitle = "";
            title = "";
        }
        public string introCopy { get; set; }
        public string introTitle { get; set; }
        public string title { get; set; }
    }

    internal sealed class QuestionData
    {
        public QuestionData()
        {
            type = "";
            body = "";
            answers = new List<Answer>();
        }

        public string type { get; set; }
        public string body { get; set; }
        public List<Answer> answers { get; set; }
    }

    internal sealed class Answer
    {
        public Answer()
        {
            body = "";
            actions = new List<AnswerAction>();
        }

        public string body { get; set; }
        public List<AnswerAction> actions { get; set; }
    }

    internal sealed class AnswerAction
    {
        public AnswerAction()
        {
            type = "";
            value = "";
            actionOperator = "";
        }

        public string type { get; set; }
        public string value { get; set; }
        public string actionOperator { get; set; }
    }

    internal sealed class ResponseData
    {
        public ResponseData()
        {
            responseDetails = new List<Dictionary<int, ResponseDetails>>();
        }

        public List<Dictionary<int, ResponseDetails>> responseDetails { get; set; }
    }

    internal sealed class ResponseDetails
    {
        public ResponseDetails()
        {
            responseBody = "";
            actions = new List<Action>();
        }

        public string responseBody { get; set; }
        public List<Action> actions { get; set; }
    }
}
