package handler

import (
	"bytes"
	"encoding/json"
	"fmt"
	"io"
	"log"
	"net/http"
	"net/url"
	"os/exec"
	"strings"

	"github.com/JakubC0I/voiceover/model"
)

var commandsImpl = model.Standard_commands

func match_command(action string) string {

	switch action {
	case "TURN":
		return commandsImpl.Turn
	case "ARRIVE":
		return commandsImpl.Arrive
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

	prompt := "You are angry. Your task is to make a SHORT and FUNNY comment. Make fun of an idiot who: " + action_list + "."

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

func delegate_to_tts(prompt string) (io.ReadCloser, *exec.Cmd) {
	cmd := exec.Command(
		"tts",
		"--text",
		prompt,
		"--model_name",
		"tts_models/multilingual/multi-dataset/your_tts",
		"--speaker_wav=resources/inputs/output.wav",
		"--language_idx=en",
		"--pipe_out")
	// errPipe, err := cmd.StderrPipe(); if err != nil { log.Fatalln(err)}
	stdPipe, err := cmd.StdoutPipe()
	if err != nil {
		log.Fatalln(err)
	}
	return stdPipe, cmd
}

func connect_with_cogui_server(prompt string) io.ReadCloser {
	cogui := "http://localhost:5002/api/tts?"
	params := url.Values{}

	params.Add("text", prompt)
	params.Add("speaker_id", "male-pt-3\n")
	params.Add("style_wav", "")
	params.Add("language_id", "en")

	resp, err := http.Get(cogui + params.Encode())
	if err != nil {
		log.Fatal()
	}
	// b, _ := io.ReadAll(resp.Body)
	// fmt.Printf("Received: %d bytes", len(b))
	return resp.Body
}

func write(reader *io.PipeReader, w http.ResponseWriter) {
	defer reader.Close()
	buf := make([]byte, 16*1024)
	for {
		n, err := reader.Read(buf)

		if n > 0 {
			w.Write(buf)
			fmt.Printf("Wrote %d bytes \n", n)
		}

		if err == io.EOF {
			fmt.Println("EOF")
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
	// pipe, cmd := delegate_to_tts(ollama_response)
	// cmd.Start()
	pipe := connect_with_cogui_server(ollama_response)
	w.Header().Add("Content-Type", "audio/wav")
	written, err := io.Copy(w, pipe)
	if err != nil {
		log.Fatal(err)
	}
	fmt.Printf("Written %d", written)
	// cmd.Wait()
	pipe.Close()
}
