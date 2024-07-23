# Staszek
Lost Java Developer, hoping to find JavaDocs that might help him find his way. In this video game, you can experience this first-hand. Staszek is a 3D first-person game where you, as a Junior Java Developer, are on a quest to find the essential JavaDocs. To accomplish this goal, you receive help from an AI-controlled narrator. Will you follow his instructions, or will you find your own way?

## Requirements
* Godot Engine (.NET) 4.2.1
* pip
* ollama
* GO

## Installation
### Ollama
Install with a Linux-based command below or [download](https://ollama.com/download)
```shell
curl -fsSL https://ollama.com/install.sh | sh
```

```shell
ollama run tinydolphin
```

## TTS
```shell
pip install TTS
```

## Starting the game
In order to start the game you must first start ollama, the tts server aswell as the go server.
Inside of ./voiceover:
```shell
go run main.go
```
```shell
tts-server --model_name tts_models/multilingual/multi-dataset/your_tts
```
```shell
ollama run tinydolphin
```
Now you can start the game inside of Godot with f5.

## Roles in development
- Jakub Ciszak - Backend
- Jarosław Nowak - Godot
- Patryk Schuler - 3D Objects
- Nico Strzedulla - Musik/SFX
