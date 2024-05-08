## Goal
Create a script allowing for easy narration creation

### How to install ollama
Use a Linux-based command below or [download](https://ollama.com/download)
```shell
curl -fsSL https://ollama.com/install.sh | sh
```

### How to install tinydolphin
- Tinydolphin is a small model based (650MB) on tiny llama. It is uncensored making conversations a bit funnier.
- Proposed prompt for our use case:
  - **Make a short funny, a bit mean, narration about a player who: saw sign "turn right", turned left.**
```shell
ollama run tinydolphin
```


### How to install tts
- Python 3.9 required
```shell
pip install TTS
```

#### How to run:
Use the following command to start a local tts server on port 5002
```shell
~~tts --text "Example string." --model_name "tts_models/multilingual/multi-dataset/your_tts" --speaker_wav=resources/inputs/output.wav  --language_idx=en --out_path resources/outputs/speech.wav~~
tts-server --model_name tts_models/multilingual/multi-dataset/your_tts
```

### Proposed json structure for our application
Predefined actions: [TURN, SEE, PICK, STAND, WALK, OTHER]
```json
{
  "actions": [
    {
      "action": "TURN",
      "value": "LEFT"
    },
    {
      "action": "SEE",
      "value": "SIGN SAYING TURN RIGHT"
    },
    {
      "action": "PICK",
      "value": "BALL"
    },
    {
      "action": "STAND",
      "value": "5 HOURS"
    },
    {
      "action": "OTHER",
      "value": "PLAYER JUMPS IN PLACE 20 TIMES"
    }
  ]
}
```


### UPDATE 08.04.2024

- Right now you can test API endpoint POST (http:localhost:5051) with 6 defined actions (see server/action.rs)
- Make sure you have ollama ready to serve and have downloaded tinydolphin
```bash
cd ./voiceover
cargo run
```
- Example of a curl:
```bash
curl --location '127.0.0.1:5051' \
--header 'Content-Type: application/json' \
--data '{"action":"TURN", "value": "left"}'
```

TODO: add option to pass many actions, rewrite it in go or java because it is getting quite annoying :)


### UPDATE 27.04.2024

- Moved to golang
- Greatly reduced latency 
- Make sure you have: go installed, ollama and tts-server ready
```bash
go run main.go
```

- Example of a curl (returns audio/wav):
```bash
curl --location '127.0.0.1:5051/prompt' \
--header 'Content-Type: application/json' \
--data '{"actions": [
    {"action":"TURN", "value": "left"},
    {"action": "STAND", "value": "for five minutes"},
    {"action": "JUMP", "value": "off a bridge"}
    ]
}    '
```
