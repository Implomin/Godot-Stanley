package handler

import (
	"fmt"
	"net/http"

	"github.com/JakubC0I/voiceover/services"
)

func Handle_text(w http.ResponseWriter, r *http.Request) {
	fmt.Println("sending prompt text...")
	w.Header().Add("Content-Type", "text/plain")
	w.Write([]byte(services.GetPrompt()))
}
