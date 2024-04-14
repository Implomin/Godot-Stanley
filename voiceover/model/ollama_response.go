package model

type Ollama_response struct {
	Model                string  `json:"model"`
	Created_at           string  `json:"created_at"`
	Response             string  `json:"response"`
	Done                 bool    `json:"done"`
	Context              []int32 `json:"context"`
	Total_duration       int64   `json:"total_duration"`
	Load_duration        int32   `json:"load_duration"`
	Prompt_eval_duration int64   `json:"prompt_eval_duration"`
	Eval_count           int32   `json:"eval_count"`
	Eval_duration        int64   `json:"eval_duration"`
}
