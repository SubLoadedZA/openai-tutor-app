# OpenAI Tutor

The OpenAI Tutor is a console-based application that leverages OpenAI's GPT-3.5-turbo model to provide a conversational AI tutor experience. Users can ask questions, and the AI will provide answers on a wide range of topics.

## Prerequisites

- .NET 6
- An OpenAI API key (sign up at [OpenAI](https://beta.openai.com/signup/))

## Setup

1. Clone the repository:
git clone https://github.com/SubLoadedZA/openai-tutor.git

2. Change the directory to the cloned repository:
cd openai-tutor

3. Open the `appsettings.json` file and replace `your_openai_api_key_here` with your actual OpenAI API key:

```json
{
  "OpenAI": {
    "ApiKey": "your_openai_api_key_here"
  }
}
```

4. Run the project.

5. The OpenAI Tutor will prompt you to ask any question. Type your question and press enter

6. To restart the conversation, type /restart and press Enter.

## License 
This project is licensed under the MIT License. 
