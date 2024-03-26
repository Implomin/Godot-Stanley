## Goal
Create script allowing for easy narration creation

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
```shell
tts --text "Example string." --model_name "tts_models/multilingual/multi-dataset/your_tts" --speaker_wav=resources/inputs/output.wav  --language_idx=en --out_path resources/outputs/speech.wav
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
