package model

type Prompt_request struct {
	Actions []action_value `json:"actions"`
}

type action_value struct {
	Action string `json:"action"`
	Value  string `json:"value"`
}
