package services

var prompt string

func GetPrompt() string {
	return prompt
}

func SetPrompt(new_prompt string) {
	prompt = new_prompt
}
