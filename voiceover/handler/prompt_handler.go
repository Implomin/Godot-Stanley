package handler

import (
	"io"
	"log"
	"net/http"
	"os"
)

func send_tts(path string, w http.ResponseWriter) {
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

// TODO: replace speech.wav with file with an id and then remove or make everything in memory
func Handle_voice(w http.ResponseWriter, r *http.Request) {
	// delegate_to_tts(ollama_response)
	// w.Header().Add("Content-Type", "audio/wav")
	// send_tts("resources/outputs/speech.wav", w)
	// w.Header().Add("Content-Type", "application/json")
	// w.Write([]byte(p))
}
