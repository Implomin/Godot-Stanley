package handler

import (
	"bytes"
	"encoding/json"
	"fmt"
	"io"
	"log"
	"net/http"
	"os"
	"os/exec"
	"strings"

	"github.com/JakubC0I/voiceover/model"
)

var commandsImpl = model.Standard_commands

func match_command(action string) string {

	switch action {
	case "TURN":
		return commandsImpl.Turn
	case "STAND":
		return commandsImpl.Stand
	case "WALK":
		return commandsImpl.Walk
	case "JUMP":
		return commandsImpl.Jump
	default:
		return commandsImpl.Other
	}
}

func format_prompt(prompt_request model.Prompt_request) []byte {

	var action_list string

	for _, action := range prompt_request.Actions {
		fmt.Println(action.Action, action.Value)
		action_list += fmt.Sprintf("%s %s, ", match_command(action.Action), action.Value)
	}

	prompt := "Make a 30 words, funny, a bit mean, joke about a player who:" + action_list + "."

	fmt.Println(prompt)

	request := model.Ollama_request{Model: "tinydolphin", Prompt: prompt, Stream: false}
	result, err := json.Marshal(request)
	if err != nil {
		log.Fatal(err)
	}
	return result
}

func get_prompt(prompt_json []byte) model.Ollama_response {

	resp, err := http.Post("http://localhost:11434/api/generate", "application/json", bytes.NewBuffer(prompt_json))
	if err != nil {
		log.Fatal(err)
	}
	defer resp.Body.Close()

	result, err := io.ReadAll(resp.Body)
	if err != nil {
		log.Fatal(err)
	}

	r := model.Ollama_response{}
	json.Unmarshal(result, &r)

	return r
}

func delegate_to_tts(prompt string) {
	cmd := exec.Command(
		"tts",
		"--text",
		prompt,
		"--model_name",
		"tts_models/multilingual/multi-dataset/your_tts",
		"--speaker_wav=resources/inputs/output.wav",
		"--language_idx=en",
		"--out_path",
		"resources/outputs/speech.wav")
	cmd.Run()
}

func read_tts(path string, w http.ResponseWriter) {
	file, err := os.Open(path)
	if err != nil {
		log.Fatal(err)
	}
	defer file.Close()

	buf := make([]byte, 32*1024)
	for {
		n, err := file.Read(buf)

		if n > 0 {
			w.Write(buf)
		}

		if err == io.EOF {
			break
		}
		if err != nil {
			log.Printf("read %d bytes: %v", n, err)
			break
		}
	}
}

func Handle_prompt(w http.ResponseWriter, r *http.Request) {
	defer r.Body.Close()
	result, err := io.ReadAll(r.Body)
	if err != nil {
		log.Fatal(err)
	}

	prompt_request := model.Prompt_request{}
	json.Unmarshal(result, &prompt_request)

	prompt := format_prompt(prompt_request)
	ollama_response_body := get_prompt(prompt)
	ollama_response := strings.ReplaceAll(ollama_response_body.Response, "\"", "\\\"")

	//	p := fmt.Sprintf(`{"prompt": "%s"}`, ollama_response)
	fmt.Println(ollama_response)

	delegate_to_tts(ollama_response)
	w.Header().Add("Content-Type", "audio/wav")
	read_tts("resources/outputs/speech.wav", w)
	// w.Header().Add("Content-Type", "application/json")
	// w.Write([]byte(p))
}
