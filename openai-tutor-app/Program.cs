using System;
using System.Collections.Generic;
using System.Text;
using RestSharp;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace OpenAITutor
{
    class Program
    {
        static IConfigurationRoot Configuration;
        static OpenAISettings OpenAISettings;
        
        static void Main(string[] args)
        {
            BuildConfiguration();

            Console.Write("Welcome to the OpenAI Tutor! Ask me any question:\n");
            Console.Write("Type /restart to start a new conversation.\n");
    
            StringBuilder conversationHistory = new StringBuilder();

            while (true)
            {
                string userQuestion = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(userQuestion))
                {
                    Console.WriteLine("Bye! Have a great day!");
                    break;
                }

                if (userQuestion.ToLower() == "/restart")
                {
                    conversationHistory.Clear();
                    Console.WriteLine("Conversation restarted. You can ask me a new question.");
                    continue;
                }

                conversationHistory.AppendLine($"User: {userQuestion}");
                string answer = GetAnswerFromOpenAI(userQuestion, conversationHistory.ToString());
                conversationHistory.AppendLine($"Tutor: {answer}");
                Console.Write(answer.Trim() + "\n");
                Console.WriteLine("-------------------------------------------------"); // Separator
            }
        }

        
        private static void BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();
            OpenAISettings = Configuration.GetSection("OpenAI").Get<OpenAISettings>();
        }


        private static string GetAnswerFromOpenAI(string userQuestion, string conversationHistory)
        {
            string openaiApiKey = OpenAISettings.ApiKey;
            string baseUrl = OpenAISettings.BaseUrl;
            string model = OpenAISettings.Model;
            string apiUrl = $"{baseUrl}/chat/completions";

            var client = new RestClient(apiUrl);
            var request = new RestRequest();

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", $"Bearer {openaiApiKey}");

            var messages = new List<object>
            {
                new { role = "system", content = "You are a helpful tutor." },
                new { role = "user", content = conversationHistory.Trim() },
                new { role = "user", content = userQuestion.Trim() }
            };

            var input = new
            {
                model = model,
                messages = messages,
                max_tokens = 100,
                n = 1,
                stop = "\n",
                temperature = 0.5,
            };

            request.AddJsonBody(JsonConvert.SerializeObject(input));

            var response = client.Execute(request, Method.Post);

            if (response.IsSuccessful)
            {
                dynamic jsonResponse = JsonConvert.DeserializeObject(response.Content);
                string answer = jsonResponse.choices[0].message.content;
                return answer.Trim();
            }
            else
            {
                Console.WriteLine("Error: " + response.ErrorMessage);
                Console.WriteLine("Status Code: " + response.StatusCode);
                Console.WriteLine("Response Content: " + response.Content);
                return "Sorry, I couldn't process your request.";
            }
        }



    }
}
