package main

import (
	"fmt"
	"log"
	"net/http"

	"github.com/JakubC0I/voiceover/handler"
)

func main() {

	http.HandleFunc("/text", handler.Handle_text)
	http.HandleFunc("/prompt", handler.Handle_prompt)

	fmt.Println("LISTENING ON PORT 5051")
	if err := http.ListenAndServe(":5051", nil); err != nil {
		log.Fatal(err)

	}

}
